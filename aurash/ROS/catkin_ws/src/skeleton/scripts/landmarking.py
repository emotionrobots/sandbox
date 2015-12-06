#!/usr/bin/env python
import sys
import dlib
import cv2
import numpy as np
import Image    
import rospy
import cv2
from std_msgs.msg import String
import time

global count
count=0
global start
start=time.time()

predictor_path = "/home/aurash/dlib/examples/build/shape_predictor_68_face_landmarks.dat"


detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor(predictor_path)
#win = dlib.image_window()



def landmark(data):
    # Capture frame-by-frame
    img = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    global count
    global start
    count=count+1
    fps=count/(time.time()-start)
    list=[]
    print fps
    temp=img
    b,g,r = cv2.split(img)
    img = cv2.merge([r,g,b])
     #Ask the detector to find the bounding boxes of each face. The 1 in the
     #second argument indicates that we should upsample the image 1 time. This
    # will make everything bigger and allow us to detect more faces.
    dets = detector(img, 1)
    #print("Number of faces detected: {}".format(len(dets)))
    for i in dets:
        shape = predictor(img, i)
        for a in xrange(68):
            b=shape.part(a)
            list.append((b.x,b.y))
        #print list
        map(lambda p: cv2.circle(temp, (int(p[0]), int(p[1])), 1, (512,512,255), -1), list)
        cv2.imshow("frame",temp)
        ch = cv2.waitKey(3)
        if ch == 27:
            rospy.signal_shutdown("")
            sys.exit()
        return list
        #win.clear_overlay()
        #win.add_overlay(shape)
        #win.set_image(img)    



def listener():
    rospy.Subscriber('rgbs', String, landmark)
    rospy.spin()

def publisher():
    rospy.init_node('listener', anonymous=True)
    pub = rospy.Publisher('face_points', String, queue_size=10)
    rate = rospy.Rate(60) # 10hz
    while not rospy.is_shutdown():
        points=listener()
        pub.publish(points)
        rate.sleep()
        




if __name__ == '__main__':
    publisher()
    

