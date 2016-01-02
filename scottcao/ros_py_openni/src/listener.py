#!/usr/bin/env python

import numpy as np
import rospy
import cv2
import ast
from ros_py_openni.msg import CustomString
from std_msgs.msg import String, Header


def callback_rgb(msg):
    # print msg.header.stamp
    frame = np.fromstring(msg.data, dtype=np.uint8).reshape(480, 640, 3)
    cv2.imshow('Frame', frame)
    cv2.waitKey(3)

def callback_depth(msg):
    # print msg.header.stamp
    frame = np.fromstring(msg.data, dtype=np.uint8).reshape(480, 640)

def callback_gest(msg):
    # print msg.header.stamp
    print msg.data

def callback_skeleton(msg):
    # print msg.header.stamp
    newpos_skeleton = ast.literal_eval(msg.data)
    # print newpos_skeleton

def callback_skeleton_msg(msg):
    # print msg.header.stamp
    print msg.data

if __name__ == '__main__':
    rospy.init_node('listener')
    rospy.Subscriber('rgb', CustomString, callback_rgb)
    rospy.Subscriber('depth', CustomString, callback_depth)
    rospy.Subscriber('gesture', CustomString, callback_gest)
    rospy.Subscriber('skeleton', CustomString, callback_skeleton)
    rospy.Subscriber('skeleton_msg', CustomString, callback_skeleton_msg)
    rospy.spin()