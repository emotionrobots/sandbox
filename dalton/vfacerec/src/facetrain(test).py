#!/usr/bin/env python

import cv2
import numpy as np
import os
import sys
import time

import numpy as np
import rospy
import cv2
from std_msgs.msg import String
# from geometry_msgs.msg import Point
from vfacerec.msg import Face
from vfacerec.msg import UnknownFace
from vfacerec.srv import SetRate
#from vfacerec.srv import SetName

from facerec.model import PredictableModel
from facerec.feature import Fisherfaces
from facerec.distance import EuclideanDistance
from facerec.classifier import NearestNeighbor
from facerec.validation import KFoldCrossValidation
from facerec.serialization import save_model, load_model


class ExtendedPredictableModel(PredictableModel):
    """ Subclasses the PredictableModel to store some more
        information, so we don't need to pass the dataset
        on each program call...
    """

    def __init__(self, feature, classifier, image_size, subject_names):
        PredictableModel.__init__(self, feature=feature, classifier=classifier)
        self.image_size = image_size
        self.subject_names = subject_names

def get_model(image_size, subject_names):
    """ This method returns the PredictableModel which is used to learn a model
        for possible further usage. If you want to define your own model, this
        is the method to return it from!
    """
    # Define the Fisherfaces Method as Feature Extraction method:
    feature = Fisherfaces()
    # Define a 1-NN classifier with Euclidean Distance:
    classifier = NearestNeighbor(dist_metric=EuclideanDistance(), k=1)
    # Return the model as the combination:
    return ExtendedPredictableModel(feature=feature, classifier=classifier, image_size=image_size, subject_names=subject_names)

def read_subject_names(path):
    """Reads the folders of a given directory, which are used to display some
        meaningful name instead of simply displaying a number.

    Args:
        path: Path to a folder with subfolders representing the subjects (persons).

    Returns:
        folder_names: The names of the folder, so you can display it in a prediction.
    """
    folder_names = []
    for dirname, dirnames, filenames in os.walk(path):
        for subdirname in dirnames:
            folder_names.append(subdirname)
    return folder_names

def read_images(path, image_size=None):
    """Reads the images in a given folder, resizes images on the fly if size is given.

    Args:
        path: Path to a folder with subfolders representing the subjects (persons).
        sz: A tuple with the size Resizes 

    Returns:
        A list [X, y, folder_names]

            X: The images, which is a Python list of numpy arrays.
            y: The corresponding labels (the unique number of the subject, person) in a Python list.
            folder_names: The names of the folder, so you can display it in a prediction.
    """
    c = 0
    X = []
    y = []
    folder_names = []
    for dirname, dirnames, filenames in os.walk(path):
        for subdirname in dirnames:
            folder_names.append(subdirname)
            subject_path = os.path.join(dirname, subdirname)
            for filename in os.listdir(subject_path):
                try:
                    im = cv2.imread(os.path.join(subject_path, filename), cv2.IMREAD_GRAYSCALE)
                    # resize to given size (if given)
                    if (image_size is not None):
                        im = cv2.resize(im, image_size)
                    X.append(np.asarray(im, dtype=np.uint8))
                    y.append(c)
                except IOError, (errno, strerror):
                    print "I/O error({0}): {1}".format(errno, strerror)
                except:
                    print "Unexpected error:", sys.exc_info()[0]
                    raise
            c = c+1
    return [X,y,folder_names]


image_size = (100, 100)
trainRate = 20
trainCount = 0
brightnessTest = int(1)


def faceDet2(data):
    nameDir = nameD
    global trainImages
    global brightnessTest
    global trainCount
    #ret, frame = video_capture.read()
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    img = cv2.resize(frame, (frame.shape[1]/2, frame.shape[0]/2), interpolation = cv2.INTER_CUBIC)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    imgeqh = cv2.equalizeHist(gray)
    faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
    for(x, y, w, h) in faces:
        cv2.ellipse(img, (x-2, y-2), (x+w+2, y+h+2), (255, 0, 0), 2)
        roi_gray = imgeqh[y:y+h, x:x+w]
        roi_color = img[y:y+h, x:x+w]

    ch = cv2.waitKey(1)

    if len(faces) == 1:

        if trainCount == trainRate:
            print "Capturing & Cropping.."
            #coords = (x, y, w, h)
            #faceCropped = img.crop(coords) #cropFaceFromImage(img)
            trainImages.append(roi_gray)
            #hsv = cv2.cvtColor(roi_gray, cv2.COLOR_RGB2HSV)
            #h, s, v = cv2.split
            print len(trainImages)
            trainCount = 0
        else:
        	trainCount = trainCount + 1

        #cv2.imshow('Trainer', frame)
        #cv2.putText(img, "Press 'B' to take a picture", (x+20,y-20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
        #cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)
    
    cv2.putText(img, "Press 'ESC' to take finish training", (20,20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)


    img2 = cv2.resize(img, (img.shape[1]*2, img.shape[0]*2), interpolation = cv2.INTER_CUBIC)
    alpha = float(1.0)
    mul_img = cv2.multiply(img2,np.array([alpha]))
    new_img = cv2.add(mul_img,np.array([brightnessTest]))  		

    cv2.imshow('Trainer', new_img)
    cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)

    if ch == 27:
        writeImagesToFile(trainImages, nameD)
        #createPKL(image_size)
        return False
    if ch == ord('c'):
    	brightnessTest-=10
    if ch == ord('v'):
    	brightnessTest+=10

    return True
     #   break

def faceDet():
    print "Starting Face Detection"

    image_size = (100, 100)

    faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")

    datasetDir = os.getcwd()+'/dataset'

    video_capture = cv2.VideoCapture(0)

    name = str(raw_input('What is your name, human? \n name:'))

    name = name.replace(" ", "_")

    nameDir = datasetDir + '/' + name

    #trainImages = []

    if not os.path.exists(nameDir):
        os.makedirs(nameDir)

    global nameD
    nameD = datasetDir + '/' + name
    global datasetD
    datasetD = os.getcwd()+'/dataset'
    print nameD

def writeImagesToFile(trainImages, nameDir):
    index = 0
    print nameD
    for img in trainImages:
        cv2.imwrite(nameDir+'/'+time.strftime("%d-%m-%Y") + '_' + time.strftime("%H:%M:%S")+'_'+str(index)+'.jpg', img)
        index+=1

def createPKL(image_size):
    [images, labels, subject_names] = read_images(os.getcwd()+'/dataset', image_size)
    list_of_labels = list(xrange(max(labels)+1))
    subject_dictionary = dict(zip(list_of_labels, subject_names))
    global model
    model = get_model(image_size=image_size, subject_names=subject_dictionary)
    print "Computing the model..."
    model.compute(images, labels)
    print "Saving the model..."
    save_model("model.pkl", model)

def callback_rgb(data):
    frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    flg = faceDet2(data)
    if(flg == False):
        rospy.signal_shutdown("")
        createPKL(image_size)
    #cv2.imshow('Frame', frame)
    #cv2.waitKey(3)

def listener():
    rospy.init_node('listener')
    rospy.Subscriber('rgb', String, callback_rgb)
    #rospy.Subscriber('depth', String, callback_depth)
    #rospy.Subscriber('gesture', String, callback_gest)
    rospy.spin()

if __name__ == '__main__':

    global faceCascade
    faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")
    global trainImages
    trainImages = []
    faceDet()
    test = False

    listener()
    #faceDet()
    test = False