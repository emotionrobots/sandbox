#include "ros/ros.h"
#include "ros_rp2w/AdvancedCommand.h"

using namespace std;


int main(int argc, char **argv)
{
  ros::init(argc, argv, "rp2w_pub");
  ros::NodeHandle n;
  ros::Publisher pub = n.advertise<ros_rp2w::AdvancedCommand>("rp2w/advanced_command", 1);
  // ros::Rate loop_rate(10);

  ros_rp2w::AdvancedCommand msg;
  bool distanceCommand = true;
  int distance = 1;
  bool thetaCommand = false;
  int theta = 0;
  msg.distanceCommand = distanceCommand;
  msg.distance = distance;
  msg.thetaCommand = thetaCommand;
  msg.theta = theta;
  pub.publish(msg);

  // cout << "distanceCommand: " << distanceCommand
  //      << "\tdistance: " << distance
  //      << "\tthetaCommand: " << thetaCommand
  //      << "\ttheta: " << theta << endl;

  // while (ros::ok())
  // {
  //   /**
  //    * This is a message object. You stuff it with data, and then publish it.
  //    */
  //   std_msgs::String msg;

  //   std::stringstream ss;
  //   ss << "hello world " << count;
  //   msg.data = ss.str();

  //   ROS_INFO("%s", msg.data.c_str());
     
  //   chatter_pub.publish(msg);

  //   ros::spinOnce();

  //   loop_rate.sleep();
  //   ++count;
  // }


  return 0;
}
