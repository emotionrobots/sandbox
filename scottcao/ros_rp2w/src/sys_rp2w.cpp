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

const double LINEAR_CONVERSION_FACTOR = 840;
const double ANGULAR_CONVERSION_FACTOR = 840;
const double LINEAR_CONVERSION = 1/M_PI*LINEAR_CONVERSION_FACTOR/5*12;
const double ANGULAR_CONVERSION = ANGULAR_CONVERSION_FACTOR/5*12/360;

rp2w::Status rc;
boost::shared_ptr<rp2w> robot(new rp2w());
char digital1;
char digital2;
int theta_;
double distance_;
boost::mutex msg_mutex;
bool stopped_early;
double stopping_distance = 1*LINEAR_CONVERSION;
int command_identity_ = 0;
int move_identity_ = 0;

void initialize() {
  digital1 = 0;
  digital2 = 0x0c;
  robot->setGPIO1(digital1);
  robot->setGPIO2(digital2);
  robot->setLeftMotorSpeed(0);
  robot->setRightMotorSpeed(0);
  robot->setCameraPan(0);
  robot->setCameraTilt(0);
}

bool update() {
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
    initialize();
    return false;
  }
  return true;
}

void setMotorSpeeds(int turn_speed, int trav_speed) {
  digital1 = robot->getGPIO1();
  int left_speed = trav_speed + turn_speed;
  int right_speed = trav_speed - turn_speed;
  if (left_speed >= 0) {
    digital1 &= ~(0x80);
  }
  else {
    digital1 |= 0x80;
  }
  if (right_speed >= 0) {
    digital1 &= ~(0x40);
  }
  else {
    digital1 |= 0x40;
  }
  char left_motor = abs(left_speed);
  char right_motor = abs(right_speed);
// ROS_INFO("Left Motor: %u", (unsigned char)(left_motor));
// ROS_INFO("Right Motor: %u", (unsigned char)(right_motor));
  robot->setGPIO1(digital1);
  robot->setLeftMotorSpeed(left_motor);
  robot->setRightMotorSpeed(right_motor);
  update();
// cout << "Motor speeds set to TURN SPEED " << turn_speed 
//      << " And TRAVEL SPEED " << trav_speed << endl;
}

void command(const ros_rp2w::AdvancedCommand::ConstPtr& msg) {
  boost::mutex::scoped_lock lock(msg_mutex);
  theta_ = msg->theta;
  distance_ = msg->distance;
  command_identity_++;
}

bool commandOverride() {
  if (msg_mutex.try_lock()) {
    if (command_identity_ != move_identity_) {
      msg_mutex.unlock();
      ROS_INFO("Command %i overriden. New move established. ", move_identity_);
      return true;
    }
    msg_mutex.unlock();
  }
  return false;
}

void move(ros::Rate loop_rate) {
  if (msg_mutex.try_lock()) {
    if (move_identity_ < command_identity_) {
      move_identity_ = command_identity_;
      int theta = theta_;
      int theta_conv = abs(theta*ANGULAR_CONVERSION);
      double distance = distance_;
      double distance_conv = abs(distance*LINEAR_CONVERSION);
      theta_ = 0;
      distance_ = 0;
      msg_mutex.unlock();
      ROS_INFO("Command %i received. Turn %i degrees and move %.2f feet", move_identity_, theta, distance);
      stopped_early = false;
      if (theta != 0) {
        int start = robot->getEncoderA();
        while (abs(robot->getEncoderA()-start) < theta_conv) {
          ros::spinOnce();
          loop_rate.sleep();
          if (commandOverride()) {
            return;
          }
          if ((uint8_t)(robot->getBumper()) != 0) {
            stopped_early = true;
            ROS_ERROR("Bumper on. Stopped early. ");
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
        }
        setMotorSpeeds(0, 0);
      }
      if (distance != 0) {
        int start = robot->getEncoderA();
    // double nonstopping_distance = distance_conv - stopping_distance;
    // // cout << "NStopping: " << stopping_distance/LINEAR_CONVERSION << endl;
    // // cout << "Nonstopping: " << nonstopping_distance/LINEAR_CONVERSION << endl;
    // while (abs(robot->getEncoderA()-start) < nonstopping_distance) {
    //   ros::spinOnce();
    //   loop_rate.sleep();
          // if (commandOverride()) {
          //   return;
          // }
    //   if ((uint8_t)(robot->getBumper()) != 0) {
    //     stopped_early = true;
    //     ROS_ERROR("Bumper on. Stopped early. ");
    //     break;
    //   }
    //   if (distance > 0) {
    //     // move forward
    //     if ((uint8_t)(robot->getFrontSonar()) <= 5) {
    //       stopped_early = true;
    //       ROS_ERROR("Front sonar on. Stopped early. ");
    //       break;
    //     }
    //     setMotorSpeeds(0, -192);
    //   }
    //   else {
    //     // move backward
    //     if ((uint8_t)(robot->getRearSonar()) <= 5) {
    //       stopped_early = true;
    //       ROS_ERROR("Rear sonar on. Stopped early. ");
    //       break;
    //     }
    //     setMotorSpeeds(0, 192);
    //   }
    //   // int now = robot->getEncoderA();
    //   // cout << now << endl;
    // }
        while (abs(robot->getEncoderA()-start) < distance_conv) {
          ros::spinOnce();
          loop_rate.sleep();
          if (commandOverride()) {
            return;
          }
          if ((uint8_t)(robot->getBumper()) != 0) {
            stopped_early = true;
            ROS_ERROR("Bumper on. Stopped early. ");
            break;
          }
          if (distance > 0) {
        // move forward
            if ((uint8_t)(robot->getFrontSonar()) <= 5) {
              stopped_early = true;
              ROS_ERROR("Front sonar on. Stopped early. ");
              break;
            }
            setMotorSpeeds(0, -128);
          }
          else {
        // move backward
            if ((uint8_t)(robot->getRearSonar()) <= 5) {
              stopped_early = true;
              ROS_ERROR("Rear sonar on. Stopped early. ");
              break;
            }
            setMotorSpeeds(0, 128);
          }
      // int now = robot->getEncoderA();
      // cout << now << endl;
        }
        setMotorSpeeds(0, 0);
    // cout << "End: " << now << endl;
      }
      ROS_INFO("Command %i Finished. ", move_identity_);
    }
    msg_mutex.unlock();
  }
}

ros_rp2w::Packet createPacket() {
  ros_rp2w::Packet packet;
  packet.leftMotorSpeed = (uint8_t)(robot->getLeftMotorSpeed());
  packet.rightMotorSpeed = (uint8_t)(robot->getRightMotorSpeed());
  packet.cameraTilt = robot->getCameraTilt();
  packet.cameraPan = robot->getCameraPan();
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
  return packet;
}

int main(int argc, char **argv) {
  ros::init(argc, argv, "rp2w");
  ros::NodeHandle n;
  ros::Rate loop_rate(60);

  rc = robot->connect("/dev/ttyUSB0");
  rc = robot->update();
  if (rc != rp2w::OK) {
   ROS_ERROR("No RP2W robot.");
   return 1;
 }
 else {
   cout << "RP2W Initialized. " << endl;
 }

 ros::Publisher pub = n.advertise<ros_rp2w::Packet>("rp2w_packet", 10);
 ros::Subscriber sub = n.subscribe("rp2w/advanced_command", 1, command);
 initialize();

 while (ros::ok()) {
  update();
  move(loop_rate);
  pub.publish(createPacket());
  ros::spinOnce();
  loop_rate.sleep();
}

return 0;
}
