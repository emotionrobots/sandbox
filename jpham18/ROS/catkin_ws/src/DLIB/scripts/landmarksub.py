#!/usr/bin/env python2

import rospy
from std_msgs.msg import String

global 


def callback(data):
	if data == True


def listener():
    rospy.init_node('sameSpeed',anonymous=True) 
    rospy.Subscriber("pygameFace",String,callback)
    rospy.spin()


if __name__ == '__main__':
	listener()