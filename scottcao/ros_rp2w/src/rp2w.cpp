#include "ros/ros.h"
#include "std_msgs/String.h"
#include "ros_rp2w/Packet.h"
#include "ros_rp2w/Command.h"
// #include "joystick.h"
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

ros_rp2w::Command::Request master;

boost::mutex mutex;

bool command(ros_rp2w::Command::Request  &req,
  ros_rp2w::Command::Response &res) {
  boost::mutex::scoped_lock lock(mutex);
  master = req;
  cout << "Master Request Updated." << endl;
  res.commandSuccessful = true;
  return true;
}

int main(int argc, char **argv) {
  /**
   * The ros::init() function needs to see argc and argv so that it can perform
   * any ROS arguments and name remapping that were provided at the command line.
   * For programmatic remappings you can use a different version of init() which takes
   * remappings directly, but for most command-line programs, passing argc and argv is
   * the easiest way to do it.  The third argument to init() is the name of the node.
   *
   * You must call one of the versions of ros::init() before using any other
   * part of the ROS system.
   */
   ros::init(argc, argv, "rp2w");

  /**
   * NodeHandle is the main access point to communications with the ROS system.
   * The first NodeHandle constructed will fully initialize this node, and the last
   * NodeHandle destructed will close down the node.
   */
   ros::NodeHandle n;

  /**
   * The advertise() function is how you tell ROS that you want to
   * publish on a given topic name. This invokes a call to the ROS
   * master node, which keeps a registry of who is publishing and who
   * is subscribing. After this advertise() call is made, the master
   * node will notify anyone who is trying to subscribe to this topic name,
   * and they will in turn negotiate a peer-to-peer connection with this
   * node.  advertise() returns a Publisher object which allows you to
   * publish messages on that topic through a call to publish().  Once
   * all copies of the returned Publisher object are destroyed, the topic
   * will be automatically unadvertised.
   *
   * The second parameter to advertise() is the size of the message queue
   * used for publishing messages.  If messages are published more quickly
   * than we can send them, the number here specifies how many messages to
   * buffer up before throwing some away.
   */
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

   // if (mutex.try_lock()) {
   //   master.leftMotorSpeed = 0;
   //   master.rightMotorSpeed = 0;
   //   master.cameraTiltShift = -1560;
   //   master.cameraPanShift = -1500;
   //   master.digital1 = 0;
   //   master.digital2 = 0; 
   //   mutex.unlock();
   //   cout << "Setup complete" << endl;
   // }

   int count = 0;
   while (ros::ok()) {
    if (mutex.try_lock()) {
      if (master.leftMotorSpeedCommand) {
        if (master.leftMotorSpeed >= 0) {
          master.digital1 &= ~(0x80);
        }
        else {
          master.digital1 |= 0x80;
        }
        printf("Left Motor Speed changed to %u\n", (unsigned char)(master.leftMotorSpeed));
        robot.setLeftMotorSpeed(abs(master.leftMotorSpeed));
      }
      if (master.rightMotorSpeedCommand) {
        if (master.rightMotorSpeed >= 0) {
          master.digital1 &= ~(0x40);
        }
        else {
          master.digital1 |= 0x40;
        }
        printf("Right Motor Speed changed to %u\n", (unsigned char)(master.rightMotorSpeed));
        robot.setRightMotorSpeed(abs(master.rightMotorSpeed));
      }
      if (master.cameraTiltShiftCommand) {
        cout << "Camera tilt changed to " << robot.getCameraTilt()-master.cameraTiltShift << endl;
        robot.setCameraTilt(robot.getCameraTilt()-master.cameraTiltShift);
        master.cameraTiltShift = 0;
      }
      if (master.cameraPanShiftCommand) {
        cout << "Camera pan changed to " << robot.getCameraPan()-master.cameraPanShift << endl;
        robot.setCameraPan(robot.getCameraPan()-master.cameraPanShift);
        master.cameraPanShift = 0;
      }
      if (master.digital1Command) {
        printf("GPIO1 changed to %u\n", (unsigned char)(master.digital1));
        robot.setGPIO1(master.digital1);
      }
      if (master.digital2Command) {
        printf("GPIO2 changed to %u\n", (unsigned char)(master.digital2));
        robot.setGPIO2(master.digital2);
      }
      mutex.unlock();
    }

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
      // printf("battery voltage: %u\n", (unsigned char)(packet.batteryVoltage));
      packet.frontSonar = (uint8_t)(robot.getFrontSonar());
      // printf("front sonar: %u\n", (unsigned char)(packet.frontSonar));
      packet.rearSonar = (uint8_t)(robot.getRearSonar());
      // printf("rear sonar: %u\n", (unsigned char)(packet.rearSonar));
      packet.bumper = (uint8_t)(robot.getBumper());
      // printf("bumper: %2X\n", packet.bumper);
      // printf("bumper: %u\n", (unsigned char)(packet.bumper));
    //   /**
    //  * The publish() function is how you send messages. The parameter
    //  * is the message object. The type of this object must agree with the type
    //  * given as a template parameter to the advertise<>() call, as was done
    //  * in the constructor above.
    //  */
      pub.publish(packet);
    }
    ros::spinOnce();
    loop_rate.sleep();
  }

  return 0;
}