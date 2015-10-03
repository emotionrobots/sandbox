import cv2
import numpy



def main():
    joe0=cv2.imread("/home/aurash/stasm4.1.0/data/aurian.jpg")
    joe1=cv2.imread("/home/aurash/stasm4.1.0/data/aurian5.jpg")
    sam0=cv2.imread("/home/aurash/stasm4.1.0/data/aurian1.jpg")
    sam1=cv2.imread("/home/aurash/stasm4.1.0/data/aurian2.jpg")
    recognizer=cv2.createLBPHFaceRecognizer()
    trainingImages = [joe0, joe1, sam0, sam1]
    trainingLabels = numpy.array([0, 0, 1, 1])
    recognizer.train(trainingImages, trainingLabels)
    testLabel, distance=recognizer.predict("/home/aurash/aurian.JPG")
    print testLabel
    print "hello"





main()