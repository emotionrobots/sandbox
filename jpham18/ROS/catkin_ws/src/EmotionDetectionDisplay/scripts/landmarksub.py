#!/usr/bin/env python2

import rospy
from std_msgs.msg import String


def callback(data):
	print data.data


def listener():
    rospy.init_node('emotionsub',anonymous=True) 
    rospy.Subscriber("landmark",String,callback)
    print "test"
    rospy.spin()


if __name__ == '__main__':
	listener()