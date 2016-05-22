#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import time
import thread
import threading
from ros_rp2w.msg import Packet
from ros_rp2w.msg import Face
import time
import sys
import os
import numpy as np
from ros_rp2w.msg import CustomString
from std_msgs.msg import String, Header
import math
from ros_rp2w.srv._festTTS import *
import ast
# from vfacerec.msg import Face
# from vfacerec.msg import UnknownFace
# from vfacerec.srv import SetRate

global name
name = "aurash"
global name2
name2 = ""
first = True
global val
val = 0
global lock
lock = threading.Lock()
global command
command = -1
global prev
prev = -1
commands= [False for i in range(11)]
global found
found=False
global lockon
lockon = False


def publisher(done):
	pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	msg=String()
	msg.data= done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		#print("Message published")
		pub.publish(msg)

def publishMove(theta,distance):
    commands[5] = True
    pub2 = rospy.Publisher("rp2w/advanced_command", AdvancedCommand, queue_size=1)
    rate = rospy.Rate(10) # 10hz
    # while not rospy.is_shutdown():
    while pub.get_num_connections() == 0:
        rate.sleep()
    msg = AdvancedCommand()
    msg.theta = theta
    msg.distance = distance
    pub.publish(msg)
    #print msg

def callbackFaceMsg(data):
	global found
	with lock:
		#print("PublishFaceMsgs received")
		commands[0] = False
		commands[1] = False
		commands[2] = False
		commands[8] = False
		#tts_client(name)
		#print name
		if(data.name != name):
			commands[0] = True
		# elif(data.data > 1):
		# 	commands[1] = True
		# elif(data.data == 0):
		# 	commands[2] = True
		elif(data.name == name):
			name2=data.name
			found=True
			print found
			commands[8] = True
		func(.5)

def callbackPacket(data):
	global blocked
	global stopped
	global moving
	global battery
	battery=False
	moving=False
	stopped=False
	blocked=False
	with lock:
		#print("Packet received")
		commands[3] = False
		commands[4] = False
		if(data.batteryVoltage < 30):
			commands[3] = True
			battery=True
		elif(data.rearSonar <= 2 or data.frontSonar<= 2 or data.bumper<=2):
			commands[4] = True
			blocked=True
		elif(data.rightMotorSpeed == 0 and data.leftMotorSpeed == 0):
			commands[5] = False
			stopped=True
		#publishMove()
		elif(data.rightMotorSpeed > 0 or data.leftMotorSpeed > 0):
			commands[5] = True
			moving=True
		func(.5)

def callbackSpeech(msg):
	with lock:
		global utter
		utter=''
		text=str(msg)
		text=text[5:]
		utter=text
		#print("Chatter received")
		commands[6] = False
		commands[7] = False
		commands[9] = False
		if(text == "goodbye"):
			commands[7] = True
			tts_client('bye')
		elif(text == "go there"): 
			commands[5] = True
			tts_client('okay boss')
		# elif(text == "where would you like me to go next"):
		# 	commands[9] = True
		# 	print("where to?")
		func(.5)

def tts_client(string2):
    rospy.wait_for_service('tts')
    try:
        tts = rospy.ServiceProxy('tts', festTTS)
        req = festTTSRequest(string2)
        resp = tts(string2)
        #print str(resp)[4:]
    except rospy.ServiceException, e:
        pass#print "Service call failed: %s"%e

def callback_skeleton(msg):
	with lock:
		#print msg.header.stamp
		global res
		newpos_skeleton = ast.literal_eval(msg.data) 
		sk_torso = 8
		global dsttorso
		dstorso=newpos_skeleton[8][2]
		global lockon
		lockon=True

def callbackDir(msg):
	with lock:
		global direction
		direction=msg.data
		if direction=='north':
			direction=180
		if direction=='northwest':
			direction=45
		if direction=='northeast':
			direction=-45
		if direction=='west':
			direction=90
		if direction=='northwest':
			direction=-90				



def listener():
	#Scott RP2W battery(sad), obstacle(surprise), and movement(neutral)
	rospy.Subscriber("rp2w_packet", Packet, callbackPacket)
	#Dalton input for non-master(disgust), zero faces(increasing sad), and number of faces(surprise)
	rospy.Subscriber("known_faces", Face, callbackFaceMsg)
	rospy.Subscriber('skeleton', CustomString, callback_skeleton)
	rospy.Subscriber('direction', CustomString, callbackDir)
	#Aurash "I quit"(disgust = 1), master "where would you like me to go"(smile), ask for directions(increasing disgust), "Bye"(happy)
	rospy.Subscriber("Ready", String, callbackSpeech)
	rospy.spin()

def move(arg1, arg2):
	tts_client('lets party')
	while True:
		try:
			if battery:
				tts_client("I am getting tired!")
		except Exception, e:
			pass
		try:			
			if found and lockon and dstorso>914 and not blocked:
				toMove=round(((dsttorso-914)*0.00328084),0)
				publishMove(0,toMove)
		except Exception, e:
			pass		
		try:
			if found and not blocked and stopped and utter=="go there":
				tts_client("where would you like me to go?")
				theta=direction
				dst=8
				publishMove(theta,dst)	
		except Exception, e:
			pass
		try:			
			#print lockon
			if found and not lockon:
				tts_client("please assume the surrender position")
		except Exception, e:
			pass			




def func(delay):
	res = ""
	global prev
	global command
	global val
	#print (str(prev)+" "+str(command))
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
	elif(commands[2]):
		val+=.2
		if(val >= 1):
			val = 1
		res = "sad " + str(val)
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
		if(val >= 1):
			val = 1
		res = "disgust "+str(val)
		command = 9 
	#print(res)
	publisher(res)

		
def main():
	rospy.init_node('main_loop', anonymous=True)
	thread.start_new_thread(move,('data','none') )
	listener() 

if __name__ == '__main__':
	main()

