/*!
 *=============================================================================
 *
 *  @file	testapp.cpp
 *
 *  @brief	RP2W controller test application 
 *	
 *=============================================================================
 */
#define __TESTAPP__CPP__

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termio.h>
#include "joystick.h"
#include "rp2w_serial.h"

using namespace std;
using namespace LibSerial;

CommandPacket command = {'S',0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
SensorPacket sensor;

int getkey() {
    int character;
    struct termios orig_term_attr;
    struct termios new_term_attr;

    /* set the terminal to raw mode */
    tcgetattr(fileno(stdin), &orig_term_attr);
    memcpy(&new_term_attr, &orig_term_attr, sizeof(struct termios));
    new_term_attr.c_lflag &= ~(ECHO|ICANON);
    new_term_attr.c_cc[VTIME] = 0;
    new_term_attr.c_cc[VMIN] = 0;
    tcsetattr(fileno(stdin), TCSANOW, &new_term_attr);

    /* read a character from the stdin stream without blocking */
    /*   returns EOF (-1) if no character is available */
    character = fgetc(stdin);

    /* restore the original terminal attributes */
    tcsetattr(fileno(stdin), TCSANOW, &orig_term_attr);

    return character;
}


#define JS_TYPE_BUTTON 		1
#define JS_TYPE_JOYSTICK	2

#define JS_BUTTON_LIGHT         0
#define JS_BUTTON_MOTOR_PWR	8
#define JS_BUTTON_SERVO_RESET	9

#define JS_JOYSTICK_PAN		0
#define JS_JOYSTICK_TILT	1
#define JS_JOYSTICK_TURN	2
#define JS_JOYSTICK_TRAV	3

#define PAN_INIT		1500
#define TILT_INIT	 	1560	

int main(int argc, char *argv[])
{
    char *p;
    char key;
    int length;
    rp2w::Status rc;
    rp2w robot;
    bool done = false;
    char dir = 0;
    int16_t pan_pos = PAN_INIT;
    int16_t tilt_pos = TILT_INIT;
    int16_t pan_speed = 0;
    int16_t tilt_speed = 0;
    int16_t turn_speed = 0;
    int16_t trav_speed = 0;
    char servo_pwr = 0; 
    char servo_reset = 0; 
    char light_on = 0;
    char digital1 = 0;
    char left_motor = 0;
    char right_motor = 0;
    int16_t left_speed = 0;
    int16_t right_speed = 0;
  
    struct js_event jse;
    int fd;
  
    if (argc != 2) {
       cout << "Usage: testapp <joystick>" << endl;
       exit (-1);
    }
 
    fd = open_joystick(argv[1]);
    if (fd < 0) {
       cerr << "No joystick at " << argv[1] << endl;
       return 1;
    }
 
    rc = robot.connect("/dev/ttyUSB0");
    if (rc != rp2w::OK) {
       cerr << "No R2PW robot" << endl;
       return 1;
    }

    robot.setGPIO1(digital1);
    robot.setLeftMotorSpeed(left_motor);
    robot.setRightMotorSpeed(right_motor);
    robot.setCameraPan(pan_pos);
    robot.setCameraTilt(tilt_pos);

    while (!done) {

       rc = robot.update();
       if (rc != rp2w::OK)
           cerr << "robot.update failed" << endl;

        if (read_joystick_event(&jse)) {

          switch (jse.type) {
             case JS_TYPE_BUTTON:
                switch (jse.number) {
                   case JS_BUTTON_LIGHT:
                      if (light_on == 0 && jse.value == 1) { 
                         digital1 ^= 0x04;
                         robot.setGPIO1(digital1); 
                      }
                      light_on = jse.value; 
                      break;   
                   case JS_BUTTON_MOTOR_PWR:
                      if (servo_pwr == 0 && jse.value == 1) { 
                         digital1 ^= 0x03;
                         robot.setGPIO1(digital1); 
                      }
                      servo_pwr = jse.value; 
                      break; 
                   case JS_BUTTON_SERVO_RESET:
                      if (servo_reset == 0 && jse.value == 1) { 
                         pan_pos = PAN_INIT;
                         tilt_pos = TILT_INIT;
                         pan_speed = 0;
                         tilt_speed = 0;
                      }
                      servo_reset = jse.value; 
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

       } // end if

       left_speed = trav_speed + turn_speed;
       right_speed = trav_speed - turn_speed;
       left_motor = abs(left_speed);
       right_motor = abs(right_speed);
       if (left_speed >= 0) 
          digital1 &= ~(0x80);
       else
          digital1 |= 0x80;
       if (right_speed >= 0) 
          digital1 &= ~(0x40);
       else
          digital1 |= 0x40;

       robot.setGPIO1(digital1);
       robot.setLeftMotorSpeed(left_motor);
       robot.setRightMotorSpeed(right_motor);

       pan_pos -= pan_speed;
       tilt_pos -= tilt_speed;
       robot.setCameraPan(pan_pos);
       robot.setCameraTilt(tilt_pos);
 
       printf("DIG=0x%02X LM=%04d RM=%04d PAN=%04d TILT=%04d\n",
              digital1, left_motor,  right_motor, 
              pan_pos, tilt_pos);

       usleep(1000);

    } // end while
    return 0;
}

#undef __TESTAPP_CPP__
/*! @} */

