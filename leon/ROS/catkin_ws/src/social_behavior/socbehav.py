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

		if(data.rearSonar == 0 or data.frontSonar == 0 or data.bumper == 0): 
			nameMap[name] -= 0.5
		if(data.batteryVoltage < 15):
			nameMap[name] -= 0.2
		if(data.batteryVoltage > prevVoltage and firsttime)
			nameMap[name] += 0.4
			firsttime = False
		else:
			firsttime = True

		prevVoltage = data.batteryVoltage
		publishVal(name)


def callbackFaceMsg(data):
	with lock:
		global name
		global nameMap
		name=data.name
		if(nameMap[name] == None):
			nameMap[name] = 0.0
		else
			nameMap[name] += 0.1
		publishVal(name)


def listener():
	rospy.Subscriber('rp2w_packet',Packet,callbackrp2w)
	rospy.Subscriber('known_faces',Face,callbackFaceMsg)
	rospy.spin()

def reg():
	with lock:
		global nameMap
		for key in nameMap:
			if(nameMap[key] < 0.0)
				nameMap[key] += .001
			if(nameMap[key] > 0.0)
				nameMap[key] -= .001
		time.sleep(5)


def main():	
	rospy.init_node('valence', anonymous=True)
	thread.start_new_thread(reg,)
	listener()

if __name__ == '__main__':
	main()
