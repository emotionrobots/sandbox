#!/usr/bin/env python

import numpy as np
import rospy
import cv2
import ast
from ros_py_openni.msg import CustomString
from std_msgs.msg import String, Header
from threading import Lock

mutex = Lock()
left_hand = None
right_hand = None

def callback_rgb(msg):
    # print msg.header.stamp
    frame = np.fromstring(msg.data, dtype=np.uint8).reshape(480, 640, 3)
    mutex.acquire()
    global left_hand, right_hand
    if left_hand is not None:
        # cv2.circle(frame, left_hand, 5, (0, 255, 0))
        ratio = 1800/left_hand[2]
        left_hand_top_corner = (int(left_hand[0]-35*ratio), int(left_hand[1]-15*ratio))
        left_hand_bottom_corner = (int(left_hand[0]+35*ratio), int(left_hand[1]+70*ratio))
        cv2.rectangle(frame, left_hand_top_corner, left_hand_bottom_corner, (0, 255, 0))
    if right_hand is not None:
        ratio = 1800/right_hand[2]
        right_hand_top_corner = (int(right_hand[0]-35*ratio), int(right_hand[1]-15*ratio))
        right_hand_bottom_corner = (int(right_hand[0]+35*ratio), int(right_hand[1]+70*ratio))
        cv2.rectangle(frame, right_hand_top_corner, right_hand_bottom_corner, (0, 255, 0))
        # cv2.circle(frame, right_hand, 5, (0, 255, 0))
    mutex.release()
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
    mutex.acquire()
    global left_hand, right_hand
    left_hand = newpos_skeleton[3]
    right_hand = newpos_skeleton[7]
    print newpos_skeleton[3][2]
    print newpos_skeleton[7][2]
    # left_hand = newpos_skeleton[3]
    # right_hand = newpos_skeleton[7]
    mutex.release()
    # print newpos_skeleton

def callback_skeleton_msg(msg):
    # print msg.header.stamp
    print msg.data

if __name__ == '__main__':
    rospy.init_node('listener')
    rospy.Subscriber('rgb', CustomString, callback_rgb)
    # rospy.Subscriber('depth', CustomString, callback_depth)
    # rospy.Subscriber('gesture', CustomString, callback_gest)
    rospy.Subscriber('skeleton', CustomString, callback_skeleton)
    rospy.Subscriber('skeleton_msg', CustomString, callback_skeleton_msg)
    rospy.spin()