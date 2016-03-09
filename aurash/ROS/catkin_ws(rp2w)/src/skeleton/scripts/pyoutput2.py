#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import time
from random import randint
import sys
global st
st = ""
def publisher(done):
	pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	rospy.init_node('emotiondisplay2', anonymous=True)
	msg=String()
	msg.data= done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)
def main():
	x= int(sys.argv[1])
	y = int(sys.argv[2])
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

if __name__ == '__main__':
    main()