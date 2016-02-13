/*
 * Copyright (C) 2008, Morgan Quigley and Willow Garage, Inc.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *   * Redistributions of source code must retain the above copyright notice,
 *     this list of conditions and the following disclaimer.
 *   * Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 *   * Neither the names of Stanford University or Willow Garage, Inc. nor the names of its
 *     contributors may be used to endorse or promote products derived from
 *     this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

// %Tag(FULLTEXT)%
#include "ros/ros.h"
#include "std_msgs/String.h"
#include </home/aurash/dlib/dlib/opencv.h>
#include <opencv2/highgui/highgui.hpp>
#include </home/aurash/dlib/dlib/image_processing/frontal_face_detector.h>
#include </home/aurash/dlib/dlib/image_processing/render_face_detections.h>
#include </home/aurash/dlib/dlib/image_processing.h>
#include </home/aurash/dlib/dlib/gui_widgets.h>
#include <sstream>    
#include <string> 
#include <vector> 
//#include </home/aurash/catkin_ws/src/skeleton/scripts/shape_predictor_68_face_landmarks.dat>
using namespace dlib;
using namespace std;

/**
 * This tutorial demonstrates simple receipt of messages over the ROS system.
 */
// %Tag(CALLBACK)%
void callback_rgb(const std_msgs::String::ConstPtr& msg)
{
  std::vector<int> vectordata(msg->data.begin(),msg->data.end());
  cv::Mat data_mat(vectordata,true);
  cv::Mat temp(cv::imdecode(data_mat,1));
  cout<<"Height: " << temp.rows <<" Width: "<<temp.cols<<endl; 
  //put 0 if you want greyscale
  ROS_INFO("I heard: %s", msg->data.c_str());
 
  
        //cv::VideoCapture cap(0);

        
        image_window win;

        // Load face detection and pose estimation models.
        frontal_face_detector detector = get_frontal_face_detector();
        shape_predictor pose_model;
        deserialize("/home/aurash/dlib/examples/build/shape_predictor_68_face_landmarks.dat") >> pose_model;
        time_t start, end;
        time(&start);
        int count=0;

        // Grab and process frames until the main window is closed by the user.

            
            // Grab a frame
            //cv::Mat temp;
            //cap >> temp;
            count=count+1;
            time(&end);
            double seconds = difftime(end, start);
            double fps=(count/seconds);
            cout<<fps<<" "<<"Printed out variable fps"<<endl;
            // Turn OpenCV's Mat into something dlib can deal with.  Note that this just
            // wraps the Mat object, it doesn't copy anything.  So cimg is only valid as
            // long as temp is valid.  Also don't do anything to temp that would cause it
            // to reallocate the memory which stores the image as that will make cimg
            // contain dangling pointers.  This basically means you shouldn't modify temp
            // while using cimg.
            cv_image<bgr_pixel> cimg(temp);
            //cv::putText(cimg, nfps,cv::Point(10,10), cv::FONT_HERSHEY_SIMPLEX, 1, cv::Scalar(0,200,200), 4);
            // Detect faces 
            std::vector<rectangle> faces = detector(cimg);
            // Find the pose of each face.
            std::vector<full_object_detection> shapes;
            for (unsigned long i = 0; i < faces.size(); ++i)
                shapes.push_back(pose_model(cimg, faces[i]));

            // Display it all on the screen
            win.clear_overlay();
            win.set_image(cimg);
            win.add_overlay(render_face_detections(shapes));
        

  
}
// %EndTag(CALLBACK)%

int main(int argc, char **argv)
{

  ros::init(argc, argv, "listener");

  /**
   * NodeHandle is the main access point to communications with the ROS system.
   * The first NodeHandle constructed will fully initialize this node, and the last
   * NodeHandle destructed will close down the node.
   */
  ros::NodeHandle n;

// %Tag(SUBSCRIBER)%
  ros::Subscriber sub = n.subscribe("rgb", 1000, callback_rgb);
  cout << "hello"<<endl;

// %EndTag(SUBSCRIBER)%

  /**
   * ros::spin() will enter a loop, pumping callbacks.  With this version, all
   * callbacks will be called from within this thread (the main one).  ros::spin()
   * will exit when Ctrl-C is pressed, or the node is shutdown by the master.
   */
// %Tag(SPIN)%
  ros::spin();
// %EndTag(SPIN)%

  return 0;
}