#ifndef CUSTOMCLOUD_H
#define CUSTOMCLOUD_H

#include <pcl/features/integral_image_normal.h>
// #include <pcl/features/normal_3d_omp.h>
// #include <pcl/features/shot_omp.h>
#include <pcl/filters/fast_bilateral.h>
#include <pcl/filters/filter.h>
#include <pcl/filters/passthrough.h>
#include <pcl/io/pcd_io.h>
#include <pcl/keypoints/uniform_sampling.h>

typedef pcl::PointXYZRGBA PointT;
typedef pcl::PointCloud<PointT> PointCloud;
typedef pcl::PointXYZRGBNormal PointNormalT;
typedef pcl::PointCloud<PointNormalT> PointCloudWithNormals;
// typedef pcl::SHOT352 FeatureT;
// typedef pcl::SHOT1344 FeatureT;
// typedef pcl::PointCloud<FeatureT> FeatureCloud;
// typedef pcl::SHOTEstimationOMP<PointNormalT, PointNormalT, FeatureT> FeatureEstimationT;
// typedef pcl::SHOTColorEstimationOMP<PointT, PointNormalT, FeatureT> FeatureEstimationT;

class CustomCloud
{
  public:

    CustomCloud ();

    CustomCloud (float rl, float ls1, float ls2, float k, float fr, u_int i);

    void setParameters (float rl, float ls1, float ls2, float k, 
        float fr, u_int i);

    void setInputCloud (PointCloud::ConstPtr xyz);

    void loadInputCloud (const std::string& pcd_file);
    
    u_int identity ();

    PointCloud::ConstPtr getPointCloud () const;

    PointCloud::Ptr getFilteredPointCloud () const;

    PointCloudWithNormals::Ptr getNormalCloud () const;

    PointCloudWithNormals::Ptr getKeypoints() const;

    // FeatureCloud::Ptr getFeatureCloud () const;

  private:

    void processInput ();

    void filter ();

    void computeNormalCloud ();

    void computeKeypoints();

    // void computeFeatureCloud ();

    // Point cloud data
    PointCloud::Ptr cloud_;
    PointCloud::Ptr filtered_cloud_;
    PointCloudWithNormals::Ptr keypoints_;

    PointCloudWithNormals::Ptr normals_;

    // FeatureCloud::Ptr features_;  

    // Parameters
    float range_limit_z;  
    float leaf_size_1_;
    float leaf_size_2_;
    float normal_k_;
    // float normal_radius_;
    float feature_radius_;

    u_int identity_;
};

#endif
