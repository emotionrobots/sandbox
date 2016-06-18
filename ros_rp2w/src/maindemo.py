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
import pygame
global gesture1
gesture1=''
global name
name = "scott"
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
global commands
commands= [False for i in range(11)]
global found
found=False
global lockon
lockon = False
global once
once=False
global utter
global acception
acception=False
utter=''
global oncelocked
oncelocked=False
global facecount
facecount=False
global looking
looking=False
global started
started=False
global handl
global handr
global handld
global onlyone
onlyone=True
global handrd
global heady
global headd
global pub; pub = rospy.Publisher('emotion', String,queue_size=1)
global pub2; pub2 = rospy.Publisher("rp2w/advanced_command", AdvancedCommand, queue_size=1)

def publisher(done):
	print "Published Message"
	msg=String()
	msg.data= done
	r = rospy.Rate(10)
	if not rospy.is_shutdown():
		print(msg.data)
		while pub.get_num_connections() == 0:
			r.sleep()
		pub.publish(msg)

def publishMove(theta,distance):
    commands[5] = True
    rate = rospy.Rate(10) # 10hz
    # while not rospy.is_shutdown():
    while pub2.get_num_connections() == 0:
        rate.sleep()
    msg = AdvancedCommand()
    msg.theta = theta
    msg.distance = distance
    pub2.publish(msg)
    #print msg

def callbackFaceMsg(data):
	global found
	global looking
	global onlyone
	global facecount
	global oncelocked
	global started
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
			if onlyone:
				started=True
				onlyone=False
			name2=data.name
			#print name2
			if facecount==False:
				tts_client("hi"+name2)
			if looking:
				tts_client("hi"+name2+', please reccallebraate')	
				looking=False
				oncelocked=False
			facecount=True	
			commands[8] = True
			time.sleep(2)
			found=True
			#print found
		func(0)

def callbackPacket(data):
	global blockedfront
	global blockedrear
	global blockedbumper
	global stopped
	global moving
	global battery
	moving=False
	stopped=False
	blockedfront=False
	blockedrear=False
	blockedbumper=False
	with lock:
		#print("Packet received")
		commands[3] = False
		commands[4] = False
		#print data.batteryVoltage
		if(data.batteryVoltage < 115):
			commands[3] = True
			battery=True
		else:
			battery=False	
		if(data.rearSonar <= 2 or data.frontSonar<= 2 or data.bumper==1):
			commands[4] = True
			if data.frontSonar<=2:
				blockedfront=True
			if data.rearSonar<=3:
				blockedrear=True
			if data.bumper<=2:
				blockedbumper=True					
			#print data.rearSonar
			# print data.frontSonar
			# print data.bumper
		else:
			blockedfront=False
			blockedrear=False
			blockedbumper=False	
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
	global found
	global acception
	global gesture1
	global started
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
			tts_client('okay')	
			time.sleep(1)
			utter=" follow"
		elif(text == " find me"): 
			commands[5] = True
			#tts_client('okay boss')	
			time.sleep(1)
			found=False
			utter=" find me"	
		elif(text == " turn"): 
			commands[5] = True
			#tts_client('okay boss')	
			time.sleep(1)
			utter=" turn"	
		elif(text == " music"): 
			commands[5] = True
			#tts_client('okay boss')	
			gesture1=''
			utter=" music"	
			started=False
		elif(text == " control"): 
			tts_client('guide me')
			commands[5] = True
			#tts_client('okay boss')	
			time.sleep(1)
			utter=" control"		
		elif(text == " yes" and acception): 
			utter=" yes"		
		elif(text == " no" and acception): 
			utter=" no"	
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
	global oncelocked
	with lock:
		global lockon
		#print msg.header.stamp
		global res
		global handl
		global handr
		global handld
		global handrd
		global heady
		global headd
		if not oncelocked:
			tts_client("locked on!")
			oncelocked=True
		newpos_skeleton = ast.literal_eval(msg.data) 
		heady=newpos_skeleton[0][1]
		headd=newpos_skeleton[0][2]
		handl=newpos_skeleton[7][1]
		handr=newpos_skeleton[3][1]
		handld=newpos_skeleton[7][2]
		handrd=newpos_skeleton[3][2]
		sk_torso = 8
		global dstorso
		global torsox
		torsox=newpos_skeleton[8][0]
		dstorso=newpos_skeleton[8][2]
		if dstorso<400:
			tts_client("I lost you, please raise your hands")
			oncelocked=False
			lockon=False
        # # if newpos_skeleton[3][1]-newpos_skeleton[0][1]>200 or newpos_skeleton[7][1]-newpos_skeleton[0][1]>200:
        # # 	print "hand over head"
        # 	rospy.signal_shutdown("User terminated")
		lockon=True
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
			direction=-90
		if direction=='northeast':
			direction=90
		if direction=='west':
			direction=-90
		if direction=='east':
			direction=90				

def callback_gest(msg):
    global gesture1
    gesture1=msg.data
    #global started
    # if gesture1=='wave'
    # 	started=True
    #print gesture1

def listener():
	#Scott RP2W battery(sad), obstacle(surprise), and movement(neutral)
	rospy.Subscriber("rp2w_packet", Packet, callbackPacket)
	#Dalton input for non-master(disgust), zero faces(increasing sad), and number of faces(surprise)
	rospy.Subscriber("known_faces", Face, callbackFaceMsg)
	rospy.Subscriber('skeleton', CustomString, callback_skeleton)
	rospy.Subscriber('direction', CustomString, callbackDir)
	#Aurash "I quit"(disgust = 1), master "where would you like me to go"(smile), ask for directions(increasing disgust), "Bye"(happy)
	rospy.Subscriber("Ready", String, callbackSpeech)
	rospy.Subscriber('gesture', CustomString, callback_gest)
	rospy.spin()



def move(arg1, arg2):
	tts_client('lets party!')
	time.sleep(1)
	count=0
	count2=0
	global utter
	global found
	global lockon
	global name
	global oncelocked
	global looking
	global handl
	global handr
	global started
	global handld
	global handrd
	global headd
	global heady
	global acception
	acception=False
	import random
	global gesture1
	battcount=0
	timesaid=time.time()
	movedonce=False
	spinright=False
	sprinleft=True
	while True:
		try:
			if battery and battcount<=3 and time.time()-timesaid>60:
				timesaid=time.time()
				battcount=battcount+1
				acception=True
				tts_client("I am getting tired can I stop?")
				time.sleep(2)
			if battery and utter==' yes':
				tts_client("thanks, please direct me to my charging station!")
				time.sleep(4)
				rospy.signal_shutdown('user terminated')
			if battery and utter==' no':
				tts_client("this is against labor laws")
				time.sleep(3)
				acception=False			
		except Exception, e:
			pass
		try:
			rand=random.randint(1,5)
			if not started and gesture1!='Wave' or utter==' music' :
				utter=''
				gesture1=''
				pygame.mixer.init()
				pygame.mixer.music.load(os.path.abspath(os.path.dirname(__file__))+"/songs/"+str(rand)+".mp3")
				pygame.mixer.music.play()
				while pygame.mixer.music.get_busy() == True and gesture1!='Wave' and not started:
					#print gesture1
					#print started
					continue		
				pygame.mixer.music.stop()		
		except Exception, e:
			pass	
		try:			
			# print found
			# print lockon
			# print not blocked
			# print dstorso>914
			# print lockon
			if found and not lockon and not blockedfront and not blockedbumper and utter==' follow':
				tts_client('I lost you, please place your hands up')
				oncelocked=False
				time.sleep(2)
			spincount=0	
			while found and lockon and dstorso>1219  and not blockedfront and not blockedbumper and utter==' follow':
				#tts_client('Okay')
				#lockon=False
				#utter=''
				toMove=abs(round(((dstorso-1219)*0.00328084),0))
				
				while torsox<280 or torsox>370 and utter==' follow':
					# if spincount>5:
					# 	utter=' find me'
					# 	movedonce=True
					# 	found=False
					# 	time.sleep(.2)
					div=torsox/dstorso
					toTurn=math.atan(div)
					toTurn=round(math.degrees(toTurn),0)
					if torsox>320:
						toTurn=toTurn*-1	
					#print toTurn
					if abs(toTurn)>=3:
						#print "turning"
						publishMove(int(toTurn),0)
						spincount=spincount+1
						time.sleep(.5)	
				# spincountr=0
				# spincountl=0
				# # while torsox<280 or torsox>370:
				# # 	if torsox<280  and spincountl<6:
				# # 		# if 320-torsox>70:
				# # 		#  	publishMove(30,0)
				# # 		# else:
				# # 		#  	publishMove(15,0)
				# # 		publishMove(15,0)
				# # 		time.sleep(.5)
				# # 		spincountl=spincountl+1
				# # 		spincountr=0
				# # 	if torsox>360 and spincountr<6:
				# # 		# if torsox-320>70:
				# # 		#  	publishMove(-30,0)
				# # 		# else:		
				# # 		#  	publishMove(-15,0)
				# # 		publishMove(-15,0)
				# # 		time.sleep(.5)	
				# # 		spincountr=spincountr+1
				# # 		spincountl=0
				# #print toMove
				# print toTurn
				# if abs(toTurn)>=3:
				# 	print "turning"
				# 	publishMove(int(toTurn),0)
				# 	time.sleep(1)
				if toMove>3:
				 	publishMove(0,3)
					spincount=0
				elif toMove!=0:	
					publishMove(0,toMove)
					spincount=0
				lockon=False
				movedonce=True
				time.sleep(toMove+.5)
			
				#found=False
			# if found and lockon and dstorso>914 and not blockedfront and not blockedbumper and utter==' follow':
			# 	tts_client('I will move closer to you')
			# 	utter=''
			# 	toMove=round(((dstorso-914)*0.00328084),0)
			# 	print toMove
			# 	publishMove(0,toMove)
			# 	lockon=False
			# 	movedonce=True
			# 	time.sleep(toMove+1.5)
			# 	found=False
			# if found and not lockon and not blockedfront and not blockedbumper and utter==' follow':
			# 	tts_client('I lost you, please place your hands up')
			# 	time.sleep(2)
		except Exception, e:
			print e	
		try:
			spincount=0	
			while found and lockon and dstorso<1219  and not blockedrear and not blockedbumper and utter==' follow':
					#tts_client('Okay')
					#lockon=False
					#utter=''
				toMove=abs(round(((1219-dstorso)*0.00328084),0))
				
				while torsox<280 or torsox>370 and utter==' follow':
					# if spincount>5:
					# 	utter=' find me'
					# 	found=False
					# 	movedonce=True
					# 	time.sleep(.2)
					div=torsox/dstorso
					toTurn=math.atan(div)
					toTurn=round(math.degrees(toTurn),0)
					if torsox>320:
						toTurn=toTurn*-1	
					#print toTurn
					if abs(toTurn)>=3:
						#print "turning"
						publishMove(int(toTurn),0)
						spincount=spincount+1
						time.sleep(.5)	
					# spincountr=0
					# spincountl=0
					# # while torsox<280 or torsox>370:
					# # 	if torsox<280  and spincountl<6:
					# # 		# if 320-torsox>70:
					# # 		#  	publishMove(30,0)
					# # 		# else:
					# # 		#  	publishMove(15,0)
					# # 		publishMove(15,0)
					# # 		time.sleep(.5)
					# # 		spincountl=spincountl+1
					# # 		spincountr=0
					# # 	if torsox>360 and spincountr<6:
					# # 		# if torsox-320>70:
					# # 		#  	publishMove(-30,0)
					# # 		# else:		
					# # 		#  	publishMove(-15,0)
					# # 		publishMove(-15,0)
					# # 		time.sleep(.5)	
					# # 		spincountr=spincountr+1
					# # 		spincountl=0
					# #print toMove
					# print toTurn
					# if abs(toTurn)>=3:
					# 	print "turning"
					# 	publishMove(int(toTurn),0)
					# 	time.sleep(1)
				if toMove!=0:	
					publishMove(0,(toMove)*-1)
					spincount=0
				lockon=False
				movedonce=True
				time.sleep(toMove+.5)
				
					#found=False
				# if found and lockon and dstorso>914 and not blockedfront and not blockedbumper and utter==' follow':
				# 	tts_client('I will move closer to you')
				# 	utter=''
				# 	toMove=round(((dstorso-914)*0.00328084),0)
				# 	print toMove
				# 	publishMove(0,toMove)
				# 	lockon=False
				# 	movedonce=True
				# 	time.sleep(toMove+1.5)
				# 	found=False
				# if found and not lockon and not blockedfront and not blockedbumper and utter==' follow':
				# 	tts_client('I lost you, please place your hands up')
				# 	time.sleep(2)
		except Exception, e:
			print e		
		try:
			# print found
			# print lockon
			# print not blocked
			#print stopped
			#print utter
			if found and lockon and not blockedfront and not blockedrear and not blockedbumper and stopped and utter==" move there":
				spinleft=False
				spinright=False
				utter=''
				#time.sleep(1)
				theta=direction
				if theta<0:
					spinleft=True
				else:
					spinright=True	
				dst=3
				#tts_client('I will turn '+str(theta)+' degrees then move '+str(dst)+' feet')
				publishMove(theta,dst)	
				timer=(abs(theta)/45)*1.5
				lockon=False
				utter=' find me'
				movedonce=True
				time.sleep(3+timer)
				found=False
				lockon=False
			if found and not lockon and not blockedfront and not blockedrear and not blockedbumper and stopped and utter==' move there':
				tts_client('I lost you, please place your hands up')
				time.sleep(2)	
				oncelocked=False
		except Exception, e:
			print e
		try:
			# print found
			# print lockon
			# print not blocked
			#print stopped
			#print utter
			if found and lockon and utter==" control":
				spinleft=False
				spinright=False
				time.sleep(2)
				turnright=False
				turnleft=False
				forward=False
				backward=False
				# print str(heady)+ '   ' +str(handr)
				while handr-handl>200 and handl<heady and handr>heady and utter==" control":
					turnright=True
					if turnright and stopped and not blockedbumper and not blockedfront and not blockedrear:
						theta=-90
						publishMove(theta,0)
						lockon=False
						spinleft=True
						utter=' find me'
						found=False
						time.sleep(1.5)
				while handl-handr>200 and handr<heady and handl>heady and utter==" control":
					turnleft=True
					if turnleft and stopped and not blockedbumper and not blockedfront and not blockedrear:
						theta=90
						publishMove(theta,0)
						lockon=False
						found=False
						spinright=True
						utter=' find me'
						time.sleep(1.5)	
				while handr<heady and handl<heady:
					forward=True
					if forward and stopped and not blockedbumper and not blockedfront and utter==" control":
						distance=1
						publishMove(0,distance)
						lockon=False
						time.sleep(1.15)
				while headd-handrd>650 and headd-handld>650:
				 	backward=True
				 	if backward and stopped and not blockedbumper and not blockedrear and utter==" control":
						distance=-1
						publishMove(0,distance)	
						lockon=False
						time.sleep(1.5)				
				movedonce=True
			if found and not lockon and utter==' control':
				tts_client('I lost you, please place your hands up')
				time.sleep(2)	
				oncelocked=False
		except Exception, e:
			print e	
		try:			
			# print lockon
			# print found
			# print stopped
			if found and not lockon and stopped and count<5 and not movedonce:
				tts_client("please assume the surrender position")
				time.sleep(3)
				count=count+1
			if found and not lockon and stopped and count>=5 and count<4 and not movedonce :
			 	tts_client("you are not listening to me, I quit ")
			 	commands[6]=True
			 	#func(.2)
			 	count=count+1
			 	time.sleep(3)
			 	rospy.signal_shutdown('user terminated')
		except Exception, e:
			print e
		try:			
			# print lockon
			# print found
			# print stopped
			count2=0
			while not found and stopped and movedonce and utter==' find me': #and not lockon
				looking=True
				if count2>8 and not blockedfront  and not blockedbumper and stopped:
					tts_client("where are you,"+name)
					publishMove(0,2)
					time.sleep(2)
					count2=0
				if count2<1:
					tts_client("I'm looking for you")
					#time.sleep(1)
				if count2>4 and count2<6:
					tts_client("I'm looking for you")
					#time.sleep(1)	
				time.sleep(1)
				if not found and not blockedbumper :	
					if spinright:
						publishMove(-45,0)
					elif spinleft:
						publishMove(45,0)
					time.sleep(2)
				elif not found and not blockedbumper and blockedrear:
					publishMove(0,1)
					time.sleep(1.5)	
				elif not found and not blockedbumper and blockedfront:
					publishMove(0,-1)
					time.sleep(1.5)		
				count2=count2+1
		except Exception, e:
			print e
		try:			
			if  stopped and utter==' turn':
				tts_client("okay" )
				if   not blockedbumper and not blockedrear:	
					publishMove(180,0)
					time.sleep(3)
				elif  not blockedbumper and blockedrear:
					publishMove(0,1)
					time.sleep(1.5)	
				elif  not blockedbumper and blockedfront:
					publishMove(0,-1)
					time.sleep(1.5)
				utter=''
				time.sleep(1)
				time.sleep(2)
		except Exception, e:
			print e





def func(delay):
	res = ""
	global prev
	global command
	global val
	print (str(prev)+" "+str(command))
	if(not prev == command):
		val = 0
	prev = command
	if(commands[3]):
		res = "sad 50"
		command = 3
	elif(commands[4]):
		res = "surprise 50"
		command = 4
	elif(commands[0]):
		res = "disgust 50"
	elif(commands[2]):
		val+=10
		if(val >= 1):
			val = 10
		res = "sad " + str(val)
		command = 2
	elif(commands[1]):
		res = "surprise 50"
		command = 1
	elif(commands[5]):
		res = "neutral 50"
		command = 5
	elif(commands[6]):
		res =  "disgust 50"
		command = 6
	elif(commands[7]):
		res = "happy 50"
		command = 7
	elif(commands[8]):
		res = "happy 50"
		command = 8
	elif(commands[9]):
		val+=10
		if(val >= 1):
			val = 10
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

