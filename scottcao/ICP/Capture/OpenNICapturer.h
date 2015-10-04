#ifndef OPENNICAPTURER_H
#define OPENNICAPTURER_H

#include "PCDWriter.h"

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

    void run ();
    

  private:

    void cloudCallback (const PointCloud::ConstPtr& cloud);

    // For detecting when SPACE is pressed.
    void keyboardCallback (const pcl::visualization::KeyboardEvent& event,
      void* nothing);

    boost::shared_ptr<pcl::visualization::PCLVisualizer> cloud_viewer_;

    boost::mutex cloud_mutex_;

    bool save_clouds_;

    PointCloud::ConstPtr cloud_;

    PCDWriter writer;

};

#endif
