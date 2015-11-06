#!/usr/bin/env python2


import cv2
import numpy as np
import pyasm
import math
from sklearn.svm import SVC
from sklearn.externals import joblib
import skimage.io as io
from skimage import img_as_ubyte
from skimage.color import rgb2gray 
import logging
import os
import landmark

global start
start = True
global feature_vector
feature_vector = np.array([[1], [1]])
emotions = [[], [], [], [], [], [], []]

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
			# print y
			image = cv2.imread("./EmotionDatabase/" + files[y], cv2.IMREAD_GRAYSCALE)
			try:
				image=rgb2gray(image)	
				image=img_as_ubyte(image)
			except IOError, exc:
				logging.error(exc.message, exc_info=True)
				raise IOError
			landmarks = pyasm.STASM().s_search_single(image)
			landmark.draw_landmarks(image, landmarks, False)
			landmark.draw_face_outline(image, landmarks)
			landmark.draw_lefteye(image, landmarks)
			landmark.draw_lefteyebrow(image, landmarks)
			landmark.draw_righteye(image, landmarks)
			landmark.draw_righteyebrow(image, landmarks)
			landmark.draw_nosebridge(image, landmarks)
			landmark.draw_nose(image, landmarks)
			landmark.draw_mouth(image, landmarks)
			cv2.namedWindow('test', cv2.WINDOW_AUTOSIZE)
			cv2.imshow('test', image)
			emotions[y%7] = findEmotion(landmarks, image) 
			# print emotions		
			cv2.waitKey(10)
		addAnother(emotions)

def trainSVM():
	global feature_vector
	SVM = SVC(probability=True)
	emotion_list = np.array(['neutral', 'happy', 'sad', 'anger', 'disgust', 'fear', 'surprise'] * (len(feature_vector)/7))
	# print feature_vector
	feature_vector = feature_vector * 500
	emotion_list = emotion_list.tolist() * 500
	# print feature_vector
	# print emotion_list
	SVM.fit(feature_vector, emotion_list)
	joblib.dump(SVM, "emotionData.bin", compress=3)


def findEmotion(landmarks, frame):

	scale = landmark.normalize(frame, landmarks)
	# Happy
	pt59 = landmarks[59]
	pt65 = landmarks[65]
	distCornersMouth  = ((pt65[0] - pt59[0])**2 + (pt65[1] - pt59[1])**2) ** .5
	distCornersMouth = distCornersMouth * scale

	# Surprise
	pt17 = landmarks[17]
	pt38 = landmarks[38]
	distEyebrowToEye = ((pt38[0] - pt17[0])**2 + (pt38[1] - pt17[1])**2) ** .5
	distEyebrowToEye = distEyebrowToEye * scale

	# Disgust (Dist btw nose and eyes
	pt58 = landmarks[58]
	pt38 = landmarks[38]
	distLeftEyeNose = ((pt58[0] - pt38[0])**2 + (pt58[1] - pt38[1])**2) ** .5
	distLeftEyeNose = distLeftEyeNose * scale
	pt54 = landmarks[54]
	pt39 = landmarks[39]
	distRightEyeNose = ((pt54[0] - pt39[0])**2 + (pt54[1] - pt39[1])**2) ** .5
	distRightEyeNose = distRightEyeNose * scale
	avgDistEyeNose = (distLeftEyeNose + distRightEyeNose)/2 

	# Dist btw eyebrows for anger
	pt21 = landmarks[21]
	pt22 = landmarks[22]
	distEyebrow = ((pt22[0] - pt21[0]) ** 2 + (pt22[1] - pt21[1])**2)**.5
	distEyebrow = distEyebrow * scale
	# print distEyebrow

	# Dist bot eye to top eye for fear
	pt32 = landmarks[32]
	pt35 = landmarks[35]
	distbotEyeToTopEyelidLeft = ((pt35[0] - pt32[0])**2 + (pt35[1]-pt32[1]) ** 2) **.5
	pt39 = landmarks[39]
	pt47 = landmarks[47]
	distbotEyeToTopEyelidRight = ((pt47[0] - pt39[0])**2 + (pt47[1]-pt39[1]) ** 2) **.5
	distbotEyeToTopEyelidLeft = distbotEyeToTopEyelidLeft * scale
	distbotEyeToTopEyelidRight = distbotEyeToTopEyelidRight * scale
	avgDistbotEyetoTopEye = (distbotEyeToTopEyelidRight + distbotEyeToTopEyelidLeft)/2

	# Dist inner eyebrow to mid eye for sadness
	pt20 = landmarks[20]
	pt22 = landmarks[22]
	pt38 = landmarks[38]
	pt39 = landmarks[39]
	distInsideLeft = ((pt38[0] - pt20[0])**2 + (pt38[1] - pt20[1])**2) **.5
	distInsideLeft = distInsideLeft * scale
	distInsideRight = ((pt39[0]-pt22[0])**2 + (pt39[1] - pt22[1])**2)**.5
	distInsideRight = distInsideRight * scale
	avgInside = (distInsideRight + distInsideLeft)/2

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
