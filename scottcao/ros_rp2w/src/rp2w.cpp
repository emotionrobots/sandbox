#include "ros/ros.h"
#include "std_msgs/String.h"
#include "ros_rp2w/Packet.h"
#include "ros_rp2w/Command.h"
#include "rp2w_serial.h"
#include <sstream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termio.h>
#include <boost/thread/mutex.hpp>

using namespace std;
using namespace LibSerial;

rp2w::Status rc;
rp2w robot;

boost::mutex mutex;

bool command(ros_rp2w::Command::Request  &req,
  ros_rp2w::Command::Response &res) {
  boost::mutex::scoped_lock lock(mutex);
  if (req.leftMotorSpeedCommand) {
    ROS_INFO("Left Motor Speed changed to %u\n", (unsigned char)(req.leftMotorSpeed));
    robot.setLeftMotorSpeed(req.leftMotorSpeed);
  }
  if (req.rightMotorSpeedCommand) {
    ROS_INFO("Right Motor Speed changed to %u\n", (unsigned char)(req.rightMotorSpeed));
    robot.setRightMotorSpeed(req.rightMotorSpeed);
  }
  if (req.cameraTiltCommand) {
    cout << "Camera tilt changed to " << req.cameraTilt << endl;
    robot.setCameraTilt(req.cameraTilt);
  }
  if (req.cameraPanCommand) {
    cout << "Camera pan changed to " << req.cameraPan << endl;
    robot.setCameraPan(req.cameraPan);
  }
  if (req.digital1Command) {
    ROS_INFO("GPIO1 changed to %u\n", (unsigned char)(req.digital1));
    robot.setGPIO1(req.digital1);
  }
  if (req.digital2Command) {
    ROS_INFO("GPIO2 changed to %u\n", (unsigned char)(req.digital2));
    robot.setGPIO2(req.digital2);
  }
  // res.commandSuccessful = true;
  rc = robot.update();
  if (rc != rp2w::OK) {
    cout << "robot.update failed (" << rc << ")";
    return false;
  }
  return true;
}

int main(int argc, char **argv) {
  /**
   * The ros::init() function needs to see argc and argv so that it can perform
   * any ROS arguments and name remapping that were provided at the command line.
   * For programmatic remappings you can use a different version of init() which takes
   * remappings directly, but for most command-line programs, passing argc and argv is
   * the easiest way to do it.  The third argument to init() is the name of the node.
   */
   ros::init(argc, argv, "rp2w");

  /**
   * NodeHandle is the main access point to communications with the ROS system.
   * The first NodeHandle constructed will fully initialize this node, and the last
   * NodeHandle destructed will close down the node.
   */
   ros::NodeHandle n;
   ros::Publisher pub = n.advertise<ros_rp2w::Packet>("rp2w_packet", 1000);
   ros::Rate loop_rate(20);

   ros::ServiceServer service = n.advertiseService("rp2w_command", command);

   rc = robot.connect("/dev/ttyUSB0");
   if (rc != rp2w::OK) {
     cout << "No RP2W robot" << endl;
     return 1;
   }
   else {
     cout << "RP2W Initialized. " << endl;
   }

   while (ros::ok()) {
    if (mutex.try_lock()) {
      rc = robot.update();
      if (rc != rp2w::OK) {
        cout << "robot.update failed (" << rc << ")";
      }
      else {
        // cout << "robot.update successful" << endl;
        ros_rp2w::Packet packet;

        packet.leftMotorSpeed = (uint8_t)(robot.getLeftMotorSpeed());
        packet.rightMotorSpeed = (uint8_t)(robot.getRightMotorSpeed());
        packet.cameraTilt = robot.getCameraTilt();
        packet.cameraPan = robot.getCameraPan();
        packet.digital1 = (uint8_t)(robot.getGPIO1());
        packet.digital2 = (uint8_t)(robot.getGPIO2());

        packet.encoderA = (int32_t)(robot.getEncoderA());
        // cout << packet.encoderA << endl;
        packet.encoderB = (int32_t)(robot.getEncoderB());
        // cout << packet.encoderB << endl;
        packet.batteryVoltage = (uint8_t)(robot.getBatteryVoltage());
        // ROS_INFO("battery voltage: %u\n", (unsigned char)(packet.batteryVoltage));
        packet.frontSonar = (uint8_t)(robot.getFrontSonar());
        // ROS_INFO("front sonar: %u\n", (unsigned char)(packet.frontSonar));
        packet.rearSonar = (uint8_t)(robot.getRearSonar());
        // ROS_INFO("rear sonar: %u\n", (unsigned char)(packet.rearSonar));
        packet.bumper = (uint8_t)(robot.getBumper());
        // ROS_INFO("bumper: %2X\n", packet.bumper);
        // ROS_INFO("bumper: %u\n", (unsigned char)(packet.bumper));
        //   /**
        //  * The publish() function is how you send messages. The parameter
        //  * is the message object. The type of this object must agree with the type
        //  * given as a template parameter to the advertise<>() call, as was done
        //  * in the constructor above.
        //  */
        pub.publish(packet);
      }
      mutex.unlock();
    }
    ros::spinOnce();
    loop_rate.sleep();
  }

  return 0;
}
