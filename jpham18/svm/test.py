#!/usr/bin/env python2


import numpy as np
import os
import pyasm
import cv2
from skimage import img_as_ubyte
from skimage.color import rgb2gray 
import logging
import os
import landmark
import svmLearn

files = os.listdir("./EmotionDatabase")
# image = cv2.imread("./testface.jpg", cv2.IMREAD_GRAYSCALE)
filename = os.getcwd
files.sort()
# print files
count = len(files) / 7
for x in range(0, count):
	svmLearn.emotions = [[], [], [], [], [], [], []]
	for y in range(7 * x, 7 * (x+1)):
		print y
		image = cv2.imread("./EmotionDatabase/" + files[y], cv2.IMREAD_GRAYSCALE)
		# print y
		# print image
		try:
			image=rgb2gray(image)	
			image=img_as_ubyte(image)
		except IOError, exc:
			logging.error(exc.message, exc_info=True)
			raise IOError
		landmarks = pyasm.STASM().s_search_single(image)
		landmark.draw_landmarks(image, landmarks)
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
		svmLearn.emotions[y%7] = svmLearn.findEmotion(landmarks, image) 
		print svmLearn.emotions		
		cv2.waitKey(0)
	svmLearn.trainSVM()

# asdf = joblib.load("emotionData.bin")