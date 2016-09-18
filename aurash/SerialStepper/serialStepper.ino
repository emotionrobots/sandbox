const int micro1 = 11;
const int micro2 = 12;
const int micro3 = 13;
const int step1 = 9;
const int direc = 8;
const int step2 = 3;
const int direc2 = 2;
int stepcount=25;//default pulse count
int delays=0;

void setup() {
  // put your setup code here, to run once:

 pinMode(step1,OUTPUT);
 pinMode(direc,OUTPUT);
  pinMode(step2,OUTPUT);
 pinMode(direc2,OUTPUT);
 pinMode(micro1,OUTPUT);
 pinMode(micro2,OUTPUT);
 pinMode(micro3,OUTPUT);
 digitalWrite(micro1,HIGH);
 digitalWrite(micro2,HIGH);
 digitalWrite(micro3,HIGH);
 Serial.begin(9600);
}

void micro() {
     
     digitalWrite(micro1,HIGH);
     digitalWrite(micro2,HIGH);
     digitalWrite(micro3,HIGH);
     stepcount = 25; 
     delays=800;
 }

void half() {
 
    digitalWrite(micro1,HIGH);
    digitalWrite(micro2,LOW);
    digitalWrite(micro3,LOW);
    stepcount=5;
    delays=1500;
}


void six() {
 
    digitalWrite(micro1,LOW);
    digitalWrite(micro2,LOW);
    digitalWrite(micro3,HIGH);
    stepcount=25;
    delays=800;
}

void fourth() {
  
    digitalWrite(micro1,LOW);
    digitalWrite(micro2,HIGH);
    digitalWrite(micro3,LOW);
    stepcount=5;
    delays=1500;
}


void left() {
    
    digitalWrite(direc,HIGH);
    for(int x=0;x<stepcount;x++) {
        digitalWrite(step1,HIGH);
        delayMicroseconds(delays);
        digitalWrite(step1,LOW);
        delayMicroseconds(delays);
       }
  }

void right() {
  
  digitalWrite(direc,LOW);
  for(int x=0;x<stepcount;x++) {
    digitalWrite(step1,HIGH);
    delayMicroseconds(delays);
    digitalWrite(step1,LOW);
    delayMicroseconds(delays);
   }
  
 }
  
void up() {
  
      digitalWrite(direc2,LOW);
      for(int x=0;x<stepcount;x++) {
        digitalWrite(step2,HIGH);
        delayMicroseconds(delays);
        digitalWrite(step2,LOW);
        delayMicroseconds(delays);
       }
  
}
  
void down() {
        
        digitalWrite(direc2,HIGH);
        for(int x=0;x<stepcount;x++) {
        digitalWrite(step2,HIGH);
        delayMicroseconds(delays);
        digitalWrite(step2,LOW);
        delayMicroseconds(delays);
       }
  
  }
  
void loop() {

  if (Serial.available() > 0) {
    // read incoming serial data:
    char inChar = Serial.read();
  // put your main code here, to run repeatedly:

  // step size prior to stepping
  if(inChar == 'm') {
    micro();
  }
   else if(inChar == 'h') {
    half();
  }
   else if (inChar == 'f') {
    fourth();
  }
    else if (inChar == 's') {
    six();
  }


  // step direction and movement
  if(inChar=='l') {  
    left();
  }
  if(inChar=='r') {
    right();
  }
  if(inChar=='u') {
    up();
  }
  if(inChar=='d') {
    down();
  }
 
}}
