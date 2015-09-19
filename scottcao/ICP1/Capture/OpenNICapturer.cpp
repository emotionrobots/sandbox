#include "OpenNICapturer.h"


void OpenNICapturer::cloudCallback (const PointCloud::ConstPtr& cloud)
{
  boost::mutex::scoped_lock lock (cloud_mutex_);
  // cloud_ = PointCloud::Ptr(cloud);
  cloud_ = cloud;
  if (viewer_mutex_.try_lock ())
  {
    showCloudLeft (cloud_);
    viewer_mutex_.unlock ();
  }
}

    // For detecting when SPACE is pressed.
void OpenNICapturer::keyboardCallback (const pcl::visualization::KeyboardEvent& event,
  void* nothing)
{
  if (event.getKeySym () == "space" && event.keyDown ())
  {
    save_clouds_ = !save_clouds_;
  }
  else if (event.getKeySym () == "esc" && event.keyDown ())
  {
    cloud_viewer_->close ();
  }
}

void OpenNICapturer::showCloudLeft (PointCloud::ConstPtr cloud)
{
  if (cloud && !cloud_viewer_->updatePointCloud (cloud, "left"))
  {
    cloud_viewer_->addPointCloud (cloud, "left", vp1);
  }
  cloud_viewer_->spinOnce ();
}

void OpenNICapturer::showCloudRight (PointCloud::Ptr cloud)
{
  if (cloud && !cloud_viewer_->updatePointCloud (cloud, "right"))
  {
    cloud_viewer_->addPointCloud (cloud, "right", vp2);
  }
  cloud_viewer_->spinOnce ();
}

OpenNICapturer::OpenNICapturer () : 
  save_clouds_ (false), 
  cloud_viewer_ (new pcl::visualization::PCLVisualizer ("Visualizer"))
{
  cloud_viewer_->createViewPort (0.0, 0.0, 0.5, 1.0, vp1);
  cloud_viewer_->createViewPort (0.5, 0.0, 1.0, 1.0, vp2);
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

void OpenNICapturer::setRegistrationParameters(int cr, float in_f, int ns, 
        float st, float mcd, int mi)
{
  reg_.setRegistrationParameters(cr, in_f, ns, st, mcd, mi);
}

void OpenNICapturer::setInputCloudParameters(float rl, float ls1, float ls2, float k, 
        float fr, u_int i)
{
  reg_.setInputCloudParamters(rl, ls1, ls2, k, fr, i);
}


void OpenNICapturer::run ()
{
  pcl::OpenNIGrabber grabber;

  reg_.clear ();

  boost::function<void (const PointCloud::ConstPtr&)> cloud_cb 
      = boost::bind (&OpenNICapturer::cloudCallback, this, _1);
  boost::signals2::connection cloud_connection 
      = grabber.registerCallback (cloud_cb);

  grabber.start ();

  int count = 0;

  while (!cloud_viewer_->wasStopped ())
  {
    PointCloud::ConstPtr cloud;
    
    // cout << ++ count << endl;

    // See if we can get a cloud
    if (cloud_mutex_.try_lock ())
    {
      cloud_.swap (cloud);
      cloud_mutex_.unlock ();
    }

    if (cloud && save_clouds_)
    {
      cout << ++count << endl;
      reg_.setInputCloud (cloud);

      if (reg_.canAlign ())
      {
        // cout << "ssss" << endl;
        reg_.align ();
        // cout << "tttt" << endl;
        if (reg_.hasConverged ())
        {
          cout << "Cloud size: " << reg_.getResultantCloud ()->points.size() << endl;
          // showCloudRight (reg_.getResultantCloud ());
          if (viewer_mutex_.try_lock ())
          {
            showCloudRight (reg_.getResultantCloud ());
            viewer_mutex_.unlock ();
          }
          // PointCloud::Ptr temp(new PointCloud);
          // copyPointCloud(*cloud, *temp);
          // showCloudRight (temp);
          // cout << "uuuu" << endl;
          // PCL_INFO ("Registration score: %d\n", reg_.getFitnessScore ());
          writer.writeToFile (reg_.getResultantCloud ());
        }
      }
    }

    if (!grabber.isRunning ())
    {
      cloud_viewer_->spin ();
    }

    // boost::this_thread::sleep (boost::posix_time::microseconds (100));
  }

  grabber.stop ();

  cloud_connection.disconnect ();

  writer.saveText ();
}