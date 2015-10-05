/*!
 *=============================================================================
 *
 *  @file	rp2w_serial.cpp
 *
 *  @brief	RP2W controller API (to the Arduino Mega 2560)
 *	
 *=============================================================================
 */
#define __RP2W_SERIAL_CPP__

#include <stdio.h>
#include "rp2w_serial.h"


/*!
 *=============================================================================
 *  
 *  @function		rp2w::Status rp2w::connect(string port)
 *  
 *  @brief	 	Connect RP2W to a serial port	
 *
 *  @return		error status
 * 
 *=============================================================================
 */
rp2w::rp2w()
{
   mCommTimeOut = 1000;
}

/*!
 *=============================================================================
 *  
 *  @function		rp2w::Status rp2w::connect(string port)
 *  
 *  @brief	 	Connect RP2W to a serial port	
 *
 *  @return		error status
 * 
 *=============================================================================
 */
rp2w::~rp2w()
{
   mSerialPort.Close();
}


/*!
 *=============================================================================
 *  
 *  @function		rp2w::Status rp2w::connect(string port)
 *  
 *  @brief	 	Connect RP2W to a serial port	
 *
 *  @return		error status
 * 
 *=============================================================================
 */
rp2w::Status rp2w::connect(string port)
{
   system("stty -F /dev/ttyUSB0 cs8 115200 ignbrk -brkint -icrnl -imaxbel \
            -opost -onlcr -isig -icanon -iexten -echo -echoe -echok -echoctl \
            -echoke noflsh -ixon -crtscts");

   mSerialPort.Open(port) ;
   if ( !mSerialPort.IsOpen() ) 
      return rp2w::COMM_OPEN_ERROR;
   mSerialPort.SetBaudRate( SerialStreamBuf::BAUD_115200 ) ;
   if ( !mSerialPort.good() ) 
      return rp2w::COMM_SETBAUDRATE_ERROR;
   mSerialPort.SetCharSize( SerialStreamBuf::CHAR_SIZE_8 ) ;
   if ( ! mSerialPort.good() )
      return rp2w::COMM_SETCHARSIZE_ERROR;
   mSerialPort.SetParity( SerialStreamBuf::PARITY_NONE ) ;
   if ( ! mSerialPort.good() )
      return rp2w::COMM_SETPARITY_ERROR;
   mSerialPort.SetNumOfStopBits( 1 ) ;
   if ( ! mSerialPort.good() ) 
      return rp2w::COMM_SETSTOPBITS_ERROR;
   mSerialPort.SetFlowControl( SerialStreamBuf::FLOW_CONTROL_NONE ) ;
   if ( ! mSerialPort.good() ) 
      return rp2w::COMM_SETFLOWCONTROL_ERROR;

   return rp2w::OK;
}


/*!
 *=============================================================================
 *  
 *  @function		rp2w::Status rp2w::rxData(char *data, int &length) 
 *
 *  @brief	 	Send a byte array of data	
 * 
 *  @return		error status
 *
 *=============================================================================
 */
rp2w::Status rp2w::rxData(char *data, int &length, int timeout)
{
   int to = timeout;
   int count = 0;
   char *p = data;

   while (count < length && timeout--) {
      if (mSerialPort.rdbuf()->in_avail() > 0) {
         mSerialPort.get(*p++);
         timeout = to;
         count++;
      } 
      else {
         usleep(100);
      }
   }

   if (timeout <= 0) {
	return rp2w::COMM_TIMEOUT_ERROR;
   }
   else {
	length = count;
   	return rp2w::OK;
   }
}


/*!
 *=============================================================================
 *  
 *  @function		rp2w::Status rp2w::txData(char *data, int length)
 *  
 *  @brief	 	Send a byte array of data	
 *
 *  @return		error status
 * 
 *=============================================================================
 */
rp2w::Status rp2w::txData(char *data, int length)
{
     mSerialPort.write(data, length);  
     if ( !mSerialPort.good() ) 
	return rp2w::COMM_WRITE_ERROR;
     else
        return rp2w::OK;
}


/*!
 *=============================================================================
 *  
 *  @function		rp2w::Status rp2w::update()
 *  
 *  @brief	 	Synchronize data with robot	
 *
 *  @return		error status
 * 
 *=============================================================================
 */
rp2w::Status rp2w::update()
{
   rp2w::Status rc;
   int checksum;
   CommandPacket txPacket;
   SensorPacket rxPacket;

   txPacket.startFlag = 'S';
   txPacket.rightMotorSpeed = rightMotorSpeed;
   txPacket.leftMotorSpeed = leftMotorSpeed;
   txPacket.cameraTiltHigh = ((cameraTilt >> 8) & 0xff);
   txPacket.cameraTiltLow = (cameraTilt & 0xff);
   txPacket.cameraPanHigh = ((cameraPan >> 8) & 0xff);
   txPacket.cameraPanLow = (cameraPan & 0xff);
   txPacket.digital1 = digital1;
   txPacket.digital2 = digital2;

 #if 0 
   printf("rightMotorSpeed = %4d\n", txPacket.rightMotorSpeed);
   printf("leftMotorSpeed  = %4d\n", txPacket.leftMotorSpeed);
   printf("cameraTiltHigh  = %4d\n", txPacket.cameraTiltHigh);
   printf("cameraTiltLow   = %4d\n", txPacket.cameraTiltLow);
   printf("cameraPanHigh   = %4d\n", txPacket.cameraPanHigh);
   printf("cameraPanLow    = %4d\n", txPacket.cameraPanLow);
   printf("digital1        = %4d\n", txPacket.digital1);
   printf("digital2        = %4d\n", txPacket.digital2);
   printf("startFlag       = %4d\n", txPacket.startFlag);
#endif
 
   checksum = txPacket.startFlag
            + txPacket.rightMotorSpeed 
            + txPacket.leftMotorSpeed
            + txPacket.cameraTiltHigh
            + txPacket.cameraTiltLow
            + txPacket.cameraPanHigh
            + txPacket.cameraPanLow
            + txPacket.digital1
            + txPacket.digital2;

   txPacket.checksumHigh = checksum >> 8;
   txPacket.checksumLow = checksum & 0xff;

#if 0
   printf("checksum     = %4d\n", checksum);
   printf("checksumHigh = %2d\n", txPacket.checksumHigh);
   printf("checksumLow  = %2d\n", txPacket.checksumLow);
#endif

   rc = txData((char *)&txPacket, sizeof(txPacket));
   if (rc != rp2w::OK) return rc;

   int length = sizeof(rxPacket);
   rc = rxData((char *)&rxPacket, length, 200);
   if (rc != rp2w::OK) 
      return rc;

   if (rxPacket.startFlag1 == 'S' & rxPacket.startFlag2 == '1'
       && rxPacket.endFlag1 == 'E' && rxPacket.endFlag2 == 'O' 
       && rxPacket.endFlag3 == 'F') {
      encoderA_1 = rxPacket.encoderA_1;
      encoderA_2 = rxPacket.encoderA_2;
      encoderA_3 = rxPacket.encoderA_3;
      encoderA_4 = rxPacket.encoderA_4;
      encoderB_1 = rxPacket.encoderB_1;
      encoderB_2 = rxPacket.encoderB_2;
      encoderB_3 = rxPacket.encoderB_3;
      encoderB_4 = rxPacket.encoderB_4;
      batteryVoltage = rxPacket.batteryVoltage;
      frontSonar = rxPacket.frontSonar;
      rearSonar = rxPacket.rearSonar;
      bumper = rxPacket.bumper;
      return rp2w::OK;
   }
   else {
      return rp2w::COMM_UNKNOWN_ERROR;
   }
}

/*!
 *=============================================================================
 *  
 *  @function		void rp2w::setLeftMotorSpeed(char speed)
 *  
 *  @brief	 	Set robot's left motor speed
 *
 *=============================================================================
 */
void rp2w::setLeftMotorSpeed(char speed)
{
    leftMotorSpeed = speed;
}

/*!
 *=============================================================================
 *  
 *  @function		void rp2w::setRightMotorSpeed(char speed)
 *  
 *  @brief	 	Set robot's right motor speed
 *
 *=============================================================================
 */
void rp2w::setRightMotorSpeed(char speed)
{
    rightMotorSpeed = speed;
}

/*!
 *=============================================================================
 *  
 *  @function		void rp2w::setCameraTilt(int16_t tilt)
 *  
 *  @brief	 	Set camera tilt position	
 *
 *=============================================================================
 */
void rp2w::setCameraTilt(int16_t tilt)
{
    cameraTilt = tilt;
}

/*!
 *=============================================================================
 *  
 *  @function		void rp2w::setCameraPan(int16_t pan)
 *  
 *  @brief	 	Set camera pan position	
 *
 *=============================================================================
 */
void rp2w::setCameraPan(int16_t pan)
{
    cameraPan = pan;
}

/*!
 *=============================================================================
 *  
 *  @function		void rp2w::setGPIO1(char bits)
 *  
 *  @brief	 	Set GPIO bank 1	
 *
 *=============================================================================
 */
void rp2w::setGPIO1(char bits)
{
    digital1 = bits;
}

/*!
 *=============================================================================
 *  
 *  @function		void rp2w::setGPIO2(char bits)
 *  
 *  @brief	 	Set GPIO bank 2
 *
 *=============================================================================
 */
void rp2w::setGPIO2(char bits)
{
    digital2 = bits;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getLeftMotorSpeed()
 *  
 *  @brief	 	Get left motor speed (from last command)	
 *
 *=============================================================================
 */
char rp2w::getLeftMotorSpeed()
{
   return leftMotorSpeed;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getRightMotorSpeed()
 *  
 *  @brief	 	Get right motor speed (from last command)	
 *
 *=============================================================================
 */
char rp2w::getRightMotorSpeed()
{
   return rightMotorSpeed;
}

/*!
 *=============================================================================
 *  
 *  @function		int16_t rp2w::getCameraPan()
 *  
 *  @brief	 	Get camera pan (from last command)	
 *
 *=============================================================================
 */
int16_t rp2w::getCameraPan()
{
   return cameraPan;
}

/*!
 *=============================================================================
 *  
 *  @function		int16_t rp2w::getCameraTilt()
 *  
 *  @brief	 	Get camera tilt (from last command)
 *
 *=============================================================================
 */
int16_t rp2w::getCameraTilt()
{
   return cameraTilt;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getGPIO1()
 *  
 *  @brief	 	Get GPIO bank 1 (from last command)
 * 
 *=============================================================================
 */
char rp2w::getGPIO1()
{
   return digital1;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getGPIO2()
 *  
 *  @brief	 	Get GPIO bank 2 (from last command)	
 *
 *=============================================================================
 */
char rp2w::getGPIO2()
{
   return digital2;
}

/*!
 *=============================================================================
 *  
 *  @function		int rp2w::getEncoderA()
 *  
 *  @brief	 	Get encoder A	
 * 
 *=============================================================================
 */
int rp2w::getEncoderA()
{
   int val = encoderA_1;
   val <<= 24; 
   val += encoderA_2 << 16; 
   val += encoderA_3 << 8; 
   val += encoderA_4; 
   return val;
}

/*!
 *=============================================================================
 *  
 *  @function		int rp2w::getEncoderB()
 *  
 *  @brief	 	Get encoder B
 * 
 *=============================================================================
 */
int rp2w::getEncoderB()
{
   int val = encoderB_1;
   val <<= 24;
   val += encoderB_2 << 16;
   val += encoderB_3 <<  8;
   val += encoderB_4;
   return val;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getBatteryVoltage()
 *  
 *  @brief	 	Get battery voltage	
 *
 *=============================================================================
 */
char rp2w::getBatteryVoltage()
{
   return batteryVoltage;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getFrontSonar()
 *  
 *  @brief	 	Get front sonar
 *
 *=============================================================================
 */
char rp2w::getFrontSonar()
{
   return frontSonar;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getRearSonar()
 *  
 *  @brief	 	Get rear sonar	
 *
 *=============================================================================
 */
char rp2w::getRearSonar()
{
   return rearSonar;
}

/*!
 *=============================================================================
 *  
 *  @function		char rp2w::getBumper()
 *  
 *  @brief	 	Get bumper	
 *
 *=============================================================================
 */
char rp2w::getBumper()
{
   return bumper;
}

#undef __RP2W_SERIAL_CPP__
/*! @} */

