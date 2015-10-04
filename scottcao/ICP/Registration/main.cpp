
#include "Registration.h"
#include "PCDWriter.h"

#include <pcl/visualization/pcl_visualizer.h>
// #include <pcl/io/pcd_io.h>

////////////////////////////////////////////////////////////////////////////////
/** \brief Load a set of PCD files that we want to register together
  * \param argc the number of arguments (pass from main ())
  * \param argv the actual command line arguments (pass from main ())
  * \param models the resultant vector of point cloud datasets
  */
void loadData (int argc, char **argv, std::vector<std::string>& models)
{
  std::string extension (".pcd");
  // Suppose the first argument is the actual test model
  for (int i = 1; i < argc; i++)
  {
    std::string fname = std::string(argv[i]);
    // Needs to be at least 5: .plot
    if (fname.size() <= extension.size())
    {
      continue;
    }

    //check that the argument is a pcd file
    if (fname.compare(fname.size() - extension.size(), extension.size(), extension) == 0)
    {
      // Load the cloud and saves it into the global list of models
      models.push_back(fname);
    }

    std::cout << "loadData: " << i << std::endl;
  }
}

int main (int argc, char ** argv)
{
    // Load data
  std::vector<std::string> data;
  loadData(argc, argv, data);

  // Check user input
  if (data.empty())
  {
    PCL_ERROR("Syntax is: %s <source.pcd> <target.pcd> [*]\n", argv[0]);
    PCL_ERROR("[*] - multiple files can be added. The registration results of (i, i+1) will be registered against (i+2), etc\n");
    return -1;
  }
  PCL_INFO ("Loaded %d datasets.\n", (int)data.size ());

  boost::shared_ptr<pcl::visualization::PCLVisualizer> cloud_viewer
  (new pcl::visualization::PCLVisualizer ("Registration"));

  Registration reg;
  reg.setInputCloudParameters (1.5, 0.004, 0.02, 12, 0.12, 0);
  reg.setRegistrationParameters (0.02, 0.01, 1000, 0.05, 0.001*0.001);

  cloud_viewer->addCoordinateSystem (1.0);
  cloud_viewer->setBackgroundColor (0, 0, 0);
  cloud_viewer->setCameraClipDistances (0.0, 1.5);
  cloud_viewer->initCameraParameters ();

  PCDWriter writer;
  writer.setFileName("Results");
  
  for (size_t i = 0; i < data.size(); i++)
  {
    reg.loadInputCloud(data[i]);

    if (reg.canAlign())
    {
      reg.align();
      if (reg.hasConverged ())
      {
        PCL_INFO("Registration converged. \n");
        PointCloud::Ptr result = reg.getResultantCloud();
        if (!cloud_viewer->updatePointCloud (result, "result"))
        {
          cloud_viewer->addPointCloud (result, "result");
        }
        cloud_viewer->spinOnce();
        writer.writeToFile(result);
      }
      else
      {
        PCL_ERROR("Registration did not converge. Frame dropped. \n");
      }
    }
  }
  
  return 0;
}