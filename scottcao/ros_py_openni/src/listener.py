#!/usr/bin/env python

import numpy as np
import rospy
import cv2
from std_msgs.msg import String


def callback_rgb(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    cv2.imshow('Frame', frame)
    cv2.waitKey(3)

def callback_depth(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640)


def listener():
    rospy.init_node('listener')
    rospy.Subscriber('rgb', String, callback_rgb)
    rospy.Subscriber('depth', String, callback_depth)
    rospy.spin()

if __name__ == '__main__':
    listener()