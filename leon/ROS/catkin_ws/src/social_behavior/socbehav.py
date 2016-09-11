#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
from array import *
from ros_rp2w.msg import Packet
from ros_rp2w.msg import Face
import time
import threading
import thread

global nameMap;nameMap = dict()
global name 
global prevVoltage
global amount; amount = 0
global firsttime; firsttime = True
global lock; lock = threading.Lock()
global pub; pub = rospy.Publisher('valence', String ,queue_size=1)


def publishVal(done):
	with lock:
		global nameMap
		res = " " + done + " " + nameMap[done]
		for key in nameMap:
			if (key != done):
				res += " "+ key + " " + nameMap[key]
		pub.publish(res)

def callbackrp2w(data):
	with lock:
		global nameMap
		global name
		global prevVoltage 
		global firsttime
		global amount
		if(data.rearSonar == 0 or data.frontSonar == 0 or data.bumper == 0): 
			amount -= 0.3
		if(data.batteryVoltage < 15):
			amount -= 0.1
		if(data.batteryVoltage > prevVoltage and firsttime)
			amount += 0.4
			firsttime = False
		else:
			firsttime = True

		prevVoltage = data.batteryVoltage
		publishVal(name)


def callbackFaceMsg(data):
	with lock:
		global name
		global nameMap
		global amount
		name=data.name
		if(nameMap[name] == None):
			nameMap[name] = 0.0
		else
			amount += 0.1
		publishVal(name)


def listener():
	rospy.Subscriber('rp2w_packet',Packet,callbackrp2w)
	rospy.Subscriber('known_faces',Face,callbackFaceMsg)
	rospy.spin()

def reg():
	with lock:
		global nameMap
		f = open('valence.txt','w')
		for key in nameMap:
			nameMap[key] = .8 * nameMap[key] + .2 * amount
			file.write(key + " " + nameMap[key])
		file.close()
		time.sleep(5)


def main():	
	rospy.init_node('valence', anonymous=True)
	thread.start_new_thread(reg,)
	listener()

if __name__ == '__main__':
	main()
