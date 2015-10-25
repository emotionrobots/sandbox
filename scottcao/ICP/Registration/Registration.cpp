#include "Registration.h"

Registration::Registration() 
{
  clear();
}

// Registration::Registration (int cr, float in_f, int ns, 
//         float st, float mcd, int mi)
// {
//   clear ();
//   setRegistrationParameters(cr, in_f, ns, st, mcd, mi);
// }

void Registration::setRegistrationParameters(float msd, float mcd, 
  int mi, float rort, float te)
{
  // sac_ia_.setMinSampleDistance(0.001);
  // // Set the maximum distance (squared) threshold between two correspondent points in source and target.
  // // If the distance squared is larger, the points will be ignored in the alignment process.
  // sac_ia_.setMaxCorrespondenceDistance(0.005*0.005);
  // // Set the number of iterations
  // sac_ia_.setMaximumIterations(600);

  // reg_.setRANSACOutlierRejectionThreshold(rort);
  // reg_.setTransformationEpsilon(te);
  // // Set the maximum distance between two correspondences (src<->tgt) to 10cm
  // // Note: adjust this based on the size of your datasets
  // reg_.setMaxCorrespondenceDistance(0.001);  
  // // reg_.setEuclideanFitnessEpsilon(mcd);
  // // Set the number of iterations
  // reg_.setMaximumIterations(mi);
}

void Registration::setInputCloudParameters (float rl, float ls1, float ls2,
        float k, float fr, u_int i)
{
  source_ = CustomCloud(rl, ls1, ls2, k, fr, i);
  target_ = CustomCloud(rl, ls1, ls2, k, fr, i);
}

void Registration::setInputCloud (PointCloud::ConstPtr cloud)
{
  if (target_.getPointCloud())
  {
    if (converge_)
    {
      source_ = CustomCloud (target_);
    }
    target_.setInputCloud (cloud);
  }
  else 
  {
    target_.setInputCloud (cloud);
    *result_ += *(target_.getPointCloud ());
  }
}

void Registration::loadInputCloud (std::string file_name)
{
  if (target_.getPointCloud())
  {
    if (converge_)
    {
      source_ = CustomCloud (target_);
    }
    target_.loadInputCloud (file_name);
  }
  else 
  {
    target_.loadInputCloud (file_name);
    *result_ += *(target_.getPointCloud ());
  }
}

void Registration::clear ()
{
  converge_ = true;
  result_ = PointCloud::Ptr (new PointCloud);
}

bool Registration::canAlign ()
{
  return target_.getPointCloud() && source_.getPointCloud();
}

void Registration::findCorrespondences (PointCloudWithNormals::Ptr src, 
                                        PointCloudWithNormals::Ptr tgt, 
                                        pcl::Correspondences &all_correspondences)
{
  // pcl::registration::CorrespondenceEstimationNormalShooting<PointT, PointT, PointT> est;
  pcl::registration::CorrespondenceEstimation<PointNormalT, PointNormalT> est;
  // pcl::registration::CorrespondenceEstimationBackProjection<PointT, PointT, PointT> est;
  est.setInputSource (src);
  est.setInputTarget (tgt);
  
  // est.setSourceNormals (src);
  // est.setTargetNormals (tgt);
  // est.setKSearch (10);
  est.determineCorrespondences (all_correspondences, 0.05);
  //est.determineReciprocalCorrespondences (all_correspondences);
}

void Registration::rejectBadCorrespondences (pcl::CorrespondencesPtr &all_correspondences,
                                            PointCloudWithNormals::Ptr src,
                               PointCloudWithNormals::Ptr tgt,
                               pcl::Correspondences &remaining_correspondences)
{
  pcl::registration::CorrespondenceRejectorMedianDistance rej;
  rej.setMedianFactor (4);
  rej.setInputCorrespondences (all_correspondences);

  // rej.getCorrespondences (remaining_correspondences);
  // return;
  
  pcl::CorrespondencesPtr remaining_correspondences_temp (new pcl::Correspondences);
  rej.getCorrespondences (*remaining_correspondences_temp);
  PCL_INFO ("[rejectBadCorrespondences] Number of correspondences remaining after rejection: %d\n", remaining_correspondences_temp->size ());

  // Reject if the angle between the normals is really off
  pcl::registration::CorrespondenceRejectorSurfaceNormal rej_normals;
  rej_normals.setThreshold (acos (pcl::deg2rad (45.0)));
  rej_normals.initializeDataContainer<PointNormalT, PointNormalT> ();
  rej_normals.setInputSource<PointNormalT> (src);
  rej_normals.setInputNormals<PointNormalT, PointNormalT> (src);
  rej_normals.setInputTarget<PointNormalT> (tgt);
  rej_normals.setTargetNormals<PointNormalT, PointNormalT> (tgt);
  rej_normals.setInputCorrespondences (remaining_correspondences_temp);
  rej_normals.getCorrespondences (remaining_correspondences);
}

void Registration::align ()
{
  pcl::registration::TransformationEstimationPointToPlaneWeighted<PointNormalT, PointNormalT, double> trans_est;

  pcl::CorrespondencesPtr all_correspondences (new pcl::Correspondences), 
                     good_correspondences (new pcl::Correspondences);

  PointCloudWithNormals::Ptr output (new PointCloudWithNormals);
  *output = *(source_.getKeypoints());

  Eigen::Matrix4d transform, final_transform (Eigen::Matrix4d::Identity ());

  int iterations = 0;
  pcl::registration::DefaultConvergenceCriteria<double> converged (iterations, transform, *good_correspondences);

  // ICP loop
  do
  {
    // Find correspondences
    findCorrespondences (output, target_.getKeypoints(), *all_correspondences);
    PCL_INFO ("Number of correspondences found: %d\n", all_correspondences->size ());

    // if (rejection)
    // {
      // Reject correspondences
      rejectBadCorrespondences (all_correspondences, output, target_.getKeypoints(), *good_correspondences);
      PCL_INFO ("Number of correspondences remaining after rejection: %d\n", good_correspondences->size ());
    // }
    // else
    //   *good_correspondences = *all_correspondences;

    std::cout << output->points.size() << std::endl;
    std::cout << target_.getKeypoints()->points.size() << std::endl;

    // Find transformation
    trans_est.estimateRigidTransformation (*output, *(target_.getKeypoints()), *good_correspondences, transform);
 
    // Obtain the final transformation    
    final_transform = transform * final_transform;

    // Transform the data
    transformPointCloudWithNormals (*(source_.getKeypoints()), *output, final_transform.cast<float> ());

    // Check if convergence has been reached
    ++iterations;
  
    // Visualize the results
    // view (output, tgt, good_correspondences);
  }
  while (!converged.hasConverged());

  std::cout << "Iterations: " << iterations << std::endl;

  PointCloud::Ptr temp(new PointCloud);
  
  pcl::transformPointCloud(*result_, *temp, final_transform.inverse().cast<float>());
  *temp += *(target_.getFilteredPointCloud());
  result_.swap(temp);
}                     

bool Registration::hasConverged () const
{
  return converge_;
}

double Registration::getFitnessScore () const
{
  return fitness_score_;
}

PointCloud::Ptr Registration::getResultantCloud () const
{
  return result_;
}