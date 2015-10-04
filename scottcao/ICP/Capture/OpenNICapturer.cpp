#include "OpenNICapturer.h"


void OpenNICapturer::cloudCallback (const PointCloud::ConstPtr& cloud)
{
  boost::mutex::scoped_lock lock (cloud_mutex_);
  cloud_ = cloud;
  if (cloud && !cloud_viewer_->updatePointCloud (cloud, "cloud"))
  {
    cloud_viewer_->addPointCloud (cloud, "cloud");
  }
  cloud_viewer_->spinOnce ();
}

// For detecting when SPACE is pressed.
void OpenNICapturer::keyboardCallback (const pcl::visualization::KeyboardEvent& event,
  void* nothing)
{
  if (event.getKeySym () == "space" && event.keyDown ())
  {
    save_clouds_ = !save_clouds_;
  }
  // else if (event.getKeySym () == "esc" && event.keyDown ())
  // {
  //   cloud_viewer_->close ();
  // }
}

OpenNICapturer::OpenNICapturer () : 
  save_clouds_ (false), 
  cloud_viewer_ (new pcl::visualization::PCLVisualizer ("Visualizer"))
{
  cloud_viewer_->addCoordinateSystem (1.0);
  cloud_viewer_->setBackgroundColor (0, 0, 0);
  cloud_viewer_->setCameraClipDistances (0.0, 1.5);
  cloud_viewer_->initCameraParameters ();
  // cloud_viewer_->setCameraPosition (0, 0, -3, 1, 0, 0); 
  cloud_viewer_->registerKeyboardCallback(&OpenNICapturer::keyboardCallback, *this);
}

PointCloud::ConstPtr OpenNICapturer::getCloud()
{
  return cloud_;
}

void OpenNICapturer::setFileName (std::string fn)
{
  writer.setFileName(fn);
}


void OpenNICapturer::run ()
{
  pcl::OpenNIGrabber grabber;

  boost::function<void (const PointCloud::ConstPtr&)> cloud_cb 
      = boost::bind (&OpenNICapturer::cloudCallback, this, _1);
  boost::signals2::connection cloud_connection 
      = grabber.registerCallback (cloud_cb);

  grabber.start ();

  int iterations = 0;

  while (!cloud_viewer_->wasStopped ())
  {
    PointCloud::ConstPtr cloud;

    // See if we can get a cloud
    if (cloud_mutex_.try_lock ())
    {
      cloud_.swap (cloud);
      cloud_mutex_.unlock ();
    }

    if (cloud && save_clouds_ && iterations % 5 == 0)
    {
      writer.writeToFile (cloud);
    }

    if (!grabber.isRunning ())
    {
      cloud_viewer_->spin ();
    }

    boost::this_thread::sleep (boost::posix_time::microseconds (100));

    iterations ++;
  }

  grabber.stop ();

  cloud_connection.disconnect ();

  writer.saveText ();
}