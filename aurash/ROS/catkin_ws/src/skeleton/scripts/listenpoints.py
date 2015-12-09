#!/usr/bin/env python

import rospy
from std_msgs.msg import UInt32MultiArray


def landmark(data):
	print data.data






def listener():
    rospy.Subscriber('face_points', UInt32MultiArray, landmark)
    rospy.spin()





if __name__ == '__main__':
	rospy.init_node('listenerp', anonymous=True)
	listener()