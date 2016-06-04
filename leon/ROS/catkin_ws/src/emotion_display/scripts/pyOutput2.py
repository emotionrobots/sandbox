#!/usr/bin/env python2
import pygame
from random import randint
import rospy
from std_msgs.msg import String
import time

def publisher(done):
	pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	msg=String()
	msg.data= done
	r = rospy.Rate(.5)
	if not rospy.is_shutdown():
		print("Message published")
		pub.publish(msg)
		r.sleep()

def main():
	rospy.init_node('main_loop', anonymous=True)
	while(not rospy.is_shutdown()):
		res = ""
		num = randint(1,7)
		if(num == 1):
			res += "anger"
		elif(num == 2):
			res += "disgust"
		elif(num == 3):
			res += "fear"
		elif(num == 4):
			res += "happy"
		elif(num == 5):
			res += "neutral"
		elif(num == 6):
			res += "sad"
		else:
			res += "surprise"
		num2 = randint(1,100)
		res+= " "+str(num2)
		# print res
		publisher(res)

if __name__ == '__main__':
	main()