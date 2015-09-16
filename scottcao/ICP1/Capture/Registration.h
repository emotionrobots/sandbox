#ifndef REGISTRATION_H
#define REGISTRATION_H

#include "CustomCloud.h"

#include <pcl/registration/sample_consensus_prerejective.h>

typedef pcl::PointXYZRGBA PointT;
typedef pcl::PointCloud<PointT> PointCloud;
typedef pcl::PointXYZRGBNormal PointNormalT;
// typedef pcl::PointCloud<PointNormalT> PointCloudWithNormals;
typedef pcl::SHOT352 FeatureT;
// typedef pcl::PointCloud<FeatureT> FeatureCloud;
// typedef pcl::SHOTEstimationOMP<PointNormalT, PointNormalT, FeatureT> FeatureEstimationT;

class Registration
{
  public:

    Registration ();

    Registration (int cr, float in_f, int ns, 
        float st, float mcd, int mi);

    void setRegistrationParameters (int cr, float in_f, int ns, 
        float st, float mcd, int mi);

    void setInputCloudParamters (float rl, float ls1, float ls2, 
        float k, float fr, u_int i);

    void setInputCloud (PointCloud::ConstPtr cloud);

    void setInputSource (PointCloud::Ptr s);

    void setInputTarget (PointCloud::Ptr t);

    void clear ();

    bool canAlign ();

    void align ();

    bool hasConverged () const;

    double getFitnessScore () const;

    PointCloud::Ptr getResultantCloud () const;

  private:

    pcl::SampleConsensusPrerejective<PointT, PointT, FeatureT> reg_;

    // Point cloud data
    CustomCloud source_;
    CustomCloud target_;

    PointCloud::Ptr result_;

    bool converge_;

    double fitness_score_;
};

#endif