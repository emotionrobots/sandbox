//=========================HEADER=============================================================
/*

   42444-RP2W
   AUTHOR: Jason Traud
   DATE: December 12, 2014 
   
   This firmware will enable control of RP2W platform                              
                   
   
   Hardware: Arduino MEGA 2560 R3 MCU-049-000
             SDR Custom Arduino Mega Shield MCU-055-000
             PWM Motor Controller (TE-058-000)
             
  		
   License: CCAv3.0 Attribution-ShareAlike (http://creativecommons.org/licenses/by-sa/3.0/)
   You're free to use this code for any venture. Attribution is greatly appreciated. 

//============================================================================================
*/

// Incoming Packet: PC to Robot
//  - 0 0x53 "S" Start Byte
//  - 1 Right Motor Speed
//  - 2 Left Motor Speed
//  - 3 Cmaera Tilt High Byte
//  - 4 Camera Tilt Low Byte
//  - 5 Camera Pan High Byte
//  - 6 Camera Pan Low Byte
//  - 7 Digital 1
//  -    0 Relay 1
//  -    1 Relay 2
//  -    2 Relay 3
//  -    3 Relay 4
//  -    4 (Not implemented)
//  -    5 (Not implemented)
//  -    6 Right Motor PWM Direction
//  -    7 Left Motor PWM Direction
//  - 8 Digital 2
//  -    0 Clear Encoder Count
//  -    1 (Not implemented)
//  -    2 Override Front Sonar
//  -    3 Override Rear Sonar
//  -    4 Override Bumper
//  -    5 (Not implemented)
//  -    6 (Not implemented)
//  -    7 (Not implemented)


// ****************************************************
// Libraries
// ****************************************************
#include <SPI.h> // SPI COM is used to communicate to the encoder buffer
#include <Servo.h>


// ****************************************************
// Hardware Pin Defines
// ****************************************************

// Motor Control Pins
#define DIRECTION_RIGHT 2
#define PWM_RIGHT 3
#define DIRECTION_LEFT 4
#define PWM_LEFT 5

// Inputs
#define BATTERY_VOLTAGE_PIN A0
#define FRONT_SONAR_PIN A2
#define REAR_SONAR_PIN A3
#define BUMPER_PIN A4

// Outputs
#define RELAY_1_PIN 27
#define RELAY_2_PIN 29
#define RELAY_3_PIN 31
#define RELAY_4_PIN 33

#define ENC_SS1_PIN 47
#define ENC_SS2_PIN 49

// ****************************************************
// Constants
// ****************************************************
#define REAR_SONAR_THRESHOLD 20
#define FRONT_SONAR_THRESHOLD 20

// ****************************************************
// Servo Defines
// ****************************************************
Servo tiltServo;
Servo panServo;

// ****************************************************
// Networking RAM
// ****************************************************
bool sFound = false;		
byte Bad_Tx;
word No_Tx = 0;

// ****************************************************
// Recieved Packet Structure
// ****************************************************
typedef struct {
  byte startFlag;
  byte rightMotorSpeed;
  byte leftMotorSpeed;
  byte cameraTiltHigh;
  byte cameraTiltLow;
  byte cameraPanHigh;
  byte cameraPanLow;
  byte digital1;
  byte digital2;
  byte checksumHigh;
  byte checksumLow;  
} packetToRobot;

packetToRobot rxPacket;

// ****************************************************
// Transmitted Packet Structure
// ****************************************************
typedef struct {
  byte startFlag1;
  byte startFlag2;
  byte encoderA_1;
  byte encoderA_2;
  byte encoderA_3;
  byte encoderA_4;
  byte encoderB_1;
  byte encoderB_2;
  byte encoderB_3;
  byte encoderB_4;
  byte batteryVoltage;
  byte frontSonar;
  byte rearSonar;
  byte bumper;
  byte endFlag1;
  byte endFlag2;
  byte endFlag3;
} packetToPC;

packetToPC txPacket;

// ****************************************************
// Encoder initialization
// RETURNS: none
// ****************************************************
void initEncoders() {
  
  // Set slave selects as outputs
  pinMode(ENC_SS1_PIN, OUTPUT);
  pinMode(ENC_SS2_PIN, OUTPUT);
  
  // Raise select pins
  // Communication begins when you drop the individual select signsl
  digitalWrite(ENC_SS1_PIN,HIGH);
  digitalWrite(ENC_SS2_PIN,HIGH);
  
  SPI.begin();
  
  // Initialize encoder 1
  //    Clock division factor: 0
  //    Negative index input
  //    free-running count mode
  //    x4 quatrature count mode (four counts per quadrature cycle)
  // NOTE: For more information on commands, see datasheet
  digitalWrite(ENC_SS1_PIN,LOW);        // Begin SPI conversation
  SPI.transfer(0x88);                   // Write to MDR0
  SPI.transfer(0x03);                   // Configure to 4 byte mode
  digitalWrite(ENC_SS1_PIN,HIGH);       // Terminate SPI conversation 

  // Initialize encoder 2
  //    Clock division factor: 0
  //    Negative index input
  //    free-running count mode
  //    x4 quatrature count mode (four counts per quadrature cycle)
  // NOTE: For more information on commands, see datasheet
  digitalWrite(ENC_SS2_PIN,LOW);        // Begin SPI conversation
  SPI.transfer(0x88);                   // Write to MDR0
  SPI.transfer(0x03);                   // Configure to 4 byte mode
  digitalWrite(ENC_SS2_PIN,HIGH);       // Terminate SPI conversation 
}

// ****************************************************
// Reads the current encoder count and updates PC packet
// RETURNS: none
// ****************************************************
void readEncoders() {
  
  // Read encoder 1
    digitalWrite(ENC_SS1_PIN,LOW);      // Begin SPI conversation
    SPI.transfer(0x60);                 // Request count
    txPacket.encoderA_1 = SPI.transfer(0x00);       // Read highest order byte
    txPacket.encoderA_2 = SPI.transfer(0x00);           
    txPacket.encoderA_3 = SPI.transfer(0x00);           
    txPacket.encoderA_4 = SPI.transfer(0x00);       // Read lowest order byte
    Serial.println((int)(txPacket.encoderA_1));
//    Serial.println("A2: "+txPacket.encoderA_2);           
//    Serial.println("A3: "+txPacket.encoderA_3);           
//    Serial.println("A4: "+txPacket.encoderA_4);     
    digitalWrite(ENC_SS1_PIN,HIGH);     // Terminate SPI conversation 

  
  // Read encoder 2
    digitalWrite(ENC_SS2_PIN,LOW);      // Begin SPI conversation
    SPI.transfer(0x60);                 // Request count
    txPacket.encoderB_1 = SPI.transfer(0x00);       // Read highest order byte
    txPacket.encoderB_2 = SPI.transfer(0x00);           
    txPacket.encoderB_3 = SPI.transfer(0x00);           
    txPacket.encoderB_4 = SPI.transfer(0x00);       // Read lowest order byte
    digitalWrite(ENC_SS2_PIN,HIGH);     // Terminate SPI conversation  

}

// ****************************************************
// Sets the encoder counts to 7F 7F 7F 7F
// RETURNS: none
// ****************************************************
void clearEncoderCount() {
    
  // Set encoder1's data register to 0
  digitalWrite(ENC_SS1_PIN,LOW);     // Begin SPI conversation  
  // Write to DTR
  SPI.transfer(0x98);    
  // Load data
  SPI.transfer(0x7F);                // Highest order byte
  SPI.transfer(0x7F);           
  SPI.transfer(0x7F);           
  SPI.transfer(0x7F);                // lowest order byte
  digitalWrite(ENC_SS1_PIN,HIGH);    // Terminate SPI conversation 
  
  delayMicroseconds(100);  // provides some breathing room between SPI conversations
  
  // Set encoder1's current data register to center
  digitalWrite(ENC_SS1_PIN,LOW);     // Begin SPI conversation  
  SPI.transfer(0xE0);    
  digitalWrite(ENC_SS1_PIN,HIGH);    // Terminate SPI conversation   
  
  // Set encoder2's data register to 0
  digitalWrite(ENC_SS2_PIN,LOW);     // Begin SPI conversation  
  // Write to DTR
  SPI.transfer(0x98);    
  // Load data
  SPI.transfer(0x7F);                // Highest order byte
  SPI.transfer(0x7F);           
  SPI.transfer(0x7F);           
  SPI.transfer(0x7F);                // lowest order byte
  digitalWrite(ENC_SS2_PIN,HIGH);    // Terminate SPI conversation 
  
  delayMicroseconds(100);  // provides some breathing room between SPI conversations
  
  // Set encoder2's current data register to center
  digitalWrite(ENC_SS2_PIN,LOW);     // Begin SPI conversation  
  SPI.transfer(0xE0);    
  digitalWrite(ENC_SS2_PIN,HIGH);    // Terminate SPI conversation 
}

// ****************************************************
// Stops the robot on a timeout event
// RETURNS: none
// ****************************************************
void allStop() {
  digitalWrite(DIRECTION_RIGHT, LOW);
  digitalWrite(DIRECTION_LEFT, LOW);
  analogWrite(PWM_RIGHT, 0);
  analogWrite(PWM_LEFT, 0);
  Serial.println("[All  Stop]");
}

// ****************************************************
// Processes the relays and reads inputs, applies them to packet
// RETURNS: none
// ****************************************************
void processIO () {
  unsigned char digitalByte = 0;
  
  digitalByte = rxPacket.digital1;
  
  // Control outputs of 4-Relay Board
  if (bitRead(digitalByte,0) == 1) { digitalWrite(RELAY_1_PIN, HIGH); }
  else                             { digitalWrite(RELAY_1_PIN, LOW); }
  
  if (bitRead(digitalByte,1) == 1) { digitalWrite(RELAY_2_PIN, HIGH); }
  else                             { digitalWrite(RELAY_2_PIN, LOW);  }
  
  if (bitRead(digitalByte,2) == 1) { digitalWrite(RELAY_3_PIN, HIGH); }
  else                             { digitalWrite(RELAY_3_PIN, LOW);  }
  
  if (bitRead(digitalByte,3) == 1) { digitalWrite(RELAY_4_PIN, HIGH); }
  else                             { digitalWrite(RELAY_4_PIN, LOW); }
    
  // Read in our analogs
  txPacket.batteryVoltage = analogRead(A1) >> 2;
  txPacket.frontSonar = analogRead(A2) >> 2;
  txPacket.rearSonar = analogRead(A3) >> 2;
  
  // Read in our digitals
  txPacket.bumper = digitalRead(A4);
  
}

// ****************************************************
// Sets servo positions
// RETURNS: none
// ****************************************************
void processServos() {
 int tiltPosition;
 int panPosition;
 
 tiltPosition = rxPacket.cameraTiltHigh * 0xFF;
 tiltPosition = tiltPosition + rxPacket.cameraTiltLow;

 panPosition = rxPacket.cameraPanHigh * 0xFF;
 panPosition = panPosition + rxPacket.cameraPanLow;
 
 tiltServo.writeMicroseconds(tiltPosition);
 panServo.writeMicroseconds(panPosition);

}

// ****************************************************
// Controls the motors and handles the movement 
// overrides and failsafes
// RETURNS: none
// ****************************************************
void processMotors() {
  
  unsigned char motorPowerRight;
  unsigned char motorPowerLeft;
  
  bool rightDirection;
  bool leftDirection;
  
  bool overrideBumper;
  bool overrideFrontSonar;
  bool overrideRearSonar;
  
  // Extract our values from the return packet
  motorPowerRight = rxPacket.rightMotorSpeed;
  motorPowerLeft = rxPacket.leftMotorSpeed;
  rightDirection = bitRead(rxPacket.digital1,6);
  leftDirection = bitRead(rxPacket.digital1,7);

  overrideBumper = bitRead(rxPacket.digital2,4);
  overrideFrontSonar = bitRead(rxPacket.digital2,2);
  overrideRearSonar = bitRead(rxPacket.digital2,3);

  // Handle the front bumper. It goes high when tripped
  // and we are only permitted to drive backwards
  if ( overrideBumper == 0 && txPacket.bumper == 1 && (rightDirection == 1 || leftDirection == 1 ) ) {
    motorPowerRight = 0;
    motorPowerLeft = 0;
  }
  
  // Handle the front sonar. Since the sonar trips while we are at
  // a distance, we can turn but not drive forward
  if (overrideFrontSonar == 0 && txPacket.frontSonar < FRONT_SONAR_THRESHOLD && (rightDirection == 1 && leftDirection == 1) ) {
    motorPowerRight = 0;
    motorPowerLeft = 0;    
  }
  
  // Handle the rear sonar. Since the sonar trips while we are at
  // a distance, we can turn but not drive forward
  if (overrideRearSonar == 0 && txPacket.rearSonar < REAR_SONAR_THRESHOLD && (rightDirection == 0 && leftDirection == 0) ) {
    motorPowerRight = 0;
    motorPowerLeft = 0;    
  }
  
  // Assign motor directions based on input packet
  if (rightDirection == 1)
    digitalWrite(DIRECTION_RIGHT, HIGH);
  else
    digitalWrite(DIRECTION_RIGHT, LOW);
  if (leftDirection == 1)
    digitalWrite(DIRECTION_LEFT, HIGH);
  else
    digitalWrite(DIRECTION_LEFT, LOW);
    
  analogWrite(PWM_RIGHT, motorPowerRight);
  analogWrite(PWM_LEFT, motorPowerLeft);
}

// ****************************************************
// Sends our data off to the PC
// RETURNS: none
// ****************************************************
void sendData() {
  
  Serial1.print("S1");

  Serial1.write(txPacket.encoderA_1);
  Serial1.write(txPacket.encoderA_2);
  Serial1.write(txPacket.encoderA_3);
  Serial1.write(txPacket.encoderA_4);  

  Serial1.write(txPacket.encoderB_1);
  Serial1.write(txPacket.encoderB_2);
  Serial1.write(txPacket.encoderB_3);
  Serial1.write(txPacket.encoderB_4);  
  
  Serial1.write(txPacket.batteryVoltage);    
  Serial1.write(txPacket.frontSonar);  
  Serial1.write(txPacket.rearSonar);    
  Serial1.write(txPacket.bumper);  
 
  Serial1.print("EOF");
}

// ****************************************************
// Handles our timeout. This is called every time through
// the main loop. Once our counter reaches a threshold the 
// motors are sent a stop command. This counter is reset
// on a good packet.
// RETURNS: none
// ****************************************************
void rxTimeout () {
  
  No_Tx = No_Tx + 1;			// Increment counter
  delay(10);
  
  // After a second without transmission, stop motors
  if (No_Tx > 80)  {
    allStop();	
  }

  if (No_Tx > 120)  {
    No_Tx = 0;
    sFound = false;
  }
}

// ****************************************************
// Initial hardware setup
// RETURNS: none
// ****************************************************
void setup() {

  // Initialize our serial COMs
  Serial.begin(115200);    // Serial output for debug
  Serial1.begin(115200);   // COM to Computer
  
  // Initialize our I/O                        
  pinMode(DIRECTION_RIGHT, OUTPUT);  
  pinMode(DIRECTION_LEFT, OUTPUT);  
  pinMode(PWM_RIGHT, OUTPUT);  
  pinMode(PWM_LEFT, OUTPUT); 
  pinMode(RELAY_1_PIN, OUTPUT);
  pinMode(RELAY_2_PIN, OUTPUT);
  pinMode(RELAY_3_PIN, OUTPUT);
  pinMode(RELAY_4_PIN, OUTPUT);
 
  pinMode(A4, INPUT); // Bumper 

  allStop();              // Make sure all motors are stopped for safety
  Serial.println("Motors Initialized...");  
  
  SPI.begin();
  initEncoders();       
  Serial.println("Encoders Initialized...");  
  clearEncoderCount();  
  Serial.println("Encoders Cleared...");  
  
  tiltServo.attach(6);
  panServo.attach(7);
  tiltServo.writeMicroseconds(1500);
  panServo.writeMicroseconds(1500);
  Serial.println("Servos Initialized...");
  
}

// ****************************************************
// Main program loop
// RETURNS: none
// ****************************************************
void loop() {
  
  word calculatedChecksum = 0;
  word expectedChecksum = 0;
  unsigned char rxByte = 0;
	
  // Debug Code
  //Serial.println("[Serial not Available]");

  // Wait for serial
  if (Serial1.available() > 0) {

    // Debug Code
    //Serial.println("[Serial is Available]");

    if (!sFound) {
      rxPacket.startFlag = Serial1.read();
      if(rxPacket.startFlag == 0x53) { sFound = true; } 
      //Serial.print("[START]");
    }

    if (sFound && (Serial1.available()  > 9 )) {
      
      // store bytes into the appropriate variables
      rxPacket.rightMotorSpeed  = Serial1.read();
      rxPacket.leftMotorSpeed   = Serial1.read();
      rxPacket.cameraTiltHigh   = Serial1.read();
      rxPacket.cameraTiltLow    = Serial1.read();
      rxPacket.cameraPanHigh    = Serial1.read();
      rxPacket.cameraPanLow     = Serial1.read();
      rxPacket.digital1         = Serial1.read();
      rxPacket.digital2         = Serial1.read();
      rxPacket.checksumHigh     = Serial1.read();
      rxPacket.checksumLow      = Serial1.read();  
      
      // Clear flag
      sFound = false; 
      
      calculatedChecksum =       rxPacket.startFlag + rxPacket.rightMotorSpeed +
                                 rxPacket.leftMotorSpeed + rxPacket.cameraTiltHigh + 
                                 rxPacket.cameraTiltLow + rxPacket.cameraPanHigh +
                                 rxPacket.cameraPanLow + rxPacket.digital1 + rxPacket.digital2;
                                 
      expectedChecksum = rxPacket.checksumHigh << 8;
      expectedChecksum = expectedChecksum + rxPacket.checksumLow;
      
      // Debug Code
      //Serial.print("["); Serial.print(expectedChecksum); Serial.print("]");
      //Serial.print("["); Serial.print(calculatedChecksum); Serial.print("]");
      
      // Checksum validation
      if (calculatedChecksum != expectedChecksum) {	
	// Debug
        //Serial.println("[Failed]");
        return;	
      }
      // Debug
      //Serial.println("[Passed]");
      
      // Clear our timeout counter
      No_Tx = 0;
      
      processIO();
      readEncoders();
      if (bitRead(rxPacket.digital2,0)) { clearEncoderCount(); }
      processMotors();
      processServos();
      sendData();
    } 
    else { rxTimeout(); }
  } else { rxTimeout(); }
}

