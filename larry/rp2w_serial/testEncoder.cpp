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
    ofstream fout("encoder5.csv");
    rp2w::Status rc;
    rp2w robot;

    char digital1 = 0;
    int16_t right_speed = 120;
    char right_motor = 0;
 
    rc = robot.connect("/dev/ttyUSB0");
    if (rc != rp2w::OK) {
       cerr << "No R2PW robot" << endl;
       return 1;
    }

    robot.setGPIO1(digital1);
    robot.setLeftMotorSpeed(0);
    robot.setRightMotorSpeed(0);

    int last = -1, peak = -1;
    while (true) {

       rc = robot.update();
       if (rc != rp2w::OK)
           cerr << "robot.update failed (" << rc << ")" << endl;

       right_motor = abs(right_speed);
       if (right_speed >= 0) 
          digital1 &= ~(0x40);
       else
          digital1 |= 0x40;

       robot.setGPIO1(digital1);
       robot.setRightMotorSpeed(right_motor);
       // robot.setRightMotorSpeed(right_motor);

       int now = robot.getEncoderA();
       cout << now << endl;
       fout << now << endl;
       // if (now > last) {
       //    if (peak != -1) {
       //      cout << peak-last << endl;
       //    }
       //    peak = last;
       //  }
       //  last = now;

    } // end while
    return 0;
}

#undef __TESTAPP_CPP__
/*! @} */

