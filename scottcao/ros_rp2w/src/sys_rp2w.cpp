#include "ros/ros.h"
#include "ros_rp2w/AdvancedCommand.h"
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
ros::Rate loop_rate(45);
int current_direction;

const float LINEAR_SPEED = 1;
const float ANGULAR_SPEED = 1;

void setMotorSpeeds(int l_speed, int r_speed) {
  char digital1 = robot.getGPIO1();
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
  robot.setLeftMotorSpeed(abs(l_speed));
  robot.setRightMotorSpeed(abs(r_speed));
  robot.setGPIO1(digital1);
  rc = robot.update();
}

void command(const ros_rp2w::AdvancedCommand::ConstPtr& msg) {
  if (msg->thetaCommand) {
    double theta = msg->theta;
    if (theta >= 0) {
      setMotorSpeeds(-128, 128);
    }
    else {
      setMotorSpeeds(128, -128);
    }
    double start = ros::Time::now().toSec();
    while (theta > 0) {
      loop_rate.sleep();
      double now = ros::Time::now().toSec();
      theta -= ANGULAR_SPEED*(now-start);
      start = now;
    }
    setMotorSpeeds(0, 0);
  }
  if (msg->distanceCommand) {
    double distance = msg->distance;
    setMotorSpeeds(128, 128);
    double start = ros::Time::now().toSec();
    while (distance > 0) {
      loop_rate.sleep();
      double now = ros::Time::now().toSec();
      distance -= LINEAR_SPEED*(now-start);
      start = now;
    }
    setMotorSpeeds(0, 0);
  }
}

int main(int argc, char **argv) {
   ros::init(argc, argv, "rp2w");
   ros::NodeHandle n;

   rc = robot.connect("/dev/ttyUSB0");
   if (rc != rp2w::OK) {
     cout << "No RP2W robot" << endl;
     return 1;
   }
   else {
     cout << "RP2W Initialized. " << endl;
   }

   ros::Subscriber sub = n.subscribe("rp2w/advanced_command", 1, command);
   ros::spin();

  return 0;
}
