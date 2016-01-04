#include "ros/ros.h"
#include "ros_rp2w/Command.h"
#include <cstdlib>

int main(int argc, char **argv)
{
  ros::init(argc, argv, "rp2w_client");
  ros::NodeHandle n;
  ros::ServiceClient client = n.serviceClient<ros_rp2w::Command>("rp2w_command");
  ros_rp2w::Command srv;
  srv.request.rightMotorSpeedCommand = false; 
  srv.request.rightMotorSpeed = 0;
  srv.request.leftMotorSpeedCommand = false; 
  srv.request.leftMotorSpeed = 0; 
  srv.request.cameraTiltCommand = false; 
  srv.request.cameraTilt = 0; 
  srv.request.cameraPanCommand = false; 
  srv.request.cameraPan = 0;
  if (client.call(srv))
  {
    // ROS_INFO("Sum: %ld", (long int)srv.response.sum);
  }
  else
  {
    ROS_ERROR("Failed to call service");
    return 1;
  }

  return 0;
}