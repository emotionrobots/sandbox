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

#include <iostream>
#include <fstream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termio.h>
#include "rp2w_serial.h"

using namespace std;
// using namespace LibSerial;

int main(int argc, char *argv[])
{
    ofstream fout("encoder3.csv");
    rp2w::Status rc;
    rp2w robot;

    char digital1 = 0;
    char digital2 = 0x0c;
    int16_t right_speed = 120;
    char right_motor = 0;
 
    rc = robot.connect("/dev/ttyUSB0");
    if (rc != rp2w::OK) {
       cerr << "No R2PW robot" << endl;
       return 1;
    }

    robot.setGPIO1(digital1);
    robot.setGPIO2(digital2);
    robot.setLeftMotorSpeed(right_motor);
    robot.setRightMotorSpeed(right_motor);

    while (true) {

       rc = robot.update();
       if (rc != rp2w::OK)
           cerr << "robot.update failed (" << rc << ")" << endl;

       // right_motor = abs(right_speed);
       // if (right_speed >= 0) 
       //    digital1 &= ~(0x40);
       // else
       //    digital1 |= 0x40;

       // robot.setGPIO1(digital1);
       // robot.setRightMotorSpeed(right_motor);

       int now = robot.getEncoderA();
       cout << now << endl;
       fout << now << endl;
    } // end while
    return 0;
}

#undef __TESTAPP_CPP__
/*! @} */

