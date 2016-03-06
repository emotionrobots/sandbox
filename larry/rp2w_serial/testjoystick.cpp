/*!
 *=============================================================================
 *
 *  @file	testapp.cpp
 *
 *  @brief	RP2W controller test application 
 *	
 *=============================================================================
 */

#include <iostream>
#include <cstdlib>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termio.h>
#include "joystick.h"

#define JS_TYPE_BUTTON    1
#define JS_TYPE_JOYSTICK  2

#define JS_BUTTON_LIGHT         0
#define JS_BUTTON_SERVO_RESET 2
#define JS_BUTTON_MOTOR_PWR 8
#define JS_BUTTON_SERVO_HOME  9

#define JS_JOYSTICK_PAN   1
#define JS_JOYSTICK_TILT  0
#define JS_JOYSTICK_TURN  2
#define JS_JOYSTICK_TRAV  3

using namespace std;

int main(int argc, char *argv[])
{
    struct js_event jse;
    int fd;
    int16_t pan_speed = 0;
    int16_t tilt_speed = 0;
    int16_t turn_speed = 0;
    int16_t trav_speed = 0;
    int16_t left_speed = 0;
    int16_t right_speed = 0;
  
    if (argc != 2) {
       cout << "Usage: testapp <joystick>" << endl;
       exit (-1);
    }
 
    fd = open_joystick(argv[1]);
    if (fd < 0) {
       cerr << "No joystick at " << argv[1] << endl;
       return 1;
    }

    while (true) {

        if (read_joystick_event(&jse)) {

          // printf("Joystick type=%2d num=%2d\n", jse.type, jse.number);
          printf("Joystick type=%2d num=%2d value =%2d\n", jse.type, jse.number, jse.value);
          if (jse.type == JS_TYPE_JOYSTICK) { 
            if (jse.number == JS_JOYSTICK_TRAV) 
              trav_speed = (int16_t)(jse.value >> 8);
            else if (jse.number == JS_JOYSTICK_TURN)
              turn_speed = (int16_t)(jse.value >> 8);
            else if (jse.number == JS_JOYSTICK_PAN) 
              pan_speed = (int16_t)(jse.value >> 12);
            else if (jse.number == JS_JOYSTICK_TILT) 
              tilt_speed = (int16_t)(jse.value >> 12);
          }

          left_speed = trav_speed + turn_speed;
          right_speed = trav_speed - turn_speed;

          cout << "Left Speed: " << left_speed << "; Right Speed: " << right_speed << endl;
          char x = abs(left_speed), y = abs(right_speed);
          printf("Left Motor: %u; Right Motor: %u\n", 
            (unsigned char)(x), (unsigned char)(y));

        } // end if

    } // end while
    return 0;
}

