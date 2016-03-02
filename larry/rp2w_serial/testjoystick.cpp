/*!
 *=============================================================================
 *
 *  @file   testapp.cpp
 *
 *  @brief  RP2W controller test application 
 *  
 *=============================================================================
 */

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
            else if (servo_pwr == 1 && jse.number == JS_JOYSTICK_PAN) 
              pan_speed = (int16_t)(jse.value >> 12);
            else if (servo_pwr == 1 && jse.number == JS_JOYSTICK_TILT) 
              tilt_speed = (int16_t)(jse.value >> 12);
          }

          left_speed = trav_speed + turn_speed;
          right_speed = trav_speed - turn_speed;

          cout << "Left Speed: " << left_speed << "; Right Speed: " << right_speed;
          printf("Left Motor: %u; Right Motor: %u", 
            (unsigned char)(left_speed), (unsigned char)(right_speed));

        } // end if
       usleep(1000);

    } // end while
    return 0;
}

