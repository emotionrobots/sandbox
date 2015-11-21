#!/usr/bin/env python

import numpy as np
import rospy
import cv2
import ast

from beginner_tutorials.msg import Skeleton
from std_msgs.msg import String


def callback_rgb(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    cv2.imshow('Frame', frame)
    cv2.waitKey(3)

def callback_depth(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640)

def callback_gest(data):
    print data

def callback_skeleton(data):
    newpos_skeleton = ast.literal_eval(data.data)
    print data.id
    # print newpos_skeleton

if __name__ == '__main__':
    rospy.init_node('listener')
    rospy.Subscriber('rgb', String, callback_rgb)
    rospy.Subscriber('depth', String, callback_depth)
    rospy.Subscriber('gesture', String, callback_gest)
    rospy.Subscriber('skeleton', Skeleton, callback_skeleton)
    rospy.spin()