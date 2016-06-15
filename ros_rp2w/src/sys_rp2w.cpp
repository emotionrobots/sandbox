#include "ros/ros.h"
#include "ros_rp2w/Packet.h"
#include "ros_rp2w/AdvancedCommand.h"
#include "rp2w_serial.h"
#include <sstream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termio.h>
#include <boost/thread/mutex.hpp>
#include <boost/shared_ptr.hpp>

using namespace std;
using namespace LibSerial;

rp2w::Status rc;
boost::shared_ptr<rp2w> robot(new rp2w());
char digital1 = 0;
char digital2 = 0x0c;
int theta_;
double distance_;
boost::mutex msg_mutex;

const double LINEAR_CONVERSION_FACTOR = 760;
const double ANGULAR_CONVERSION_FACTOR = 860;
const double LINEAR_CONVERSION = M_PI/LINEAR_CONVERSION_FACTOR*5/12;
const double ANGULAR_CONVERSION = ANGULAR_CONVERSION_FACTOR/5*12/360;

void setMotorSpeeds(int turn_speed, int trav_speed) {
  digital1 = robot->getGPIO1();
  int l_speed = trav_speed + turn_speed;
  int r_speed = trav_speed - turn_speed;
  if (l_speed >= 0) {
    digital1 &= ~(0x80);
  }
  else {
    digital1 |= 0x80;
  }
  if (r_speed >= 0) {
    digital1 &= ~(0x40);
  }
  else {
    digital1 |= 0x40;
  }
  char l_motor = abs(l_speed);
  char r_motor = abs(r_speed);
  // ROS_INFO("Left Motor: %u", (unsigned char)(l_motor));
  // ROS_INFO("Right Motor: %u", (unsigned char)(r_motor));
  robot->setLeftMotorSpeed(l_motor);
  robot->setRightMotorSpeed(r_motor);
  robot->setGPIO1(digital1);
  rc = robot->update();
  int fail_count = 0;
  while (rc != rp2w::OK) {
    fail_count++;
    cout << "robot.update failed (" << rc << ") x " << fail_count << endl;
    robot = boost::shared_ptr<rp2w>(new rp2w());
    rc = robot->connect("/dev/ttyUSB0");
    rc = robot->update();
  }
  if (fail_count > 0) {
    cout << "robot reconnected" << endl;
    digital1 = 0;
    digital2 = 0;
    robot->setGPIO1(digital1);
    robot->setGPIO2(digital2);
    robot->setLeftMotorSpeed(0);
    robot->setRightMotorSpeed(0);
  }
  // cout << "Motor speeds set to TURN SPEED " << turn_speed 
  //      << " And TRAVEL SPEED " << trav_speed << endl;
}

void command(const ros_rp2w::AdvancedCommand::ConstPtr& msg) {
  // cout << "Command Received. " << endl;
  boost::mutex::scoped_lock lock(msg_mutex);
  theta_ = msg->theta;
  distance_ = msg->distance;
}

int main(int argc, char **argv) {
 ros::init(argc, argv, "rp2w");
 ros::NodeHandle n;

 rc = robot->connect("/dev/ttyUSB0");
 rc = robot->update();
 if (rc != rp2w::OK) {
   cout << "No RP2W robot. " << endl;
   return 1;
 }
 else {
   cout << "RP2W Initialized. " << endl;
 }

 robot->setGPIO1(digital1);
 robot->setGPIO2(digital2);
 setMotorSpeeds(0, 0);
 
 ros::Publisher pub = n.advertise<ros_rp2w::Packet>("rp2w_packet", 10);
 ros::Subscriber sub = n.subscribe("rp2w/advanced_command", 1, command);
 ros::Rate loop_rate(60);
 bool stopped_early;

 while (ros::ok()) {
  rc = robot->update();
  int fail_count = 0;
  while (rc != rp2w::OK) {
    fail_count++;
    cout << "robot.update failed (" << rc << ") x " << fail_count << endl;
    robot = boost::shared_ptr<rp2w>(new rp2w());
    rc = robot->connect("/dev/ttyUSB0");
    rc = robot->update();
  }
  if (fail_count > 0) {
    cout << "robot reconnected" << endl;
    digital1 = 0;
    digital2 = 0;
    robot->setGPIO1(digital1);
    robot->setGPIO2(digital2);
  }
  if (msg_mutex.try_lock()) {
    int theta = theta_;
    int theta_conv = abs(theta*ANGULAR_CONVERSION);
    double distance = distance_;
    theta_ = 0;
    distance_ = 0;
    msg_mutex.unlock();
    stopped_early = false;
    if (theta != 0) {
      int start = robot->getEncoderA();
      // int now = robot.getEncoderA();
      // cout << "Start: " << start << endl;
      while (abs(robot->getEncoderA()-start) < theta_conv) {
        if ((uint8_t)(robot->getBumper()) != 0) {
          stopped_early = true;
          break;
        }
        if (theta >= 0) {
          // move counterclockwise
          setMotorSpeeds(112, 0);
        }
        else {
          // move clockwise
          setMotorSpeeds(-112, 0);
        }
        // now = robot.getEncoderA();
        // cout << now << " " << abs(now-start)*ANGULAR_CONVERSION << endl;
      }
      setMotorSpeeds(0, 0);
      // cout << "End: " << now << endl;
    }
    if (distance != 0) {
      int start = robot->getEncoderA(), now = robot->getEncoderA();
      // cout << "Start: " << start << endl;
      while (abs(now-start)*LINEAR_CONVERSION < abs(distance)
        // && (uint8_t)(robot.getFrontSonar()) > 5 
        // && (uint8_t)(robot.getRearSonar()) > 5
        ) {
        // loop_rate.sleep();
        if ((uint8_t)(robot->getBumper()) != 0) {
          stopped_early = true;
          break;
        }
        if (distance > 0) {
          // move forward
          if ((uint8_t)(robot->getFrontSonar()) <= 5) {
            stopped_early = true;
            break;
          }
          setMotorSpeeds(0, -128);
        }
        else {
          // move backward
          if ((uint8_t)(robot->getRearSonar()) <= 5) {
            stopped_early = true;
            break;
          }
          setMotorSpeeds(0, 128);
        }
        now = robot->getEncoderA();
        // cout << now << endl;
      }
      setMotorSpeeds(0, 0);
      // cout << "End: " << now << endl;
    }
  }
  ros_rp2w::Packet packet;

  packet.leftMotorSpeed = (uint8_t)(robot->getLeftMotorSpeed());
  packet.rightMotorSpeed = (uint8_t)(robot->getRightMotorSpeed());
  packet.cameraTilt = robot->getCameraTilt();
  packet.cameraPan = robot->getCameraPan();
  // packet.digital1 = (uint8_t)(robot.getGPIO1());
  // packet.digital2 = (uint8_t)(robot.getGPIO2());
  packet.digital1 = (uint8_t)(digital1);
  packet.digital2 = (uint8_t)(digital2);

  packet.encoderA = (uint32_t)(robot->getEncoderA());
  // cout << packet.encoderA << endl;
  packet.encoderB = (uint32_t)(robot->getEncoderB());
  // cout << packet.encoderB << endl;
  packet.batteryVoltage = (uint8_t)(robot->getBatteryVoltage());
  // ROS_INFO("battery voltage: %u", (unsigned char)(packet.batteryVoltage));
  packet.frontSonar = (uint8_t)(robot->getFrontSonar());
  // ROS_INFO("front sonar: %u", (unsigned char)(packet.frontSonar));
  packet.rearSonar = (uint8_t)(robot->getRearSonar());
  // ROS_INFO("rear sonar: %u", (unsigned char)(packet.rearSonar));
  packet.bumper = (uint8_t)(robot->getBumper());
  // ROS_INFO("bumper: %u", (unsigned char)(packet.bumper));

  pub.publish(packet);

  ros::spinOnce();
  loop_rate.sleep();
}

return 0;
}
