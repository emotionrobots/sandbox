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
from vfacerec.msg import PublishFaceMsg
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

def convertToStr(image):
    img_str = cv2.imencode('.jpg', image)[1].tostring()
    return img_str

test = False
nameD = ""
datasetD = ""
model = None
fisherModel = None
eigenModel = None
faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")
trainImages = []


pubRate = 30
rateCounter = 30

recognizedFaceName = []
recognizedFaceImg = []
recognizedFaceLLx = []
recognizedFaceLLy = []
recognizedFaceURx = []
recognizedFaceURy = []

unknownFaceName = []
unknownFaceLLx = []
unknownFaceLLy = []
unknownFaceURx = []
unknownFaceURy = []

depthData = ""
nameDepthDir = ""

def faceRecInit():
    model_filename = os.getcwd() + '/model.pkl'

    video_capture = cv2.VideoCapture(0)

    faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")

    if not os.path.exists(model_filename):
        print 'Model does not exist, please train at least 1 person'
        sys.exit()

def faceRec(data, depthData):
    frameshow = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    depthFrame = np.fromstring(depthData.data, dtype=np.uint8).reshape(480, 640)
    global rateCounter
    if rateCounter == 0:
        #ret, frame = video_capture.read()
        frame = frameshow #np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
        img = cv2.resize(frame, (frame.shape[1]/2, frame.shape[0]/2), interpolation = cv2.INTER_CUBIC)
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        imgeqh = cv2.equalizeHist(gray)
        faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
        unknownfaceCount = 1
        for(x, y, w, h) in faces:
            cv2.rectangle(img, (x, y), (x+w, y+h), (255, 0, 0), 2)
            roi_gray = imgeqh[y:y+h, x:x+w]
            roi_color = img[y:y+h, x:x+w]
            #face = cv2.resize(roi_gray, image_size, interpolation = cv2.INTER_CUBIC)
            face = cv2.resize(roi_gray, image_size, interpolation = cv2.INTER_CUBIC)

            #leave here for testing
            prediction = model.predict(face)
            predicted_label = prediction[0]
            classifier_output = prediction[1]
            # Now let's get the distance from the assuming a 1-Nearest Neighbor.
            # Since it's a 1-Nearest Neighbor only look take the zero-th element:
            distance = classifier_output['distances'][0]

            cv2.putText(img, str(distance), (20,20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)

            #print p2[0]
            if distance <= 1700.0:
                cv2.rectangle(img, (x,y),(x+w,y+h),(0,255,0),2)
                n = model.subject_names[predicted_label]
                n =  n.replace("_", " ")
                cv2.putText(img, n, (x+20,y-20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
                global recognizedFaceName
                boo = n in recognizedFaceName
                if boo is False:
                    recognizedFaceName.append(n)
                    global recognizedFaceImg
                    recognizedFaceImg.append(convertToStr(face))
                    global recognizedFaceURx
                    recognizedFaceURx.append(x+w)
                    global recognizedFaceURy
                    recognizedFaceURy.append(y)
                    global recognizedFaceLLx
                    recognizedFaceLLx.append(x)
                    global recognizedFaceLLy
                    recognizedFaceLLy.append(y+h)

                print model.subject_names[predicted_label]
                print distance

            if distance > 1700.0:
                global unknownFaceName
                unknownFaceName.append("unknown-" + str(unknownfaceCount))
                print "unknown-"+ str(unknownfaceCount)
                global unknownFaceURx
                unknownFaceURx.append(x+w)
                global unknownFaceURy
                unknownFaceURy.append(y)
                global unknownFaceLLx
                unknownFaceLLx.append(x)
                global unknownFaceLLy
                unknownFaceLLy.append(y+h)
                unknownfaceCount = unknownfaceCount + 1

                print "Unknown"
            #else:
            #    cv2.rectangle(img, (x,y),(x+w,y+h),(0,0,255),2)

        

        img2 = cv2.resize(img, (img.shape[1]*2, img.shape[0]*2), interpolation = cv2.INTER_CUBIC)

        #cv2.imshow('Recognizer', img2)
        frameshow = img2
        print "showing rec frame"
        #cv2.namedWindow("Rec", cv2.WINDOW_NORMAL)

        rateCounter = pubRate
        return True
    else:
        rateCounter = rateCounter - 1
    
    ch = cv2.waitKey(1)

    if ch == 27:
        return False
    cv2.imshow('Recognizer', frameshow)
    cv2.namedWindow("Recognizer", cv2.WINDOW_NORMAL)
    return True

def callback_rgb(data):
    #frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    flg = faceRec(data, depthData)
    if(flg == False):
        rospy.signal_shutdown("")
    #else:
    #    flg = faceDet2(data)
    #    if(flg == False):
    #        rospy.signal_shutdown("")
    #cv2.imshow('Frame', frame)
    #cv2.waitKey(3)

def callback_depth(data):
    #frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)
    global depthData
    depthData = data


def listener():
    rospy.init_node('listener')
    rospy.Subscriber('depth', String, callback_depth)
    rospy.Subscriber('rgb', String, callback_rgb)
    #rospy.Subscriber('gesture', String, callback_gest)
    rospy.spin()

def faceListener():
    rospy,init_node('face_listener')
    rospy.Subscriber("known_faces", PublishFaceMsg, callback_facelist)

    rospy.spin()

def callback_facelist(data):
    faceList = data;

def handle_set_rate(req):
    print "Setting Rate to %s"%(req)
    global pubRate
    pubRate = req
    global rateCounter
    rateCounter = 0;
    return SetRateResponse(req)
    
def handle_set_name(req):
    print "Setting Name to %s"%(req)
    return SetNameResponse(req)

def handle_forget_name(req):
    print "Forgetting Name %s"%(req)
    #Delete folder with name
    return ForgetNameResponse(req)

def init_servers():
    rospy.init_node('set_rate_server')
    s = rospy.Service('set_rate', SetRate, handle_set_rate)
    
    rospy.init_node('set_name_server')
    s2 = rospy.Service('set_name', SetName, handle_set_name)

    rospy.init_node('forget_name_server')
    s3 = rospy.Service('forget_name', ForgetName, handle_forget_name)

    rospy.init_node('learn_name_server')
    s3 = rospy.Service('learn_name', LearnName, handle_forget_name)
    
    print "Initialized Servers"
    rospy.spin()

# def setName(face_id, name):

# def reset():

# def forget(face_id):

# def learn(face_id, name):

def getStatus():
    return 0

def publisher():
    global face_pub
    global face_array
    face_pub = rospy.Publisher('known_faces', PublishFaceMsg, queue_size=10)
    face_array = PublishFaceMsg()
    while not rospy.is_shutdown():
        publishRFI()
    #unknown_pub = rospy.Publisher('unknown_faces', UnknownFace, queue_size=10)
    #recFaceName_pub = rospy.Publisher('recFaceName', String, queue_size=10)
    #recFaceLL_pub = rospy.Publisher('recFaceLL', geometry_msgs.msg.Point, queue_size=10)
    #recFaceUR_pub = rospy.Publisher('recFaceUR', geometry_msgs.msg.Point, queue_size=10)
    #rate = rospy.Rate(pubRate)
    #rate.sleep()

def publishRFI():
    #for each face in array
    #publish info and face
    #remove from array
    print "call"


    for i in range(len(recognizedFaceName)):
        face_msg = Face()
        face_msg.name = recognizedFaceName[i]
        face_msg.image = recognizedFaceImg[i]
        face_msg.llx = recognizedFaceLLx[i]
        face_msg.lly = recognizedFaceLLy[i]
        face_msg.urx = recognizedFaceURx[i]
        face_msg.ury = recognizedFaceURy[i]
        face_array.known_faces.append(face_msg)

    for i in range(len(unknownFaceName)):
        face_msg_unknown = UnknownFace()
        face_msg_unknown.name = unknownFaceName[i]
        face_msg_unknown.llx = unknownFaceLLx[i]
        face_msg_unknown.lly = unknownFaceLLy[i]
        face_msg_unknown.urx = unknownFaceURx[i]
        face_msg_unknown.ury = unknownFaceURy[i]
        face_array.unknown_faces.append(face_msg_unknown)

    print "te"
    face_pub.publish(face_array)

    #global recognizedFaceName
    del recognizedFaceName[:]
    #global recognizedFaceImg
    del recognizedFaceImg[:]
    #global recognizedFaceLLx
    del recognizedFaceLLx[:]
    #global recognizedFaceLLy
    del recognizedFaceLLy[:]
    #global recognizedFaceURx
    del recognizedFaceURx[:]
    #global recognizedFaceURy
    del recognizedFaceURy[:]

    #global unknownFaceName
    del unknownFaceName[:]
    #global unknownFaceLLx
    del unknownFaceLLx[:]
    #global unknownFaceLLy
    del unknownFaceLLy[:]
    #global unknownFaceURx
    del unknownFaceURx[:]
    #global unknownFaceURy
    del unknownFaceURy[:]

    #recFaceImg_pub.publish()

# #### Main loop

    #rate.sleep()

if __name__ == '__main__':
    global datasetDir
    datasetDir = os.getcwd()+'/dataset'
    print "test"
    global faceCascade
    faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")
    #if(c()):
    global model
    model_filename = os.getcwd() + '/model.pkl'
    model = load_model(model_filename)

    #fisherModel = createFisherModel()
    #eigenModel = createEigenModel()

    test = True
    #else:
    #    faceDet()
    #    trainImages = []
    #    test = False
        #createPKL(image_size)
    listener()
    print "ddd"
    publisher()
    init_servers()
    #if(c()):
        #faceDet()
    #else
    #main()

#main()
