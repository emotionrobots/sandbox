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

ros_rp2w::Command::Request master_req;

boost::mutex mutex;

bool command(ros_rp2w::Command::Request  &req,
  ros_rp2w::Command::Response &res) {
  boost::mutex::scoped_lock lock(mutex);
  master_req = req;
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
     cout << "No R2PW robot" << endl;
     return 1;
   }

   while (ros::ok()) {
    rc = robot.update();
    
    if (rc != rp2w::OK) {
      cout << "robot.update failed (" << rc << ")";
    }
    else {
      boost::mutex::scoped_lock lock(mutex);
      ros_rp2w::Packet packet;

      packet.rightMotorSpeed = (uint8_t)(robot.getLeftMotorSpeed());
      packet.leftMotorSpeed = (uint8_t)(robot.getRightMotorSpeed());
      packet.cameraTilt = robot.getCameraTilt();
      packet.cameraPan = robot.getCameraPan();
      packet.digital1 = (uint8_t)(robot.getGPIO1());
      packet.digital2 = (uint8_t)(robot.getGPIO2());

      packet.encoderA = (int32_t)(robot.getEncoderA());
      packet.encoderB = (int32_t)(robot.getEncoderB());
      packet.batteryVoltage = (uint8_t)(robot.getBatteryVoltage());
      packet.frontSonar = (uint8_t)(robot.getFrontSonar());
      packet.rearSonar = (uint8_t)(robot.getRearSonar());
      packet.bumper = (uint8_t)(robot.getBumper());
      /**
     * The publish() function is how you send messages. The parameter
     * is the message object. The type of this object must agree with the type
     * given as a template parameter to the advertise<>() call, as was done
     * in the constructor above.
     */
     pub.publish(packet);
   }

   if (mutex.try_lock()) {
    if (master_req.rightMotorSpeedCommand) {
      cout << "Right Motor Speed changed to " << master_req.rightMotorSpeed;
      robot.setRightMotorSpeed((char)(master_req.rightMotorSpeed));
    }
    if (master_req.leftMotorSpeedCommand) {
      cout << "Right Motor Speed changed to " << master_req.leftMotorSpeed;
      robot.setLeftMotorSpeed((char)(master_req.leftMotorSpeed));
    }
    if (master_req.cameraTiltCommand) {
      cout << "Camera tilt changed to " << master_req.cameraTilt;
      robot.setCameraTilt(master_req.cameraTilt);
    }
    if (master_req.cameraPanCommand) {
      cout << "Camera tilt changed to " << master_req.cameraPan;
      robot.setCameraPan(master_req.cameraPan);
    }
    if (master_req.digital1Command) {
      cout << "GPIO1 changed to " << master_req.digital1;
      robot.setGPIO1((char)(master_req.digital1));
    }
    if (master_req.digital2Command) {
      cout << "GPIO2 changed to " << master_req.digital2;
      robot.setGPIO2((char)(master_req.digital2));
    }
    rc = robot.update();
  }
  ros::spinOnce();
  loop_rate.sleep();
}

return 0;
}
