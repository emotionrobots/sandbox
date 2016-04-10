#!/usr/bin/env python

import rospy
import #file1
import #file2
import #file3
import #file4
import std_msgs.msg


start = False;

def main():
	print "hello"
	while(start == False):
		listen();
	else:
		voiceRec();
		if(batteryLevel() < .05)
			say("I am tired. Can I stop?");
			emoFace(sad , .5)
		if(batteryLevel() <= .01)
			say("Out of battery. I quit.");
			emoFace(sad, .99)

def movement(location):
	target = #location;
	moveToTarget();
	turnToMaster();
	emoFace(#emotion)

def faceRec():
	faceDetected = #getFace();
	if(faceDetected == master):
		return True;

def voiceRec():	
	lineHeard = #incoming voice;
	if(lineHeard == "go there"):
		movement(location);
	#add more voices lines
		
def emoFace(emotion):
	#LEONS STUFF

def listen():
	masterFace = faceRec();
	if(masterFace == True):
		say("Please raise your hands towards me");
		say("Where would you like me to go?");
		voiceRec();
	if(masterFace == False):
		say("Your are not my master")

def faceRecCallback():
	#code

def voiceRecCallback():
	#code

def emoFaceCallback():
	#code

def listener():
	rospy.init_node('listener')	
	rospy.Subscriber('faceRec', #datatype, faceRecCallback);
	rospy.Subscriber('voiceRec', #datatype, voicRecCallback);
	# rospy.Subscriber('')


if __name__ == '__main__':
	main();