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
// ros::Rate loop_rate(60);
int current_direction;
char digital1 = 0;
char digital2 = 0x0c;
int l_speed = 0;
int r_speed = 0;

const double LINEAR_CONVERSION = M_PI/1024*5/12;
const double ANGULAR_CONVERSION = 2048*0.0635/0.2032*360;

void setMotorSpeeds(int turn_speed, int trav_speed) {
  digital1 = robot.getGPIO1();
  l_speed = trav_speed + turn_speed;
  r_speed = trav_speed - turn_speed;
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
  robot.setLeftMotorSpeed(l_motor);
  robot.setRightMotorSpeed(r_motor);
  robot.setGPIO1(digital1);
  rc = robot.update();
  // cout << "Motor speeds set to TURN SPEED " << turn_speed 
  //      << " And TRAVEL SPEED " << trav_speed << endl;
}

void command(const ros_rp2w::AdvancedCommand::ConstPtr& msg) {
  cout << "Command Received. " << endl;
  if (msg->thetaCommand) {
    double theta = msg->theta;
    if (theta >= 0) {
      setMotorSpeeds(-128, 0);
    }
    else {
      setMotorSpeeds(128, 0);
    }
    int start = robot.getEncoderA(), now;
    while (theta > 0) {
      // loop_rate.sleep();
      theta -= abs(now-start)*ANGULAR_CONVERSION;
      start = now;
    }
    setMotorSpeeds(0, 0);
  }
  if (msg->distanceCommand) {
    int start = robot.getEncoderA(), now = robot.getEncoderA();
    cout << "Start: " << start << endl;
    while ((now-start)*LINEAR_CONVERSION < msg->distance) {
      // loop_rate.sleep();
      if (msg->distance > 0) {
        setMotorSpeeds(0, -128);
      }
      else {
        setMotorSpeeds(0, 128);
      }
      now = robot.getEncoderA();
      // cout << now << endl;
    }
    cout << "End: " << now << endl;
    setMotorSpeeds(0, 0);
  }
  cout << "Finished." << endl;
}

int main(int argc, char **argv) {
   ros::init(argc, argv, "rp2w");
   ros::NodeHandle n;

   rc = robot.connect("/dev/ttyUSB0");
   rc = robot.update();
   if (rc != rp2w::OK) {
     cout << "No RP2W robot" << endl;
     return 1;
   }
   else {
     cout << "RP2W Initialized. " << endl;
   }

   robot.setGPIO1(digital1);
   robot.setGPIO2(digital2);
   setMotorSpeeds(0, 0);
   
   ros::Subscriber sub = n.subscribe("rp2w/advanced_command", 1, command);
   ros::spin();

  return 0;
}
