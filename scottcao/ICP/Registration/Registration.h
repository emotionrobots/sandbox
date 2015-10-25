#ifndef REGISTRATION_H
#define REGISTRATION_H

#include "CustomCloud.h"

#include <pcl/common/angles.h>
#include <pcl/registration/correspondence_estimation.h>
#include <pcl/registration/correspondence_estimation_backprojection.h>
#include <pcl/registration/correspondence_estimation_normal_shooting.h>
#include <pcl/registration/correspondence_rejection_median_distance.h>
#include <pcl/registration/correspondence_rejection_surface_normal.h>
// #include <pcl/registration/transformation_estimation_point_to_plane_lls.h>
#include <pcl/registration/transformation_estimation_point_to_plane_weighted.h>
#include <pcl/registration/default_convergence_criteria.h>
#include <pcl/registration/ia_ransac.h>
#include <pcl/registration/icp.h>

class Registration
{
  public:

    Registration ();

    // Registration (int cr, float in_f, int ns, 
    //     float st, float mcd, int mi);

    void setRegistrationParameters(float msd, float mcd, 
        int mi, float rort, float te);

    void setInputCloudParameters (float rl, float ls1, float ls2, 
        float k, float fr, u_int i);

    void setInputCloud (PointCloud::ConstPtr cloud);

    void loadInputCloud (std::string file_name);

    void clear ();

    bool canAlign ();

    void findCorrespondences (PointCloudWithNormals::Ptr src, 
                              PointCloudWithNormals::Ptr tgt, 
                              pcl::Correspondences &all_correspondences);

    void rejectBadCorrespondences (pcl::CorrespondencesPtr &all_correspondences,
                               PointCloudWithNormals::Ptr rc,
                               PointCloudWithNormals::Ptr tgt,
                               pcl::Correspondences &remaining_correspondences);

    void align ();

    bool hasConverged () const;

    double getFitnessScore () const;

    PointCloud::Ptr getResultantCloud () const;

  private:

    // pcl::SampleConsensusPrerejective<PointT, PointT, FeatureT> reg_;
    // pcl::SampleConsensusInitialAlignment<PointNormalT, PointNormalT, FeatureT> sac_ia_;

    // pcl::IterativeClosestPoint<PointNormalT, PointNormalT> reg_;
    
    // Point cloud data
    CustomCloud source_;
    CustomCloud target_;

    PointCloud::Ptr result_;

    bool converge_;

    double fitness_score_;
};

#endif