#ifndef OPENNICAPTURER_H
#define OPENNICAPTURER_H

#include "CustomCloud.h"
#include "PCDWriter.h"
#include "Registration.h"

#include <pcl/io/openni_grabber.h>
#include <pcl/visualization/pcl_visualizer.h>

typedef pcl::PointXYZRGBA PointT;
typedef pcl::PointCloud<PointT> PointCloud;

class OpenNICapturer
{
  public:

    OpenNICapturer ();

    PointCloud::ConstPtr getCloud();

    void setFileName(std::string fn);

    void setRegistrationParameters(int cr, float in_f, int ns, 
        float st, float mcd, int mi);

    void setInputCloudParameters(float rl, float ls1, float ls2, 
        float k, float fr, u_int i);

    void run ();
    

  private:

    void cloudCallback (const PointCloud::ConstPtr& cloud);

    // For detecting when SPACE is pressed.
    void keyboardCallback (const pcl::visualization::KeyboardEvent& event,
      void* nothing);

    void showCloudLeft(PointCloud::ConstPtr cloud);

    void showCloudRight(PointCloud::Ptr cloud);

    boost::shared_ptr<pcl::visualization::PCLVisualizer> cloud_viewer_;

    int vp1;
    int vp2;

    boost::mutex cloud_mutex_;
    boost::mutex viewer_mutex_;

    bool save_clouds_;

    PointCloud::ConstPtr cloud_;

    Registration reg_;

    PCDWriter writer;

};

#endif