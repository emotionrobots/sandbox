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

  reg_.setTransformationEpsilon(1e-16);
  // Set the maximum distance between two correspondences (src<->tgt) to 10cm
  // Note: adjust this based on the size of your datasets
  reg_.setMaxCorrespondenceDistance(0.05);  
  reg_.setEuclideanFitnessEpsilon(0.002);
  // Set the number of iterations
  reg_.setMaximumIterations(200);
  reg_.setRANSACOutlierRejectionThreshold(0.02);
  reg_.setRANSACIterations(200);

  // pcl::registration::CorrespondenceEstimationNormalShooting<PointNormal, PointNormal, PointNormal>::Ptr cens 
  // (new pcl::registration::CorrespondenceEstimationNormalShooting<PointNormal, PointNormal, PointNormal>);
  pcl::registration::CorrespondenceEstimationBackProjection<PointNormalT, PointNormalT, PointNormalT>::Ptr est
    (new pcl::registration::CorrespondenceEstimationBackProjection<PointNormalT, PointNormalT, PointNormalT>);
  reg_.setCorrespondenceEstimation(est);

  // Add rejector
  pcl::registration::CorrespondenceRejectorMedianDistance::Ptr rej_med 
    (new pcl::registration::CorrespondenceRejectorMedianDistance);
  rej_med->setMedianFactor (4);
  reg_.addCorrespondenceRejector (rej_med);

  pcl::registration::CorrespondenceRejectorSurfaceNormal::Ptr rej_normals 
    (new pcl::registration::CorrespondenceRejectorSurfaceNormal);
  // rej_normals->setThreshold (0); //Could be a lot of rotation -- just make sure they're at least within 0 degrees
  rej_normals->setThreshold (acos (pcl::deg2rad (45.0)));
  reg_.addCorrespondenceRejector (rej_normals);
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
  last_transform_ = Eigen::Matrix4f::Identity ();
}

bool Registration::canAlign ()
{
  return target_.getPointCloud() && source_.getPointCloud();
}

void Registration::align ()
{
  PointCloudWithNormals::Ptr aligned(new PointCloudWithNormals);
  PointCloud::Ptr temp(new PointCloud);
  reg_.setInputSource (source_.getKeypoints());
  reg_.setInputTarget (target_.getKeypoints());
  reg_.align(*aligned, last_transform_);
  converge_ = reg_.hasConverged ();

  if (converge_)
  {
    last_transform_ = reg_.getFinalTransformation();
    pcl::transformPointCloud(*result_, *temp, last_transform_);
    *temp += *(target_.getPointCloud());
    process(temp);
  }
}

void Registration::process (PointCloud::Ptr cloud)
{
  PointCloud::Ptr temp(new PointCloud);
  // Uniform sampling object.
  pcl::UniformSampling<PointT> uniform_filter;
  // uniform_filter.setInputCloud(temp);
  uniform_filter.setInputCloud(cloud);
  // We set the size of every voxel to be 1x1x1 mm
  // (only one point per every cubic millimeter will survive).
  uniform_filter.setRadiusSearch(0.001);
  // We need an additional object to store the indices of surviving points.
  pcl::PointCloud<int> keypointIndices;
  uniform_filter.compute(keypointIndices);
  copyPointCloud(*cloud, keypointIndices.points, *temp);
  std::cout << cloud->points.size() << " " << temp->points.size() << std::endl;
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