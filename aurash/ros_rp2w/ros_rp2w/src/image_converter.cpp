

#include <ros/ros.h>
#include <image_transport/image_transport.h>
#include <cv_bridge/cv_bridge.h>
#include <sensor_msgs/image_encodings.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>
#include </home/emotion/Software/dlib/dlib/opencv.h>
#include </home/emotion/Software/dlib/dlib/image_processing/frontal_face_detector.h>
#include </home/emotion/Software/dlib/dlib/image_processing/render_face_detections.h>
#include </home/emotion/Software/dlib/dlib/image_processing.h>
#include </home/emotion/Software/dlib/dlib/gui_widgets.h>
#include <vector>
#include <sstream>
#include "std_msgs/MultiArrayLayout.h"
#include "std_msgs/Header.h"
#include "std_msgs/MultiArrayDimension.h"
#include "std_msgs/UInt32MultiArray.h"
#include "ros_rp2w/face_p.h"



using namespace dlib;
using namespace std;


shape_predictor pose_model;
frontal_face_detector detector;
std::vector<int>number;

//static const std::string OPENCV_WINDOW = "Image window";

class ImageConverter
{
  ros::NodeHandle nh_;
  image_transport::ImageTransport it_;
  image_transport::Subscriber image_sub_;
  image_transport::Publisher image_pub_;
  ros::Publisher face_points_;
  
public:
  ImageConverter()
    : it_(nh_)
  {
    // Subscrive to input video feed and publish output video feed
    image_sub_ = it_.subscribe("rgb2", 1, 
      &ImageConverter::imageCb, this);
    image_pub_ = it_.advertise("/image_converter/output_video", 1);
    face_points_ = nh_.advertise<ros_rp2w::face_p>("face_points", 1000);

    //cv::namedWindow(OPENCV_WINDOW);
  }




  ~ImageConverter()
  {
    //cv::destroyWindow(OPENCV_WINDOW);
  }

  /*boost::python::list toPythonList(std::vector<cv::Point> vector) {
  typename std::vector<cv::Point>::iterator iter;
  boost::python::list list;
  for (iter = vector.begin(); iter != vector.end(); ++iter) {
    list.append(*iter);
  }
  return list;
}*/

  void imageCb(const sensor_msgs::ImageConstPtr& msg)
  {
    cv_bridge::CvImagePtr cv_ptr;
    try
    {
      cv_ptr = cv_bridge::toCvCopy(msg, sensor_msgs::image_encodings::BGR8);
    }
    catch (cv_bridge::Exception& e)
    {
      ROS_ERROR("cv_bridge exception: %s", e.what());
      return;
    }
    //try
    //{
        
        //image_window win;

        // Load face detection and pose estimation models.
            

        // Grab and process frames until the main window is closed by the user.
            
            // Grab a frame
            cv::Mat temp;
            temp= cv_ptr->image;
            // Turn OpenCV's Mat into something dlib can deal with.  Note that this just
            // wraps the Mat object, it doesn't copy anything.  So cimg is only valid as
            // long as temp is valid.  Also don't do anything to temp that would cause it
            // to reallocate the memory which stores the image as that will make cimg
            // contain dangling pointers.  This basically means you shouldn't modify temp
            // while using cimg.
            cv_image<bgr_pixel> cimg(temp);

            // Detect faces 
            std::vector<rectangle> faces = detector(cimg);
            // Find the pose of each face.
            std::vector<full_object_detection> shapes;
            cv::Mat temp2;
            temp2= toMat(cimg);
            std_msgs::UInt32MultiArray array;
            ros_rp2w::face_p msg2;
            for (unsigned long i = 0; i < faces.size(); ++i)
                {
                  //shapes.push_back(pose_model(cimg, faces[i]));
                  full_object_detection shape = pose_model(cimg, faces[i]);

                  for( int i=0; i<shape.num_parts();i++)
                    {
                      
                      //assign array a random number between 0 and 255.
                      array.data.push_back(int(shape.part(i).x()));
                      array.data.push_back(int(shape.part(i).y()));
                      msg2.image = *cv_ptr->toImageMsg();
                      cv::circle(temp2, cv::Point(shape.part(i).x(),shape.part(i).y()), 2, (0,0,255), CV_FILLED, CV_AA, 0);
                      }

                      //int fontFace = CV_FONT_HERSHEY_SCRIPT_SIMPLEX;
                      //double fontScale = .3;
                      //int thickness = 1;  
                      //ostringstream convert;
                      //convert << i; 
                      //cv::putText(temp2, convert.str(), cv::Point(shape.part(i).x(),shape.part(i).y()), fontFace, fontScale, (0,0,255), thickness,8);
                    }
                    msg2.arr=array;
                    cout <<array<< endl;
                    msg2.header.stamp=  ros::Time::now();
                    face_points_.publish(msg2);
                    
                 
                  //cout << "number of parts: "<< shape.num_parts() << endl;
                  //cout << "pixel position of first part:  " << shape.part(0) << endl;
                  //cout << "pixel position of second part: " << shape.part(1) << endl;
                
            // Display it all on the screen
            //win.clear_overlay();
            //win.set_image(cimg);
            //win.add_overlay(render_face_detections(shapes));
            int flags;
            //cv::namedWindow("dlib landmarking",  flags=CV_WINDOW_NORMAL);
            //cv::imshow("dlib landmarking", temp2);
            //cv::waitKey(3);


      //  }
    
    /*catch(serialization_error& e)
    {
        cout << "You need dlib's default face landmarking model file to run this example." << endl;
        cout << "You can get it from the following URL: " << endl;
        cout << "   http://dlib.net/files/shape_predictor_68_face_landmarks.dat.bz2" << endl;
        cout << endl << e.what() << endl;
    }
    catch(exception& e)
    {
        cout << e.what() << endl;
    }*/
    
    // Update GUI Window

    
    // Output modified video stream
    //image_pub_.publish(cv_ptr->toImageMsg());
  }
};

int main(int argc, char** argv)
{

  detector = get_frontal_face_detector();
  deserialize("/home/emotion/Software/dlib/examples/build/shape_predictor_68_face_landmarks.dat") >> pose_model;
  ros::init(argc, argv, "image_converter");
  ImageConverter ic;
  ros::spin();
  return 0;
}
