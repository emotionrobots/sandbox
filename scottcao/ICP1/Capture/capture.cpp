

#include <iostream>
#include <fstream>
  
#include <pcl/features/normal_3d_omp.h>
#include <pcl/features/shot_omp.h>

#include <pcl/filters/filter.h>
#include <pcl/filters/passthrough.h>
#include <pcl/filters/radius_outlier_removal.h>
#include <pcl/filters/voxel_grid.h>

#include <pcl/io/openni_grabber.h>
#include <pcl/io/pcd_io.h>

#include <pcl/registration/sample_consensus_prerejective.h>

#include <pcl/visualization/pcl_visualizer.h>


// Convenient typedefs
 typedef pcl::PointXYZRGBA PointT;
 typedef pcl::PointCloud<PointT> PointCloud;
 typedef pcl::PointXYZRGBNormal PointNormalT;
 typedef pcl::PointCloud<PointNormalT> PointCloudWithNormals;
 typedef pcl::SHOT352 FeatureT;
 typedef pcl::PointCloud<FeatureT> FeatureCloud;
 typedef pcl::SHOTEstimationOMP<PointNormalT, PointNormalT, FeatureT> FeatureEstimationT;

 // Define a new point representation for < x, y, z, curvature >
class PointNormalRepresentation : public pcl::PointRepresentation<PointNormalT>
{
  using pcl::PointRepresentation<PointNormalT>::nr_dimensions_;
  public:
    PointNormalRepresentation()
    {
      // Define the number of dimensions
      nr_dimensions_ = 4;
    }

    // Override the copyToFloatArray method to define our feature vector
    virtual void copyToFloatArray(const PointNormalT &p, float * out) const
    {
      // < x, y, z, curvature >
      out[0] = p.x;
      out[1] = p.y;
      out[2] = p.z;
      out[3] = p.curvature;
    }
  };


 class PCDCloud
 {
 public:
  PCDCloud() :
    normal_k_(20),
    feature_radius_(0.9f),
    leaf_size_(0.03f), 
    identity_(0)
  {}

  ~PCDCloud() {}

    // Process the given cloud
  void setInputCloud(PointCloud::Ptr xyz, u_int& identity)
  {
    cloud_ = PointCloud::Ptr(new PointCloud);
    copyPointCloud(*xyz, *cloud_);
    identity_ = identity;
    processInput();
  }

    // Load and process the cloud in the given PCD file
  void loadInputCloud(const std::string &pcd_file)
  {
    cloud_ = PointCloud::Ptr(new PointCloud);
    pcl::io::loadPCDFile(pcd_file, *cloud_);
    processInput();
  }

  u_int identity()
  {
    return identity_;
  }

    // Get a pointer to the cloud 3D points
  PointCloud::Ptr getPointCloud() const
  {
    return (cloud_);
  }

    // Get a pointer to the cloud of 3D surface normals
  PointCloudWithNormals::Ptr getPointCloudWithNormals() const
  {
    return (normals_);
  }

    // Get a pointer to the cloud of feature descriptors
  FeatureCloud::Ptr getFeatureCloud() const
  {
    return (features_);
  }

protected:
  // Compute the surface normals and local features
  void processInput()
  {
    computePointCloudWithNormals();
    computeFeatureCloud();
  }

    // Compute the surface normals
  void computePointCloudWithNormals()
  {
    std::vector<int> indices;
    pcl::removeNaNFromPointCloud(*cloud_,*cloud_, indices);

    pcl::PassThrough<PointT> range_filter;
    range_filter.setFilterFieldName("z");
    range_filter.setFilterLimits(0.0, 1.0);
    range_filter.setInputCloud(cloud_);
    range_filter.filter(*cloud_);

    PointCloud::Ptr f_cloud = PointCloud::Ptr(new PointCloud);

    // Filter object.
    pcl::RadiusOutlierRemoval<PointT> filter;
    filter.setInputCloud(cloud_);
    // Every point must have 125 neighbors within 10 cm, or it will be removed.
    filter.setRadiusSearch(0.10);
    filter.setMinNeighborsInRadius(125);
    filter.filter(*f_cloud);

    cout << cloud_->points.size() << " " << f_cloud->points.size() << endl;

    pcl::VoxelGrid<PointT> grid;
    grid.setLeafSize(leaf_size_, leaf_size_, leaf_size_);
    grid.setInputCloud(f_cloud);
    grid.filter(*f_cloud);

    normals_ = PointCloudWithNormals::Ptr(new PointCloudWithNormals);
    pcl::search::KdTree<PointT>::Ptr tree(new pcl::search::KdTree<PointT>);

    pcl::NormalEstimationOMP<PointT, PointNormalT> norm_est;
    norm_est.setInputCloud(f_cloud);
    norm_est.setSearchMethod(tree);
    norm_est.setKSearch(normal_k_);
    norm_est.compute(*normals_);

    copyPointCloud(*f_cloud, *normals_);
  }

    // Compute the local feature descriptors
  void computeFeatureCloud()
  {
    features_ = FeatureCloud::Ptr(new FeatureCloud);
    pcl::search::KdTree<PointNormalT>::Ptr tree(new pcl::search::KdTree<PointNormalT>);

    FeatureEstimationT feat_est;
    feat_est.setInputCloud(normals_);
    feat_est.setInputNormals(normals_);
    feat_est.setSearchMethod(tree);
    feat_est.setRadiusSearch(feature_radius_);
    feat_est.compute(*features_);
  }

private:
    // Point cloud data
  PointCloud::Ptr cloud_;
  PointCloudWithNormals::Ptr normals_;
  FeatureCloud::Ptr features_;

    // Parameters
  float leaf_size_;
  float normal_k_;
  float feature_radius_;

  u_int identity_;
};


// A cloud that will store color info. 
PointCloud::Ptr inputCloud(new PointCloud);                                              
// Point cloud viewer object.
boost::shared_ptr<pcl::visualization::PCLVisualizer> viewer
 (new pcl::visualization::PCLVisualizer("Registration Capture")); 
// Program controls.  
bool saveClouds(false), sourceDefined(false), inputLocked(true);

int vp1, vp2;

u_int frame_count = 0;


void showCloudLeft(const PointCloud::ConstPtr& cloud_left)
{
  viewer->removePointCloud("left");
  viewer->addPointCloud(cloud_left, "left", vp1);
  viewer->spinOnce();
}

void showCloudRight(PointCloud::Ptr cloud_right)
{
  viewer->removePointCloud("right");
  viewer->addPointCloud(cloud_right, "right", vp2);
}

////////////////////////////////////////////////////////////////////////////////
// Align a pair of PointCloud datasets and return the convergence result
bool pairAligned(PCDCloud& src, PCDCloud& tgt, PointCloud::Ptr result)
{
  // cout << src.getPointCloud()->points.size() << " " 
       // << tgt.getPointCloud()->points.size() << endl;

  try 
  {
    PointCloudWithNormals::Ptr temp(new PointCloudWithNormals);

    pcl::SampleConsensusPrerejective<PointNormalT, PointNormalT, FeatureT> reg;

    // Instead of matching a descriptor with its nearest neighbor, choose randomly between
    // the N closest ones, making it more robust to outliers, but increasing time.
    reg.setCorrespondenceRandomness(5);
    // Set the fraction (0-1) of inlier points required for accepting a transformation.
    // At least this number of points will need to be aligned to accept a pose.
    reg.setInlierFraction(0.25f);
    // Set the number of samples to use during each iteration (minimum for 6 DoF is 3).
    reg.setNumberOfSamples(3);
    // Set the similarity threshold (0-1) between edge lengths of the polygons. The
    // closer to 1, the more strict the rejector will be, probably discarding acceptable poses.
    reg.setSimilarityThreshold(0.8f);
    // Set the maximum distance threshold between two correspondent points in source and target.
    // If the distance is larger, the points will be ignored in the alignment process.
    reg.setMaxCorrespondenceDistance(0.0075f);
    reg.setMaximumIterations(100);

    // Instantiate our custom point representation (defined above) ...
    PointNormalRepresentation point_representation;
    // Weight the 'curvature' dimension so that it is balanced against x, y, and z
    float alpha[4] = {1.0, 1.0, 1.0, 1.0};
    point_representation.setRescaleValues(alpha);
    reg.setPointRepresentation
     (boost::make_shared<const PointNormalRepresentation>(point_representation));

    reg.setInputSource(src.getPointCloudWithNormals());
    reg.setInputTarget(tgt.getPointCloudWithNormals());
    reg.setSourceFeatures(src.getFeatureCloud());
    reg.setTargetFeatures(tgt.getFeatureCloud());

    reg.align(*temp);

    bool converge = reg.hasConverged();
    if (converge)
    {
      pcl::transformPointCloud(*result, *result, reg.getFinalTransformation());
      *result += *(tgt.getPointCloud());
    }
    return converge;
  } 
  catch(int e)
  {
    PCL_ERROR("Exception occured. Frame dropped. ");
    return false;
  }

  return false;
}               

// This function is called every time the device has new data.
void grabberCallback(const PointCloud::ConstPtr& cloud)
{
  if (!viewer->wasStopped()) 
  {
    showCloudLeft(cloud);
  }

  if (saveClouds)
  {
    inputLocked = true;
    
    (*inputCloud).clear();
    copyPointCloud(*cloud, *inputCloud);

    inputLocked = false;

    ++frame_count;
    cout << "Frame count: " << frame_count << endl;
  }
}

// For detecting when SPACE is pressed.
void keyboardEventOccurred(const pcl::visualization::KeyboardEvent& event,
  void* nothing)
{
  if (event.getKeySym() == "space" && event.keyDown())
  {
    saveClouds = true;
  }
}

bool writeToFile(std::string inputName, u_int filesSaved, 
  PointCloud::Ptr targetCloud, ofstream* output_text)
{
  std::stringstream stream;
  stream << "PCD/" << inputName << "-" << filesSaved << ".pcd";
  std::string filename = stream.str();

  if (pcl::io::savePCDFile(filename, *targetCloud, true) == 0)
  {
    std::cout << "Saved " << filename << "." << endl;
    (*output_text) << filename << " ";
    return true;
  }

  PCL_ERROR("Problem saving %s.\n", filename.c_str());
  return false;
}

int main(int argc, char** argv)
{
  // Name of saved PCD files
  std::string inputName = "ICP";    

  if (argc == 2)
  {
    inputName = argv[1];
  }
  else if (argc > 2)
  {
    return -1;
  }

  // Used to write output to file
  ofstream output_text;
  output_text.open((inputName+".txt").c_str());

  // OpenNI grabber that takes data from the device.
  pcl::Grabber* openniGrabber  = new pcl::OpenNIGrabber();
  if (openniGrabber == 0)
  {
    return -1;
  }
  boost::function<void (const PointCloud::ConstPtr&)> f =
    boost::bind(&grabberCallback, _1);
  openniGrabber->registerCallback(f);

  viewer->registerKeyboardCallback(keyboardEventOccurred);
  viewer->createViewPort(0.0, 0.0, 0.5, 1.0, vp1);
  viewer->createViewPort(0.5, 0.0, 1.0, 1.0, vp2);

  // For the numbering of the clouds saved to disk.                                          
  u_int filesSaved = 1;
  PCDCloud source, target; 
  bool sourceDefined(false);
  PointCloud::Ptr result(new PointCloud);

  // Start fetching and displaying frames from the device.
  openniGrabber->start();

  // Main loop.
  while (!viewer->wasStopped())
  { 
    if (saveClouds && frame_count > 0)
    {
      target.setInputCloud(inputCloud, frame_count);
      bool converge(true);
      if (sourceDefined && target.identity() != source.identity())
      {
        converge = pairAligned(source, target, result);
      }
      else if (!sourceDefined)
      {
        *result += *(target.getPointCloud());
        sourceDefined = true;
      }

      if (converge)
      {
        source = PCDCloud(target);
        if (writeToFile(inputName, filesSaved, target.getPointCloud(), &output_text))
        {
          ++filesSaved;
        }
        showCloudRight(result);
      }
    }
  }

  output_text.close();

  openniGrabber->stop();
}
