#!/usr/bin/env python2


import cv2
import numpy as np
import dlib
import math
from sklearn.svm import SVC
from sklearn.externals import joblib
import skimage.io as io
from skimage import img_as_ubyte
from skimage.color import rgb2gray 
import logging
import os
import DLIBlandmarking

global start
start = True
global feature_vector
feature_vector = np.array([[1], [1]])
emotions = [[], [], [], [], [], [], []]
detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor("/home/julian/dlib/examples/build/shape_predictor_68_face_landmarks.dat")

def main():
	print "\n\nProgram is running, please wait for it to complete\n\n"
	files = os.listdir("./EmotionDatabase")
	filename = os.getcwd
	files.sort()
	# print files
	count = len(files) / 7

	if len(files) % 7 != 0:
		print "Need full set of pictures"
		return
	for x in range(0, count):
		emotions = [[], [], [], [], [], [], []]
		for y in range(7 * x, 7 * (x+1)):	
			list = []
			start = True
			image = cv2.imread("./EmotionDatabase/" + files[y], cv2.IMREAD_GRAYSCALE)
			dets = detector(image, 0)
			if(start == False):
				list = list.tolist()
			for i in dets:
				shape = predictor(image, i)
  				for a in xrange(68):
					b=shape.part(a)
					list.append([b.x,b.y])
			list = np.array(list)
			start = False
			DLIBlandmarking.draw_landmarks(image, list, False)	
			print list
			emotions[y%7] = findEmotion(list) 
			# print emotions		
			cv2.namedWindow("test", cv2.WINDOW_NORMAL)
			cv2.imshow("test", image)
			cv2.waitKey(1)
		addAnother(emotions)

def trainSVM():
	print "test1"
	global feature_vector
	SVM = SVC(probability=True)
	emotion_list = np.array(['neutral', 'happy', 'sad', 'anger', 'disgust', 'fear', 'surprise'] * (len(feature_vector)/7))
	print "test2"
	feature_vector = feature_vector * 200
	print "test3"
	emotion_list = emotion_list.tolist() * 200
	print "test4"
	print feature_vector
	print len(feature_vector)
	print emotion_list
	print len(emotion_list)
	SVM.fit(feature_vector, emotion_list)
	print "test5"
	joblib.dump(SVM, "emotionDataBase.bin", compress=3)
	print "test6"


def findEmotion(landmarks):
# Returns an Array containing each feature vector
	scale = DLIBlandmarking.normalize(landmarks)
		
	# Happiness (Dist btw corners of mouth)
	pt48 = landmarks[48]
	pt54 = landmarks[54]
	distCornersMouth  = ((pt54[0] - pt48[0])**2 + (pt54[1] - pt48[1])**2) ** .5
	distCornersMouth = distCornersMouth * scale
	# print distCornersMouth

	# Surprise (Dist btw eyebrows and eyes)
	pt19 = landmarks[19]	
	pt37 = landmarks[37]
	distEyebrowToEyeLeft = ((pt37[0] - pt19[0])**2 + (pt37[1] - pt19[1])**2) ** .5
	distEyebrowToEyeLeft = distEyebrowToEyeLeft * scale
	pt24 = landmarks[24]	
	pt44 = landmarks[44]
	distEyebrowToEyeRight = ((pt44[0] - pt24[0])**2 + (pt44[1] - pt24[1])**2) ** .5
	distEyebrowToEyeRight = distEyebrowToEyeRight * scale

	distEyebrowToEye = (distEyebrowToEyeLeft + distEyebrowToEyeRight) /2
	# print distEyebrowToEye

	# Disgust (Dist btw nose and eyes)
	
	# Left Eye
	pt30 = landmarks[30]
	pt39 = landmarks[39]
	distLeftEyeNose = ((pt30[0] - pt39[0])**2 + (pt30[1] - pt39[1])**2) ** .5
	distLeftEyeNose = distLeftEyeNose * scale
	# Right Eye
	pt30 = landmarks[30]
	pt42 = landmarks[42]
	distRightEyeNose = ((pt30[0] - pt42[0])**2 + (pt30[1] - pt42[1])**2) ** .5
	distRightEyeNose = distRightEyeNose * scale
	avgDistEyeNose = (distLeftEyeNose + distRightEyeNose)/2
	# print avgDistEyeNose

	# Dist btw eyebrows for anger
	pt21 = landmarks[21]
	pt22 = landmarks[22]
	distEyebrow = ((pt22[0] - pt21[0]) ** 2 + (pt22[1] - pt21[1])**2)**.5
	distEyebrow = distEyebrow * scale
	# print distEyebrow
 
	# Dist bot eye to top eye for fear
	pt37 = landmarks[37]
	pt41 = landmarks[41]
	distbotEyeToTopEyelidLeft = ((pt41[0] - pt37[0])**2 + (pt41[1]-pt37[1]) ** 2) **.5
	pt43 = landmarks[43]
	pt47 = landmarks[47]
	distbotEyeToTopEyelidRight = ((pt47[0] - pt43[0])**2 + (pt47[1]-pt43[1]) ** 2) **.5
	distbotEyeToTopEyelidLeft = distbotEyeToTopEyelidLeft * scale
	distbotEyeToTopEyelidRight = distbotEyeToTopEyelidRight * scale
	avgDistbotEyetoTopEye = (distbotEyeToTopEyelidRight + distbotEyeToTopEyelidLeft)/2
	# print avgDistMidEyetoTopEye

	# Dist inner eyebrow to mid eye for sadness
	pt21 = landmarks[21]
	pt22 = landmarks[22]
	pt39 = landmarks[39]
	pt42 = landmarks[42]
	distInsideLeft = ((pt39[0] - pt21[0])**2 + (pt39[1] - pt21[1])**2) **.5
	distInsideLeft = distInsideLeft * scale
	distInsideRight = ((pt42[0]-pt22[0])**2 + (pt42[1] - pt22[1])**2)**.5
	distInsideRight = distInsideRight * scale
	avgInside = (distInsideRight + distInsideLeft)/2

	print [distCornersMouth, distEyebrowToEye, avgDistEyeNose, distEyebrow, avgDistbotEyetoTopEye, avgInside]
	return [distCornersMouth, distEyebrowToEye, avgDistEyeNose, distEyebrow, avgDistbotEyetoTopEye, avgInside]

def addAnother(emoArr):
	global start
	global feature_vector
	# print emoArr
	sum = feature_vector
	for x in range(0, len(emoArr)):
		if start == True:
			sum = [emoArr[x]]
			start = False
		else:
			sum = sum + [emoArr[x]]
		# print emoArr[x]
	feature_vector = sum

if __name__ == '__main__':
	main()

trainSVM()
