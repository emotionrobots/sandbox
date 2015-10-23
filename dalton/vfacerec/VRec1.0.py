import cv2
import numpy as np
import os
import sys
import time

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

def main():
    print "Starting face detection"

    image_size = (100, 100)

    if len(sys.argv) > 1:
        startTrainOrRec = sys.argv[1]

        if startTrainOrRec == 'rec':
            faceRec(image_size)
            sys.exit()

    faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")

    datasetDir = os.getcwd()+'/dataset'

    video_capture = cv2.VideoCapture(0)

    name = str(raw_input('What is your name, human? \n name:'))

    name = name.replace(" ", "_")

    nameDir = datasetDir + '/' + name

    trainImages = []

    if not os.path.exists(nameDir):
        os.makedirs(nameDir)

    while True:
        ret, frame = video_capture.read()
        img = cv2.resize(frame, (frame.shape[1]/2, frame.shape[0]/2), interpolation = cv2.INTER_CUBIC)
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
        for(x, y, w, h) in faces:
            cv2.rectangle(img, (x, y), (x+w, y+h), (255, 0, 0), 2)
            roi_gray = gray[y:y+h, x:x+w]
            roi_color = img[y:y+h, x:x+w]

        ch = cv2.waitKey(10)

        if len(faces) == 1:

            if ch == ord('b'):
                print "Capturing & Cropping.."
                #coords = (x, y, w, h)
                #faceCropped = img.crop(coords) #cropFaceFromImage(img)
                trainImages.append(roi_color)
                print len(trainImages)

            #cv2.imshow('Trainer', frame)
            cv2.putText(img, "Press 'B' to take a picture", (x+20,y+20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
            #cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)
        
        cv2.putText(img, "Press 'ESC' to take finish training", (20,20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)

        img2 = cv2.resize(img, (img.shape[1]*2, img.shape[0]*2), interpolation = cv2.INTER_CUBIC)
        
        cv2.imshow('Trainer', img2)
        cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)

        if ch == 27:
            writeImagesToFile(trainImages, nameDir)
            createPKL(image_size)
            break

def writeImagesToFile(trainImages, nameDir):
    index = 0
    for img in trainImages:
        cv2.imwrite(nameDir+'/'+time.strftime("%d-%m-%Y") + '_' + time.strftime("%H:%M:%S")+'_'+str(index)+'.jpg', img)
        index+=1

def createPKL(image_size):
    [images, labels, subject_names] = read_images(os.getcwd()+'/dataset', image_size)
    list_of_labels = list(xrange(max(labels)+1))
    subject_dictionary = dict(zip(list_of_labels, subject_names))
    model = get_model(image_size=image_size, subject_names=subject_dictionary)
    print "Computing the model..."
    model.compute(images, labels)
    print "Saving the model..."
    save_model("model.pkl", model)

def faceRec(image_size):
    model_filename = os.getcwd() + '/model.pkl'

    video_capture = cv2.VideoCapture(0)

    faceCascade = cv2.CascadeClassifier("haarcascade_frontalface_alt2.xml")

    if not os.path.exists(model_filename):
        print 'Model does not exist, please train at least 1 person'
        sys.exit()

    model = load_model(model_filename)

    while True:
        ret, frame = video_capture.read()
        img = cv2.resize(frame, (frame.shape[1]/2, frame.shape[0]/2), interpolation = cv2.INTER_CUBIC)
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
        for(x, y, w, h) in faces:
            cv2.rectangle(img, (x, y), (x+w, y+h), (255, 0, 0), 2)
            roi_gray = gray[y:y+h, x:x+w]
            roi_color = img[y:y+h, x:x+w]
            face = cv2.resize(roi_gray, image_size, interpolation = cv2.INTER_CUBIC)

            #pred = model.predict(face)

            prediction = model.predict(face)
            predicted_label = prediction[0]
            classifier_output = prediction[1]
            # Now let's get the distance from the assuming a 1-Nearest Neighbor.
            # Since it's a 1-Nearest Neighbor only look take the zero-th element:
            distance = classifier_output['distances'][0]

            #print p2[0]
            cv2.rectangle(img, (x,y),(x+w,y+h),(0,255,0),2)
            if distance < 10.0:
                cv2.rectangle(img, (x,y),(x+w,y+h),(0,255,0),2)
                cv2.putText(img, model.subject_names[predicted_label], (x+20,y-20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
                print model.subject_names[predicted_label]
            #else:
            #    cv2.rectangle(img, (x,y),(x+w,y+h),(0,0,255),2)

        ch = cv2.waitKey(10)

        img2 = cv2.resize(img, (img.shape[1]*2, img.shape[0]*2), interpolation = cv2.INTER_CUBIC)

        cv2.imshow('Trainer', img2)
        cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)

        if ch == 27:
            break
main()