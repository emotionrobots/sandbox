#!/usr/bin/env python

import cv2, os
import numpy as np
from PIL import Image
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

# Newly trained images stored here
trainImages = []
# Contains the number IDs
labelNums = []
# Contains the string names
labelNames = []
# Image size
image_size = (100, 100)
# User's name (sent through chat bot node or from user input in console)
username = ""
# Dataset Path
datasetPath = os.getcwd()+'/dataset'
# Utils Path
utilPath = os.getcwd()+'/utils'
# Counter for frames
trainCount = 0
# Rate for delayed capture
trainRate = 5

# Feature flags
enableChatBotInput = False

def read_images(path, sz=None):
    c = 0
    X,y = [], []
    for dirname, dirnames, filenames in os.walk(path):
        for subdirname in dirnames:
            subject_path = os.path.join(dirname, subdirname)
            for filename in os.listdir(subject_path):
                try:
                    im = cv2.imread(os.path.join(subject_path, filename), cv2.IMREAD_GRAYSCALE)
                    # resize to given size (if given)
                    if (sz is not None):
                        im = cv2.resize(im, sz)
                    X.append(np.asarray(im, dtype=np.uint8))
                    y.append(c)
                except IOError, (errno, strerror):
                    print "I/O error({0}): {1}".format(errno, strerror)
                except:
                    print "Unexpected error:", sys.exc_info()[0]
                    raise
            c = c+1
    return [X,y]

def get_images_and_labels(path):
    # Append all the absolute image paths in a list image_paths
    # We will not read the image with the .sad extension in the training set
    # Rather, we will use them to test our accuracy of the training
    image_paths = [os.path.join(path, f) for f in os.listdir(path) if not f.endswith('.sad')]
    # images will contains face images
    images = []
    # labels will contains the label that is assigned to the image
    labels = []
    for image_path in image_paths:
        # Read the image and convert to grayscale
        print image_path
        #image_pil = Image.open(image_path).convert('L')
        im = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
        im = cv2.resize(im, image_size)
        # Convert the image format into numpy array
        img = np.asarray(im, dtype=np.uint8)
        print img.size
        print img.shape
        print img.dtype
        # Get the name label of the image
        strLabel = str((os.path.split(image_path)[1].split("_")[0]))
        # Get the index of number ID of the name
        nbrIndex = labelNames.index(strLabel)
        # Get the number ID of the name
        nbr = int(labelNums[nbrIndex])
        # Print for debugging
        print nbr
        # Detect the face in the image
        faces = faceCascade.detectMultiScale(img)
        # If face is detected, append the face to images and the label to labels
        for (x, y, w, h) in faces:
            images.append(img[y: y + h, x: x + w])
            labels.append(nbr)
            cv2.imshow("Adding faces to traning set...", img[y: y + h, x: x + w])
    # return the images list and labels list
    return images, labels

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
    print "Saving images..."
    print utilPath
    writeImagesToFile(trainImages, datasetPath)
    print datasetPath
    images, labels = get_images_and_labels(datasetPath)
    cv2.destroyAllWindows()
    lbphrec.train(np.asarray(images), np.asarray(labels))
    lbphrec.save(utilPath+"/recModelLBPH.xml")
    #eigenrec.train(np.asarray(images), np.asarray(labels))
    #eigenrec.save(utilPath+"/recModelEigen.xml")
    #fisherrec.train(np.asarray(images), np.asarray(labels))
    #fisherrec.save(utilPath+"/recModelFisher.xml")
    rospy.signal_shutdown("")
    
def initTrainer():
    print "Initializing Face Training..."
    global username
    if(enableChatBotInput == False):
        name = str(raw_input('What is your name? \n name:'))
        username = name.replace(" ", ".")
        print username in labelNames
        if((username in labelNames) == False):
            if(len(labelNums) == 0):
                lastID = 0
                labelNums.append(lastID)
            else:
                lastID = labelNums[-1]
                labelNums.append(int(lastID)+1)
            labelNames.append(username)
            writeIDLabels()

def trainer(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    img = cv2.resize(frame, (frame.shape[1]/2, frame.shape[0]/2), interpolation = cv2.INTER_CUBIC)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    imgeqh = cv2.equalizeHist(gray)
    faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
    for(x, y, w, h) in faces:
        cv2.rectangle(img, (x-2, y-2), (x+w+2, y+h+2), (255, 0, 0), 2)
        roi = imgeqh[y:y+h, x:x+w]
        face = cv2.resize(roi, image_size, interpolation = cv2.INTER_CUBIC)
    if len(faces) == 1:
        global trainCount
        global trainRate
        global trainImages
        if trainCount == trainRate:
            print "Click!"
            trainImages.append(face)
            trainCount = 0
        else:
            trainCount = trainCount + 1
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
    flg = trainer(data)
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
