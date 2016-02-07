#!/usr/bin/env python
#PKG = 'numpy_tutorial'
#import roslib; roslib.load_manifest(PKG)
import cv2
import rospy
from rospy_tutorials.msg import Floats
from rospy.numpy_msg import numpy_msg
import numpy as np

def callback(data):
    #print rospy.get_name(), "I heard %s"%str(data.data)
    #data=np.asarray(data)
    print data
   # cv2.fromarray(data)
    #cv2.imshow(data)

def listener():
    rospy.init_node('listener')
    rospy.Subscriber("rgb", numpy_msg(Floats), callback)
    rospy.spin()

if __name__ == '__main__':
    listener()
