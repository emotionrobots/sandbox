#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import time
from random import randint
global st
st = ""

def publisher(done):
	pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	msg=String()
	msg.data= done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)

def main1():
	rospy.Subscriber("emotion",String,callback)
	rospy.spin()

def main():
	while(True):
			x = randint(0,6)
			y = randint(0,100)
			if(x == 0):
				st = "anger"
			elif(x == 1):
				st = "disgust"
			elif(x == 2):
				st = "fear"
			elif(x == 3):
				st = "happy"
			elif(x == 4):
				st = "neutral"
			elif(x == 5):
				st = "sad"
			else:
				st = "surprise"
			st = st + " "+str(y/100.0)
			print str(st)
			publisher(st)
			time.sleep(.5)		

def callback(data):
	while(True):
		#	x = randint(0,6)
		#	y = randint(0,100)
		#	if(x == 0):
		#		st = "anger"
		#	elif(x == 1):
		#		st = "disgust"
		#	elif(x == 2):
		#		st = "fear"
		#	elif(x == 3):
		#		st = "happy"
		#	elif(x == 4):
		#		st = "neutral"
		#	elif(x == 5):
		#		st = "sad"
		#	else:
		#		st = "surprise"
		#	st = st + " "+str(y/100.0)
		#print str(data.data)
		#publisher(data.data)
		#time.sleep(.5)	
		print str(st)
		publisher(st)
		time.sleep(.5)	


if __name__ == '__main__':
	rospy.init_node('emotiondisplay2', anonymous=True)	
	main()
