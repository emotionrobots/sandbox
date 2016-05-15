/*!
 *=============================================================================
 *
 *  @file	rp2w_serial.h
 *
 *  @brief	RP2W controller API (to the Arduino Mega 2560)
 *	
 *=============================================================================
 */
#ifndef __RP2W_SERIAL_H__
#define __RP2W_SERIAL_H__

#include <SerialStream.h>
#include <iostream>
#include <sstream>
#include <unistd.h>
#include <cstdlib>
#include <string>

using namespace std;
using namespace LibSerial;

#define BYTE 	unsigned char

typedef struct {
     BYTE startFlag;
     BYTE rightMotorSpeed;
     BYTE leftMotorSpeed;
     BYTE cameraTiltHigh;
     BYTE cameraTiltLow;
     BYTE cameraPanHigh;
     BYTE cameraPanLow;
     BYTE digital1;
     BYTE digital2;
     BYTE checksumHigh;
     BYTE checksumLow;
} CommandPacket;
 
typedef struct {
     BYTE startFlag1;
     BYTE startFlag2;
     BYTE encoderA_1;
     BYTE encoderA_2;
     BYTE encoderA_3;
     BYTE encoderA_4;
     BYTE encoderB_1;
     BYTE encoderB_2;
     BYTE encoderB_3;
     BYTE encoderB_4;
     BYTE batteryVoltage;
     BYTE frontSonar;
     BYTE rearSonar;
     BYTE bumper;
     BYTE endFlag1;
     BYTE endFlag2;
     BYTE endFlag3;
} SensorPacket;

class rp2w
{
public:
   typedef enum _rp2w { 
      OK = 0,
      COMM_UNKNOWN_ERROR,
      COMM_OPEN_ERROR,
      COMM_CLOSE__ERROR,
      COMM_SETBAUDRATE_ERROR,
      COMM_SETCHARSIZE_ERROR,
      COMM_SETPARITY_ERROR,
      COMM_SETFLOWCONTROL_ERROR,
      COMM_SETSTOPBITS_ERROR,
      COMM_TIMEOUT_ERROR,
      COMM_WRITE_ERROR,
   } Status;

   rp2w();
   ~rp2w();
   Status connect(string port);  
   Status rxData(char *data, int &length, int timeout);
   Status txData(char *data, int length);

   Status update();

   void setLeftMotorSpeed(char speed);
   void setRightMotorSpeed(char speed);
   void setCameraPan(int16_t pan);
   void setCameraTilt(int16_t tilt);
   void setGPIO1(char bits);
   void setGPIO2(char bits);
   char getLeftMotorSpeed(); 
   char getRightMotorSpeed();
   int16_t getCameraPan();
   int16_t getCameraTilt();
   char getGPIO1();
   char getGPIO2();

   int getEncoderA();
   int getEncoderB();
   char getBatteryVoltage();
   char getFrontSonar();
   char getRearSonar();
   char getBumper();

private:
   // Comm
   SerialStream mSerialPort;
   int mCommTimeOut;

   // Commands to robot
   char rightMotorSpeed;
   char leftMotorSpeed;
   int16_t cameraTilt;
   int16_t cameraPan;
   char digital1;
   char digital2;

   // Sensors from robot
   char encoderA_1;
   char encoderA_2;
   char encoderA_3;
   char encoderA_4;
   char encoderB_1;
   char encoderB_2;
   char encoderB_3;
   char encoderB_4;
   char batteryVoltage;
   char frontSonar;
   char rearSonar;
   char bumper;
};

#endif  // __RP2W_SERIAL_H__
/*! @} */
