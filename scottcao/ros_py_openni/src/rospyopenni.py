#!/usr/bin/env python

# coding: utf-8 

# In[ ]: 

import openni as opi
import numpy as np
from random import randrange
import sys
import rospy
from std_msgs.msg import String
import cv2


def publisher(): 

    rospy.init_node('OpenNI', anonymous=True)
    rgb_pub = rospy.Publisher('rgb', String, queue_size=10)
    depth_pub = rospy.Publisher('depth', String, queue_size=10) 
    gesture_pub = rospy.Publisher('gesture', String, queue_size=10)
    rate = rospy.Rate(30) # 10hz 

    # #### Create context and generators

    ctx = opi.Context()
    ctx.init() 

    depth_generator = opi.DepthGenerator()
    depth_generator.create(ctx)
    depth_generator.set_resolution_preset(opi.RES_VGA)
    depth_generator.fps = 30 

    image_generator = opi.ImageGenerator()
    image_generator.create(ctx) 

    hands_generator = opi.HandsGenerator()
    hands_generator.create(ctx)

    gesture_generator = opi.GestureGenerator()
    gesture_generator.create(ctx)
    gesture_generator.add_gesture("Click")
    gesture_generator.add_gesture('Wave') 


    # #### Write callbalks ...

    def gesture_detected(src, gesture, id, end_point):
        pass

    def gesture_progress(src, gesture, point, progress):
        # print "Emma Watson is waving !!", src 
        # print gesture
        gesture_pub.publish(""+gesture)


    # #### Register callbacks ...

    gesture_generator.register_gesture_cb(gesture_detected, gesture_progress)


    # #### Converting and publishing captured data

    def capture_rgb():
        rgb_pub.publish(image_generator.get_raw_image_map_bgr())

    def capture_depth():
        depth_pub.publish(depth_generator.get_raw_depth_map_8()) 


    ctx.start_generating_all() 

    # #### Main loop

    while not rospy.is_shutdown():
        capture_rgb()
        capture_depth()
        ctx.wait_any_update_all()
        rate.sleep() 


    # #### Then we stop and close the context 

    ctx.stop_generating_all()

    ctx.shutdown() 

if __name__ == '__main__':
    publisher() 

