#include "CustomCloud.h"

CustomCloud::CustomCloud() : 
  range_limit_z (1.5), 
  leaf_size_1_ (0.01), 
  leaf_size_2_ (0.03),
  normal_k_ (20),
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
  return (f_cloud_);
}  

// Get a pointer to the cloud of 3D surface normals
PointCloudWithNormals::Ptr CustomCloud::getPointCloudWithNormals() const
{
  return (normals_);
}    

// Get a pointer to the cloud of feature descriptors
FeatureCloud::Ptr CustomCloud::getFeatureCloud() const
{
  return (features_);
}

// Compute the surface normals and local features
void CustomCloud::processInput()
{
  filter();
  computePointCloudWithNormals();
  computeFeatureCloud();
}    

// Remove points beyond a certain z limit
void CustomCloud::filter()
{
  PointCloud::Ptr temp(new PointCloud);
  f_cloud_ = PointCloud::Ptr(new PointCloud);        

  pcl::PassThrough<PointT> range_filter;
  range_filter.setFilterFieldName("z");
  range_filter.setFilterLimits(0, range_limit_z);
  range_filter.setInputCloud(cloud_);
  range_filter.filter(*temp);      

  pcl::VoxelGrid<PointT> grid;
  grid.setLeafSize(leaf_size_1_, leaf_size_1_, leaf_size_1_);
  grid.setInputCloud(temp);
  grid.filter(*f_cloud_);  

  // std::cout << temp->points.size() << " " << f_cloud_->points.size() << std::endl;
}  

// Compute the surface normals
void CustomCloud::computePointCloudWithNormals()
{
  normals_ = PointCloudWithNormals::Ptr(new PointCloudWithNormals);
  pcl::search::KdTree<PointT>::Ptr tree(new pcl::search::KdTree<PointT>);      

  pcl::NormalEstimationOMP<PointT, PointNormalT> norm_est;
  norm_est.setInputCloud(f_cloud_);
  norm_est.setSearchMethod(tree);
  norm_est.setKSearch(normal_k_);
  norm_est.compute(*normals_);      

  copyPointCloud(*f_cloud_, *normals_);
}      

// Compute the local feature descriptors
void CustomCloud::computeFeatureCloud()
{
  PointCloud::Ptr temp (new PointCloud);

  pcl::VoxelGrid<PointT> grid;
  grid.setLeafSize(leaf_size_2_, leaf_size_2_, leaf_size_2_);
  // grid.setLeafSize(0.02, 0.02, 0.02);
  grid.setInputCloud(f_cloud_);
  grid.filter(*temp);  

  // std::cout << normals_->points.size() << " " << temp->points.size() << std::endl;

  features_ = FeatureCloud::Ptr(new FeatureCloud);
  pcl::search::KdTree<PointT>::Ptr tree(new pcl::search::KdTree<PointT>);      

  FeatureEstimationT feat_est;
  feat_est.setSearchSurface (f_cloud_);
  feat_est.setInputCloud (temp);
  feat_est.setInputNormals (normals_);
  feat_est.setSearchMethod (tree);
  feat_est.setRadiusSearch (feature_radius_);
  feat_est.compute (*features_);

  f_cloud_.swap(temp);
}