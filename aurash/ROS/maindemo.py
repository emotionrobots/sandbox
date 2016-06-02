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
from ros_rp2w.msg import AdvancedCommand

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
global once
once=False
global utter
utter=''
global oncelocked
oncelocked=False
global facecount
facecount=False

def publisher(done):
	pub = rospy.Publisher('emotion', String,queue_size=1)
	msg=String()
	msg.data= done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		#print("Message published")
		for x in xrange(5):
			pub.publish(msg)

def publishMove(theta,distance):
    commands[5] = True
    pub2 = rospy.Publisher("rp2w/advanced_command", AdvancedCommand, queue_size=1)
    rate = rospy.Rate(10) # 10hz
    # while not rospy.is_shutdown():
    while pub2.get_num_connections() == 0:
        rate.sleep()
    msg = AdvancedCommand()
    msg.theta = theta
    msg.distance = distance
    pub2.publish(msg)
    print msg

def callbackFaceMsg(data):
	global found
	global facecount
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
		if(data.name == name and not found):
			name2=data.name
			#print name2
			if facecount==False:
				tts_client("hi"+name2+", glad to see you")
			facecount=True	
			commands[8] = True
			time.sleep(2)
			found=True
			#print found
		func(.2)

def callbackPacket(data):
	global blockedfornt
	global blockedrear
	global blockedbumber
	global stopped
	global moving
	global battery
	moving=False
	stopped=False
	blockedfront=False
	blockedrear=False
	blockedbumber=False
	with lock:
		#print("Packet received")
		commands[3] = False
		commands[4] = False
		#print data.batteryVoltage
		if(data.batteryVoltage < 100):
			commands[3] = True
			battery=True
		else:
			battery=False	
		if(data.rearSonar <= 2 or data.frontSonar<= 2 or data.bumper==1):
			commands[4] = True
			if data.frontSonar<=2:
				blockedfront=True
			if data.rearSonar<=2:
				blockedrear=True
			if data.bumper<=2:
				blockedbumper=True					
			#print data.rearSonar
			# print data.frontSonar
			# print data.bumper
		else:
			blockedfront=False
			blockedrear=False
			blockedbumber=False	
		if(data.rightMotorSpeed == 0 and data.leftMotorSpeed == 0):
			commands[5] = False
			stopped=True
			#print data.leftMotorSpeed
		else:
			stopped=False
		#publishMove()
		if(data.rightMotorSpeed > 0 or data.leftMotorSpeed > 0):
			commands[5] = True
			moving=True
		else:
			moving=False	
		func(.2)

def callbackSpeech(msg):
	global utter
	with lock:
		text=str(msg)
		text=text[5:]
		text=str(text)
		#print utter 
		#print("Chatter received")
		commands[6] = False
		commands[7] = False
		commands[9] = False
		if(text == " bye nora"):
			commands[7] = True
			tts_client('bye')
			rospy.signal_shutdown("user terminated")
		elif(text == " move there"): 
			commands[5] = True
			tts_client('okay boss')
			time.sleep(1)
			utter=' move there'
		elif(text == " follow"): 
			commands[5] = True
			#tts_client('okay boss')	
			time.sleep(1)
			utter=" follow"
		elif(text == " find me"): 
			commands[5] = True
			#tts_client('okay boss')	
			time.sleep(1)
			utter=" find me"	
		elif(text == " turn"): 
			commands[5] = True
			#tts_client('okay boss')	
			time.sleep(1)
			utter=" turn"	
		elif(text == " yes"): 
			utter=" yes"		
		# elif(text == "where would you like me to go next"):
		# 	commands[9] = True
		# 	print("where to?")
		func(.2)

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
		global dstorso
		dstorso=newpos_skeleton[8][2]
        # # if newpos_skeleton[3][1]-newpos_skeleton[0][1]>200 or newpos_skeleton[7][1]-newpos_skeleton[0][1]>200:
        # # 	print "hand over head"
        # 	rospy.signal_shutdown("User terminated")
		global lockon
		lockon=True
		global oncelocked
		oncelocked=True
		commands[5]=True
		#print dstorso

def callbackDir(msg):
	with lock:
		global direction
		direction=msg.data
		#print direction
		if direction=='north':
			direction=180
		if direction=='northwest':
			direction=-135
		if direction=='northeast':
			direction=135
		if direction=='west':
			direction=-90
		if direction=='east':
			direction=90				



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
	tts_client('lets party!')
	time.sleep(1)
	count=0
	count2=0
	global utter
	global found
	global lockon
	battcount=0
	timesaid=time.time()
	movedonce=False
	while True:
		try:
			if battery and battcount<=3 and time.time()-timesaid>60:
				timesaid=time.time()
				battcount=battcount+1
				tts_client("I am getting tired can I stop?")
				time.sleep(2)
			if battery and utter==' yes':
				tts_client("please direct me to my charging station!")
				time.sleep(4)
				rospy.signal_shutdown('user terminated')	
		except Exception, e:
			pass
		try:			
			# print found
			# print lockon
			# print not blocked
			# print dstorso>914
			if found and lockon and dstorso>914 and not blockedfront and not blockedbumber and utter==' follow':
				tts_client('I will move closer to you')
				utter=''
				toMove=round(((dstorso-914)*0.00328084),0)
				print toMove
				publishMove(0,toMove)
				lockon=False
				movedonce=True
				time.sleep(toMove)
				found=False
			if found and not lockon and not blockedfront and not blockedbumber and utter==' follow':
				tts_client('I lost you, please place your hands up')
				time.sleep(2)
		except Exception, e:
			pass		
		try:
			# print found
			# print lockon
			# print not blocked
			#print stopped
			#print utter
			if found and lockon and not blockedfront and not blockedrear and not blockedbumber and stopped and utter==" move there":
				utter=''
				time.sleep(1)
				theta=direction
				dst=3
				tts_client('I will turn '+str(theta)+' degrees then move '+str(dst)+' feet')
				publishMove(theta,dst)	
				timer=(abs(theta)/45)*1.5
				lockon=False
				movedonce=True
				time.sleep(3+timer)
				found=False
			if found and not lockon and not blockedfront and not blockedrear and not blockedbumber and stopped and utter==' move there':
				tts_client('I lost you, please place your hands up')
				time.sleep(2)	
		except Exception, e:
			print e
		try:			
			# print lockon
			# print found
			# print stopped
			if found and not lockon and stopped and count<3 and not movedonce:
				tts_client("please assume the surrender position")
				time.sleep(3)
				count=count+1
			if found and not lockon and stopped and count>=3 and count<4 and not movedonce :
			 	tts_client("you are not listening to me, I quit ")
			 	commands[6]=True
			 	#func(.2)
			 	time.sleep(3)
		except Exception, e:
			print e
		try:			
			# print lockon
			# print found
			# print stopped
			count2=0
			while not found  and stopped and movedonce:# and utter==' find me': and not lockon
				if count2==8 and not blockedfront  and not blockedbumber and stopped:
					tts_client("I will scoot up a little")
					publishMove(0,2)
					time.sleep(2)
				if count2<1 or count2>4 :
					tts_client("I'm looking for you")
					time.sleep(1)
				time.sleep(1.5)
				if not found and not blockedbumber and not blockedrear:	
					publishMove(30,0)
					time.sleep(3)
				elif not found and not blockedbumber and blockedrear:
					publishMove(0,1)
					time.sleep(1.5)	
				elif not found and not blockedbumber and blockedfront:
					publishMove(0,-1)
					time.sleep(1.5)		
				count2=count2+1
		except Exception, e:
			print e
		try:			
			if  stopped and utter==' turn':
				tts_client("I'm turning 90 degrees" )
				utter=''
				time.sleep(1)
				publishMove(90,0)
				time.sleep(2)
		except Exception, e:
			print e





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
	thread.start_new_thread(move,('data','none'))
	listener() 

if __name__ == '__main__':
	main()

