#!/usr/bin/env python

import numpy as np
import rospy
import cv2
import ast
from std_msgs.msg import String, Header
import Image
from vfacerec.msg import Face
from vfacerec.msg import PublishFaceMsg



def facer(data):
    x=data.data
    print "test"
    print x

	







def run():
    rospy.init_node('listener2')
    rospy.Subscriber("known_faces",PublishFaceMsg,facer)
    rospy.spin()


if __name__ == '__main__':
    run()