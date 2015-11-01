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
files.sort()
for x in range(0, len(files)):	
	img = cv2.imread("./EmotionDatabase/" + files[x], cv2.IMREAD_GRAYSCALE)
	try:
		img=rgb2gray(img)	
		img=img_as_ubyte(img)
	except IOError, exc:
		logging.error(exc.message, exc_info=True)
		raise IOError
	landmarks = pyasm.STASM().s_search_single(img)
	landmark.draw_landmarks(img, landmarks)
	landmark.draw_face_outline(img, landmarks)
	landmark.draw_lefteye(img, landmarks)
	landmark.draw_lefteyebrow(img, landmarks)
	landmark.draw_righteye(img, landmarks)
	landmark.draw_righteyebrow(img, landmarks)
	landmark.draw_nosebridge(img, landmarks)
	landmark.draw_nose(img, landmarks)
	landmark.draw_mouth(img, landmarks)
	cv2.namedWindow('test', cv2.WINDOW_AUTOSIZE)
	cv2.imshow('test', img)
	cv2.waitKey(0)
	print files[x]
	print landmark.detectEmotion(img, landmarks)
	print landmark.displayEmotion(img, landmarks) 