#ifndef PCDWRITER_H
#define PCDWRITER_H

// #include "CustomCloud.h"

#include <iostream>
#include <fstream>

#include <pcl/io/pcd_io.h>

typedef pcl::PointXYZRGBA PointT;
typedef pcl::PointCloud<PointT> PointCloud;

class PCDWriter
{
  public:

    PCDWriter ();

    PCDWriter (std::string fn);

    void setFileName(std::string fn);

    void writeToFile (PointCloud::Ptr cloud);

    void writeToFile (PointCloud::ConstPtr cloud);

    void saveText ();

  private:

    void saveFileName (std::string file);

    std::string file_name_;

    u_int file_count_;

    std::stringstream stream;
};

#endif