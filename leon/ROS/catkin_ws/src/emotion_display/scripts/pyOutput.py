#!/usr/bin/env python2
import rospy
import std_msgs.msg 
import time
import thread
import threading

global name
name = ""
first = True
global lock
lock = threading.Lock()
commands= [False for i in range(11)]


def publisher(done):
	pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	msg=String()
	msg.data= done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)
def callback(data):
	with lock:
		# commands[0] = False
		# commands[1] = False
		# commands[2] = False
		# commands[8] = False
		# if(data.data[0] != name):
		# 	commands[0] = True
		# elif(data.data amt > 1):
		# 	commands[1] = True
		# elif(data.data amt == 0):
		# 	commands[2] = True
		# elif(data.data[0] == name):
		#	commands[8] = True
def callback2(data):
	with lock:
		# commands[3] = False
		# commands[4] = False
		# commands[5] = False
		# if(data.batteryVoltage < ):
		# 	commands[3] = True
		# elif(data.rearSonar <= 2 or data.frontSonar<= 2 or data.bumper<=2)
		# 	commands[4] = True
		# elif(data.rightMotorSpeed > 0 or data.leftMotorSpeed > 0)
		# 	commands[5] = True
def callback3(data):
	with lock:
		# commands[6] = False
		# commands[7] = False
		# commands[9] = False
		# if(data.data == "i quit")
		# 	commands[6] = True
		# elif(data.data == "bye"): 
		# 	commands[7] = True
		# elif(data.data == "where would you like me to go next"):
		# 	commands[9] = True
def listener():
	rospy.init_node('main_loop', anonymous=True)
	#Dalton input for non-master(disgust), zero faces(increasing sad), and number of faces(surprise)
	rospy.Subscriber("known_faces", PublishFaceMsgs, callback)
	# Scott RP2W battery(sad), obstacle(surprise), and movement(neutral)
	rospy.Subscriber("rp2w_packet", Packet, callback2)
	# Aurash "I quit"(disgust = 1), master "where would you like me to go"(smile), ask for directions(increasing disgust), "Bye"(happy)
	rospy.Subscriber("chatter", String, callback3)
	rospy.spin()

def func(delay):
	res = ""
	val = 0
	command = -1
	prev = -1
	while(not rospy.is_shutdown):
		if(not prev == command):
			val = 0
		prev = command
		if(commands[3]):
			res = "sad 1"
			command = 3
		elif(commands[4]):
			res = "surprise 1"
			command = 4
		elif(commands[0]):
			res = "disgust 1"
			command = 0
		elif(commands[2]):
			val+=.2
			res = "sad " + val
			command = 2
		elif(commands[1]):
			res = "surprise 1"
			command = 1
		elif(commands[5]):
			res = "neutral 1"
			command = 5
		elif(commands[6]):
			res =  "disgust 1"
			command = 6
		elif(commands[7]):
			res = "happy 1"
			command = 7
		elif(commands[8]):
			res = "happy 1"
			command = 8
		elif(commands[9]):
			val+=.33
			res = "disgust "+val
			command = 9 
		publisher(res)
		time.sleep(delay)
def main():
	thread.start_new_thread(func, (.25 ))
	listener() 

if __name__ == '__main__':
	main()

