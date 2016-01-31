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


int main(int argc, char *argv[])
{
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

    while (true) {

        if (read_joystick_event(&jse)) {

          // printf("Joystick type=%2d num=%2d\n", jse.type, jse.number);
          printf("Joystick type=%2d num=%2d value =%2d\n", jse.type, jse.number, jse.value);

        } // end if
       usleep(1000);

    } // end while
    return 0;
}

#undef __TESTAPP_CPP__
/*! @} */

