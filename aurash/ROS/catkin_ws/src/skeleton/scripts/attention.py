#!/usr/bin/env python
from pocketsphinx import *
from sphinxbase import *
import os, sys
import pyaudio
import time
import cv2
import sys
import nltk
from nltk import pos_tag, word_tokenize
import rospy
from std_msgs.msg import String
from std_msgs.msg import Int8

def face_detect():
    cascPath = "/home/aurash/cascades/haarcascade_frontalface_default.xml"
    eye_cascade = cv2.CascadeClassifier('/home/aurash/cascades/haarcascade_eye.xml')
    faceCascade = cv2.CascadeClassifier(cascPath)
    smile_cascade=cv2.CascadeClassifier('/home/aurash/cascades/haarcascade_smile.xml')
    nose_cascade=cv2.CascadeClassifier('/home/aurash/cascades/haarcascade_mcs_nose.xml')

    video_capture = cv2.VideoCapture(0)
    found=False
    while found==False:
        # Capture frame-by-frame
        ret, frame = video_capture.read()

        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        faces = faceCascade.detectMultiScale(
            gray,
            scaleFactor=1.2,
            minNeighbors=5,
            minSize=(30, 30),
            flags=cv2.cv.CV_HAAR_SCALE_IMAGE
        )



        # Draw a rectangle around the faces
        for (x, y, w, h) in faces:
            cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
            roi_gray = gray[y:y+h, x:x+w]
            roi_color = frame[y:y+h, x:x+w]
            eyes = eye_cascade.detectMultiScale(roi_gray,1.1,50,minSize=(20, 20),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
            for (ex,ey,ew,eh) in eyes:
                cv2.rectangle(roi_color,(ex,ey),(ex+ew,ey+eh),(0,255,0),2)
            smile = smile_cascade.detectMultiScale(roi_gray, scaleFactor=1.3, minNeighbors=500, minSize=(20, 20), flags=cv2.cv.CV_HAAR_SCALE_IMAGE)    
            smile = smile_cascade.detectMultiScale(roi_gray,1.3,20)
            for (sx,sy,sw,sh) in smile:
                cv2.rectangle(roi_color,(sx,sy),(sx+sw,sy+sh),(255,255,0),2)
            nose = nose_cascade.detectMultiScale(roi_gray,1.1,5,minSize=(20, 20),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
            nose = nose_cascade.detectMultiScale(roi_gray,1.3,20)
            for (nx,ny,nw,nh) in nose:
                cv2.rectangle(roi_color,(nx,ny),(nx+nw,ny+nh),(0,255,255),2)



        # Display the resulting frame
        cv2.imshow('Video', frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

        if len(faces)>0 and len(eyes)>1 and len(smile)>0 and len(nose)>0:
            found=True
            video_capture.release()
            cv2.destroyAllWindows()
            return True


def speech():
    pub2 = rospy.Publisher('strtspch', String, queue_size=10)
    rospy.init_node('Attention')
    msg=String()
    msg=str("start1".lower())
    pub2.publish(msg)
    rospy.spin()  


                   
def engage():
        a=face_detect()
        if a:
            speech()




if __name__=='__main__':
    try:
    	while True:
            engage()
    except rospy.ROSInterruptException:
        pass
