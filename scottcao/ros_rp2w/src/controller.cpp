#include "ros/ros.h"
#include "ros_rp2w/Packet.h"
#include "ros_rp2w/Command.h"
#include <boost/thread/mutex.hpp>
#include <cstdlib>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termio.h>
#include "joystick.h"

using namespace std;

#define JS_TYPE_BUTTON    1
#define JS_TYPE_JOYSTICK  2

#define JS_BUTTON_LIGHT         0
#define JS_BUTTON_SERVO_RESET 2
#define JS_BUTTON_MOTOR_PWR 8
#define JS_BUTTON_SERVO_HOME  9

#define JS_JOYSTICK_PAN   0
#define JS_JOYSTICK_TILT  1
#define JS_JOYSTICK_TURN  2
#define JS_JOYSTICK_TRAV  3

boost::mutex mutex;
ros_rp2w::Packet::ConstPtr packet;

void packetCallback(const ros_rp2w::Packet::ConstPtr& msg) {
  boost::mutex::scoped_lock lock(mutex);
  packet = msg;
}

int main(int argc, char **argv) {
  ros::init(argc, argv, "rp2w_controller");

  if (argc != 2) {
    ROS_INFO("Usage: testapp <joystick>");
    return 1;
  }
  int fd = open_joystick(argv[1]);
  if (fd < 0) {
    ROS_ERROR("No joystick at %s", argv[1]);
    return 1;
  }

  ros::NodeHandle n;
  ros::Subscriber sub = n.subscribe("rp2w_packet", 1000, packetCallback);
  ros::ServiceClient client = n.serviceClient<ros_rp2w::Command>("rp2w_command");

  int16_t pan_home = 1500;
  int16_t tilt_home = 1560;
  int16_t pan_pos = pan_home;
  int16_t tilt_pos = tilt_home;
  int16_t pan_speed = 0;
  int16_t tilt_speed = 0;
  int16_t turn_speed = 0;
  int16_t trav_speed = 0;
  char servo_pwr = 0; 
  char servo_reset = 0; 
  bool light_on = false;
  bool light_button = false;
  int16_t left_speed = 0;
  int16_t right_speed = 0;
  int16_t l_speed = 0;
  int16_t r_speed = 0;
  struct js_event jse;
  
  while (ros::ok) {  
    ros_rp2w::Command srv;
    if (mutex.try_lock()) {
      if (packet) {
        srv.request.digital1 = packet->digital1;
      }
      else {
        srv.request.digital1 = 0;
      }
      mutex.unlock();
    }

    if (read_joystick_event(&jse)) {
      ROS_INFO("Joystick type=%2d num=%2d", jse.type, jse.number);

      switch (jse.type) {

        case JS_TYPE_BUTTON:
        switch (jse.number) {

          case JS_BUTTON_LIGHT:
            // if (light_on == 0 && jse.value == 1) { 
            //   srv.request.digital1 ^= 0x04; 
            // }
            if (!light_button) {
              light_on = !light_on; 
            }
            light_button = !light_button;
          break;   

          case JS_BUTTON_MOTOR_PWR:
            if (servo_pwr == 0 && jse.value == 1) { 
              srv.request.digital1 ^= 0x03;
            }
            servo_pwr = jse.value; 
          break; 

          case JS_BUTTON_SERVO_HOME:
            if (servo_reset == 0 && jse.value == 1) { 
              pan_pos = pan_home;
              tilt_pos = tilt_home;
              pan_speed = 0;
              tilt_speed = 0;
            }
            servo_reset = jse.value; 
          break; 

          case JS_BUTTON_SERVO_RESET:
            if (jse.value == 1) { 
              pan_home = pan_pos; 
              tilt_home = tilt_pos; 
            }
          break;

          default:
          break;
        } // end switch; 
        break;

        case JS_TYPE_JOYSTICK: 
          if (jse.number == JS_JOYSTICK_TRAV) 
            trav_speed = (int16_t)(jse.value >> 8);
          else if (jse.number == JS_JOYSTICK_TURN)
            turn_speed = (int16_t)(jse.value >> 8);
          else if (jse.number == JS_JOYSTICK_PAN) 
            pan_speed = (int16_t)(jse.value >> 12);
          else if (jse.number == JS_JOYSTICK_TILT) 
            tilt_speed = (int16_t)(jse.value >> 12);
        break;

        default:
        break;
      }; // end switch

      l_speed = trav_speed + turn_speed;
      r_speed = trav_speed - turn_speed;
      if (l_speed >= 0) {
        srv.request.digital1 &= ~(0x80);
      }
      else {
        srv.request.digital1 |= 0x80;
      }
      if (r_speed >= 0) {
        srv.request.digital1 &= ~(0x40);
      }
      else {
        srv.request.digital1 |= 0x40;
      }
      if (l_speed != left_speed) {
        left_speed = l_speed;
        srv.request.leftMotorSpeedCommand = true;
        srv.request.leftMotorSpeed = abs(left_speed);
      }
      if (r_speed != right_speed) {
        right_speed = r_speed;
        srv.request.rightMotorSpeedCommand = true;
        srv.request.rightMotorSpeed = abs(right_speed);
      }
      // cout << left_speed << " " << right_speed;
      if (pan_speed != 0) {
        pan_pos -= pan_speed;
        srv.request.cameraPanCommand = true;
        srv.request.cameraPan = pan_pos;
      }
      if (tilt_speed != 0) {
        tilt_pos -= tilt_speed;
        srv.request.cameraTiltCommand = true;
        srv.request.cameraTilt = tilt_pos;
      }

      if (light_on) {
        srv.request.digital1 ^= 0x04; 
      }
      srv.request.digital1Command = true;
    }

    if (client.call(srv)) {
    }
    else {
      ROS_ERROR("Failed to call service rp2w_command");
      return 1;
    }
  }

  return 0;
}