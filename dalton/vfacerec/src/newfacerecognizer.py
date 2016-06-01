#!/usr/bin/env python

import cv2, os
import numpy as np
import os
import sys
import time
import rospy
from std_msgs.msg import String
# from geometry_msgs.msg import Point
from vfacerec.msg import Face
from vfacerec.msg import UnknownFace
from vfacerec.srv import SetRate

#Cascade file
faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")

lbphrec = cv2.createLBPHFaceRecognizer()
eigenrec = cv2.createEigenFaceRecognizer()
fisherrec = cv2.createFisherFaceRecognizer()

# Contains the number IDs
labelNums = []
# Contains the string names
labelNames = []
# Image size
image_size = (100, 100)
# Utils Path
utilPath = os.getcwd()+'/utils'
# Feature flags
enableChatBotInput = False

def loadIDLabels():
    if os.path.exists(utilPath+'/labels.txt'):
        print "Labels file exists"
        with open(utilPath+'/labels.txt') as f:
            for line in f:
                ln = line.split(" ")
                for a in ln:
                    if(len(a) > 1):
                        namenum = a.split(",")
                        labelNums.append(namenum[0])
                        labelNames.append(namenum[1])

def writeIDLabels():
    with open(utilPath+'/labels.txt', 'w') as text_file:
        for i in range(len(labelNums)):
            text_file.write(str(labelNums[i])+","+str(labelNames[i])+" ")

def prepareExit():
    cv2.destroyAllWindows()
    rospy.signal_shutdown("")
    
def initRec():
    print "Initializing Face Recognition..."
    lbphrec.load(utilPath+"/recModelLBPH.xml")

def recognizer(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    img = cv2.resize(frame, (frame.shape[1]/2, frame.shape[0]/2), interpolation = cv2.INTER_CUBIC)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    imgeqh = cv2.equalizeHist(gray)
    faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
    for(x, y, w, h) in faces:
        cv2.rectangle(img, (x-2, y-2), (x+w+2, y+h+2), (255, 0, 0), 2)
        roi = imgeqh[y:y+h, x:x+w]
        face = cv2.resize(roi, image_size, interpolation = cv2.INTER_CUBIC)

    cv2.putText(img, "Press 'ESC' to finish training", (20,20), cv2.FONT_HERSHEY_COMPLEX, .5, 255)
    img2 = cv2.resize(img, (img.shape[1]*2, img.shape[0]*2), interpolation = cv2.INTER_CUBIC)
    cv2.imshow('Trainer', img2)
    cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)

    ch = cv2.waitKey(1)
    if ch == 27:
        return False
    return True

def callback_rgb(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    flg = recognizer(data)
    # If user ended trainer
    if(flg == False):
        prepareExit()

def callback_name(data):
    print "Received name from chat bot: " + data.data

def listener():
    # Name of this listener node
    rospy.init_node('listener')
    # Subscribe to RGB frame
    rospy.Subscriber('rgb', String, callback_rgb)
    # Subscribe to name from chat bot (for training)
    if(enableChatBotInput):
        rospy.Subscriber('username', String, callback_name)

    rospy.spin()

def writeImagesToFile(trainImages, nameDir):
    index = 0
    if not os.path.exists(nameDir):
        os.makedirs(nameDir)
    for img in trainImages:
        cv2.imwrite(datasetPath+'/'+username+'_'+time.strftime("%d-%m-%Y") + '_' + time.strftime("%H:%M:%S")+'_'+str(index)+'.png', img)
        index+=1


if not os.path.exists(utilPath):
    os.makedirs(utilPath)
loadIDLabels()
initTrainer()
listener()
