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
  sac_ia_.setMinSampleDistance(msd);
  // Set the maximum distance (squared) threshold between two correspondent points in source and target.
  // If the distance squared is larger, the points will be ignored in the alignment process.
  sac_ia_.setMaxCorrespondenceDistance(0.01*0.01);
  // Set the number of iterations
  sac_ia_.setMaximumIterations(600);

  reg_.setRANSACOutlierRejectionThreshold(rort);
  reg_.setTransformationEpsilon(te);
  // Set the maximum distance between two correspondences (src<->tgt) to 10cm
  // Note: adjust this based on the size of your datasets
  reg_.setMaxCorrespondenceDistance(0.01);  
  // reg_.setEuclideanFitnessEpsilon(mcd);
  // Set the number of iterations
  reg_.setMaximumIterations(mi);
}

void Registration::setInputCloudParamters (float rl, float ls1, float ls2,
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

void Registration::clear ()
{
  converge_ = true;
  result_ = PointCloud::Ptr (new PointCloud);
}

bool Registration::canAlign ()
{
  return target_.getPointCloud() && source_.getPointCloud() 
    && source_.identity() != target_.identity();
}

void Registration::align ()
{
  PointCloud::Ptr temp(new PointCloud);
  PointCloud::Ptr aligned1(new PointCloud);
  PointCloud::Ptr aligned2(new PointCloud);

  sac_ia_.setInputSource(source_.getFilteredPointCloud());
  sac_ia_.setInputTarget(target_.getFilteredPointCloud());
  sac_ia_.setSourceFeatures(source_.getFeatureCloud());
  sac_ia_.setTargetFeatures(target_.getFeatureCloud());

  sac_ia_.align(*aligned1);
  Eigen::Matrix4f rough_transform = sac_ia_.getFinalTransformation();

  std::cout << "e" << std::endl;

  reg_.setInputSource (source_.getFilteredPointCloud ());
  reg_.setInputTarget (target_.getFilteredPointCloud ());

  reg_.align(*aligned2, rough_transform);
  converge_ = reg_.hasConverged ();

  if (converge_)
  {
    pcl::transformPointCloud(*result_, *temp, reg_.getFinalTransformation());
    *temp += *(target_.getPointCloud());
    result_.swap(temp);
  }

  std::cout << "f" << std::endl;
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