#!/usr/bin/env python
# coding: utf-8

# In[ ]:

import openni as opi
import numpy as np
# import cv
# import cv2
# import Image
from random import randrange
import sys
import rospy
from rospy.numpy_msg import numpy_msg
from rospy_tutorials.msg import Floats
# from std_msgs.msg import String
# from sensor_msgs.msg import Image
# from cv_bridge import CvBridge, CvBridgeError


def publisher(): 

    pub = rospy.Publisher('frames', numpy_msg(Floats), queue_size=10)
    # pub = rospy.Publisher('frames', String, queue_size=10)
    rospy.init_node('OpenNI', anonymous=True)
    rate = rospy.Rate(5) # 10hz

    # #### Create context and generators

    # In[ ]:

    ctx = opi.Context()
    ctx.init()


    # In[ ]:

    depth_generator = opi.DepthGenerator()
    depth_generator.create(ctx)
    depth_generator.set_resolution_preset(opi.RES_VGA)
    depth_generator.fps = 30

    image_generator = opi.ImageGenerator()
    image_generator.create(ctx)

    gesture_generator = opi.GestureGenerator()
    gesture_generator.create(ctx)
    gesture_generator.add_gesture('Wave')


    # #### Write some callbalks ...

    # In[ ]:

    def gesture_detected(src, gesture, id, end_point):
        pass

    def gesture_progress(src, gesture, point, progress): 
        print "Dave is waving !!", src


    # #### And register them ...

    # In[ ]:

    gesture_generator.register_gesture_cb(gesture_detected, gesture_progress)


    # #### Some things for this particular demo ...

    # In[ ]:


    def capture_rgb():
        # Create array from the raw depth map string
        #frame = np.fromstring(depth_generator.get_raw_depth_map_8(), "uint8").reshape(480, 640)
        rgb_frame = np.fromstring(image_generator.get_raw_image_map_bgr(),
                              dtype=np.uint8).reshape(480, 640, 3)
        print((type)(rgb_frame))
        pub.publish(rgb_frame)
        rate.sleep()

    ctx.start_generating_all()

    while not rospy.is_shutdown():
        capture_rgb()
        ctx.wait_any_update_all()


    # #### Then we stop and close the context

    # In[ ]:

    ctx.stop_generating_all()
    ctx.shutdown()

if __name__ == '__main__':
    try:
        publisher()
    except rospy.ROSInterruptException:
        pass