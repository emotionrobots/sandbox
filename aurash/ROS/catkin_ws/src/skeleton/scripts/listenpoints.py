#!/usr/bin/env python

import rospy
import numpy
from cv_bridge import CvBridge, CvBridgeError
from skeleton.msg import face_p


def landmark(data):
    face_points2=numpy.asarray(data.array)
    try:
        cv_image = self.bridge.imgmsg_to_cv2(data.image, "bgr8")
    except CvBridgeError as e:
        print(e)







def listener():
    rospy.Subscriber('face_points', face_p, landmark)
    rospy.spin()





if __name__ == '__main__':
	rospy.init_node('listenerp', anonymous=True)
	listener()