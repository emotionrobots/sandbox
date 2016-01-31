#!/usr/bin/env python

import rospy
import numpy
from cv_bridge import CvBridge, CvBridgeError
from skeleton.msg import face_p
import cv2
from std_msgs.msg import String
from skeleton.msg import CustomString

bridge=CvBridge()

def landmark(data):
    #face_points2=numpy.asarray(data.arr.data)
    points = []
    for x in range(0,len(data.arr.data)):
       points.append(data.arr.data[x])
    print points
    numface=(len(points)+1)/136
    print numface   
    #try:
        #cv_image = bridge.imgmsg_to_cv2(data.image, "bgr8")
        #cv2.putText(cv_image, str(data.header.stamp), (100,100), cv2.FONT_HERSHEY_DUPLEX, 1, (0,0,255),1)
        #cv2.imshow("points2",cv_image)
    #except CvBridgeError as e:
    #    print(e)
    #map(lambda p: cv2.circle(cv_image, (int(p[0]), int(p[1])), 1, (512,512,255), -1), points)
    cv2.circle(frame, (100, 100), 1, (512,512,255), -1)
    
    cv2.putText(frame, str(timer), (100,100), cv2.FONT_HERSHEY_DUPLEX, 1, (0,0,255),1)
    cv2.imshow("points",frame)
    
    if cv2.waitKey(1) & 0xFF == ord('q'):
        return
    

def callback_rgb(data):
    global frame
    global timer
    timer=data.header.stamp
    #timer= rospy.Time.to_time(timer)
    frame = numpy.fromstring(data.data, dtype=numpy.uint8).reshape(480, 640, 3)




def listener():
    rospy.Subscriber('rgb', CustomString, callback_rgb)
    rospy.Subscriber('face_points', face_p, landmark)
    rospy.spin()


if __name__ == '__main__':
	rospy.init_node('listenerp', anonymous=True)
	listener()
