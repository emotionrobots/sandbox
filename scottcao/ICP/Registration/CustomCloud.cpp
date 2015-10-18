#include "CustomCloud.h"

CustomCloud::CustomCloud() : 
  range_limit_z (1.5), 
  leaf_size_1_ (0.01), 
  leaf_size_2_ (0.03),
  normal_k_ (10),
  // normal_radius_(0.03), 
  feature_radius_ (0.1),
  identity_ (0)
{}  

CustomCloud::CustomCloud(float rl, float ls1, float ls2, float k, float fr, u_int i) 
{
  setParameters(rl, ls1, ls2, k, fr, i);
}  

void CustomCloud::setParameters (float rl, float ls1, float ls2, float k, 
  float fr, u_int i)
{
  range_limit_z = rl; 
  leaf_size_1_ = ls1;
  leaf_size_2_ = ls2;
  normal_k_ = k;
  // normal_radius_ = k;
  feature_radius_ = fr;
  identity_ = i;
}

// Process the given cloud
void CustomCloud::setInputCloud(PointCloud::ConstPtr xyz)
{
  cloud_ = PointCloud::Ptr(new PointCloud);
  copyPointCloud(*xyz, *cloud_);
  ++ identity_; 
  processInput();
}  

// Load and process the cloud in the given PCD file
void CustomCloud::loadInputCloud(const std::string& pcd_file)
{
  cloud_ = PointCloud::Ptr(new PointCloud);
  pcl::io::loadPCDFile(pcd_file, *cloud_);
  processInput();
}

u_int CustomCloud::identity()
{
  return (identity_);
}  

// Get a pointer to the cloud 3D points
PointCloud::ConstPtr CustomCloud::getPointCloud() const
{
  return (cloud_);
}  

// Get a pointer to the filtered cloud 3D points
PointCloud::Ptr CustomCloud::getFilteredPointCloud() const
{
  return (filtered_cloud_);
}  

// Get a pointer to the cloud of 3D surface normals
PointCloudWithNormals::Ptr CustomCloud::getNormalCloud() const
{
  return (normals_);
}    

PointCloudWithNormals::Ptr CustomCloud::getKeypoints() const
{
  return (keypoints_);
}

// // Get a pointer to the cloud of feature descriptors
// FeatureCloud::Ptr CustomCloud::getFeatureCloud() const
// {
//   return (features_);
// }

// Compute the surface normals and local features
void CustomCloud::processInput()
{
  std::cout << "a" << std::endl;
  filter();
  std::cout << "b" << std::endl;
  computeNormalCloud();
  std::cout << "c" << std::endl;
  computeKeypoints();
  std::cout << "d" << std::endl;
  // computeFeatureCloud();
  // std::cout << "e" << std::endl;
}    

// Remove points beyond a certain z limit
void CustomCloud::filter()
{
  filtered_cloud_ = PointCloud::Ptr(new PointCloud); 
  pcl::FastBilateralFilter<PointT> bilateral_filter;

  bilateral_filter.setInputCloud(cloud_);
  bilateral_filter.setSigmaS(5);
  bilateral_filter.setSigmaR(0.005);

  bilateral_filter.filter(*filtered_cloud_);

  // std::cout << "1: " << filtered_cloud_->points.size()/(640.0*480.0) << std::endl;
}  

// Compute the surface normals
void CustomCloud::computeNormalCloud()
{
  normals_ = PointCloudWithNormals::Ptr(new PointCloudWithNormals);

  // Object for normal estimation.
  pcl::IntegralImageNormalEstimation<PointT, PointNormalT> normalEstimation;
  normalEstimation.setInputCloud(filtered_cloud_);
  // Other estimation methods: COVARIANCE_MATRIX, AVERAGE_DEPTH_CHANGE, SIMPLE_3D_GRADIENT.
  // They determine the smoothness of the result, and the running time.
  // normalEstimation.setNormalEstimationMethod(normalEstimation.AVERAGE_3D_GRADIENT);
  normalEstimation.setNormalEstimationMethod(normalEstimation.SIMPLE_3D_GRADIENT); 
  // Depth threshold for computing object borders based on depth changes, in meters.
  normalEstimation.setMaxDepthChangeFactor(0.02f);
  // Factor that influences the size of the area used to smooth the normals.
  normalEstimation.setNormalSmoothingSize(10);
 
  // Calculate the normals.
  normalEstimation.compute(*normals_);
  copyPointCloud(*filtered_cloud_, *normals_);

  std::vector<int> mapping;
  pcl::removeNaNNormalsFromPointCloud(*normals_, *normals_, mapping);
  pcl::removeNaNFromPointCloud(*normals_, *normals_, mapping);
}      

void CustomCloud::computeKeypoints()
{
  keypoints_ = PointCloudWithNormals::Ptr(new PointCloudWithNormals);
  // Uniform sampling object.
  pcl::UniformSampling<PointNormalT> uniform_filter;
  // uniform_filter.setInputCloud(temp);
  uniform_filter.setInputCloud(normals_);
  // We set the size of every voxel to be 1x1x1cm
  // (only one point per every cubic centimeter will survive).
  uniform_filter.setRadiusSearch(0.02f);
  // We need an additional object to store the indices of surviving points.
  pcl::PointCloud<int> keypointIndices;
  uniform_filter.compute(keypointIndices);
  // copyPointCloud(*temp, keypointIndices.points, *filtered_cloud_);
  copyPointCloud(*normals_, keypointIndices.points, *keypoints_);
}

// // Compute the local feature descriptors
// void CustomCloud::computeFeatureCloud()
// {
//   // std::cout << "2: " << temp->points.size()/(640.0*480.0) << std::endl;

//   features_ = FeatureCloud::Ptr(new FeatureCloud);
//   pcl::search::KdTree<PointNormalT>::Ptr tree(new pcl::search::KdTree<PointNormalT>);      

//   FeatureEstimationT feat_est;
//   feat_est.setNumberOfThreads(4);
//   feat_est.setSearchSurface (normals_);
//   feat_est.setInputCloud (keypoints_);
//   feat_est.setInputNormals (normals_);
//   feat_est.setSearchMethod (tree);
//   feat_est.setRadiusSearch (feature_radius_);
//   feat_est.compute (*features_);
// }