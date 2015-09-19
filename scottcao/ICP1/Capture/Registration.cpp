#include "Registration.h"

Registration::Registration() 
{
  clear();
}

Registration::Registration (int cr, float in_f, int ns, 
        float st, float mcd, int mi)
{
  clear ();
  setRegistrationParameters(cr, in_f, ns, st, mcd, mi);
}

void Registration::setRegistrationParameters (int cr, float in_f, int ns, 
        float st, float mcd, int mi)
{
  // Instead of matching a descriptor with its nearest neighbor, choose randomly between
  // the N closest ones, making it more robust to outliers, but increasing time.
  reg_.setCorrespondenceRandomness (cr);

  // Set the fraction (0-1) of inlier points required for accepting a transformation.
  // At least this number of points will need to be aligned to accept a pose.
  reg_.setInlierFraction (in_f);

  // Set the number of samples to use during each iteration (minimum for 6 DoF is 3).
  reg_.setNumberOfSamples (ns);

  // Set the similarity threshold (0-1) between edge lengths of the polygons. The
  // closer to 1, the more strict the rejector will be, probably discarding acceptable poses.
  reg_.setSimilarityThreshold (st);

  // Set the maximum distance threshold between two correspondent points in source and target.
  // If the distance is larger, the points will be ignored in the alignment process.
  reg_.setMaxCorrespondenceDistance (mcd);

  // Set the number of RANSAC iterations
  reg_.setMaximumIterations (mi);
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
  try 
  {
    PointCloud::Ptr temp(new PointCloud);
    PointCloud::Ptr aligned(new PointCloud);

    // // Instantiate our custom point representation (defined above) ...
    // PointNormalRepresentation point_representation;
    // // Weight the 'curvature' dimension so that it is balanced against x, y, and z
    // float alpha[4] = {1.0, 1.0, 1.0, 1.0};
    // point_representation.setRescaleValues (alpha);
    // reg_.setPointRepresentation
    //  (boost::make_shared<const PointNormalRepresentation>(point_representation));

    reg_.setInputSource (source_.getFilteredPointCloud ());
    reg_.setInputTarget (target_.getFilteredPointCloud ());

    reg_.setSourceFeatures (source_.getFeatureCloud ());
    reg_.setTargetFeatures (target_.getFeatureCloud ());

    reg_.align (*aligned);

    converge_ = reg_.hasConverged ();
    // fitness_score_ = reg_.getFitnessScore ();

    if (converge_)
    {
      pcl::transformPointCloud(*result_, *temp, reg_.getFinalTransformation());
      *temp += *(target_.getPointCloud());
      result_.swap(temp);

      // pcl::VoxelGrid<PointT> grid;
      // float leaf_size_(0.03);

      // grid.setLeafSize(leaf_size_, leaf_size_, leaf_size_);
      // grid.setInputCloud(result_);
      // grid.filter(*result_);
    }
  } 
  catch (int e)
  {
    PCL_ERROR ("Exception occured. Frame dropped. ");
  }
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