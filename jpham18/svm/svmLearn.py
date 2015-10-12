#!/usr/bin/env python2

import cv2
import numpy as np
import pyasm
import math
from sklearn.svm import SVC
from sklearn.externals import joblib 
import landmark
from Tkinter import *
from threading import Thread
import tkMessageBox
from skimage import img_as_ubyte
from skimage.color import rgb2gray 
import logging
import os

root = Tk()
frame1 = Frame(root, width=100, height=100)
global emoCount
emoCount = 0
neut = []
hap = []
sad = []
ang = []
disg = []
fear = []
surp = []
global emotions
emotions = [neut, hap, sad, ang, disg, fear, surp]
global ready
ready = False
global endProgram
endProgram = False

def key(event):
	# global filename
	# filename = os.getcwd()
	global endProgram
	global emoCount
	global ready
	if ready == True:
		for x in range(0,9):
			flag, frameG = cap.read()
		try:
			image=rgb2gray(frameG)	
			image=img_as_ubyte(image)
		except IOError, exc:
			logging.error(exc.message, exc_info=True)
			raise IOError 
		landmarksG = pyasm.STASM().s_search_single(image)	
		# print landmarksG
		landmark.draw_face(frameG, landmarksG, False)
		# print "test"
		cv2.imshow("Live Landmarking", frameG)	
	   	if event.char == ' ' and emoCount < 7:
			if emoCount == 0:
				print "Please make a Happy face and press space"
			if emoCount == 1:
				print "Please make a Sad face and press space"
			if emoCount == 2:
				print "Please make a Angry face and press space"
			if emoCount == 3:
				print "Please make a Disgusted face and press space"
			if emoCount == 4:
				print "Please make a Fearful face and press space"
			if emoCount == 5:
				print "Please make a Surprised face and press space"
			addToEmo = tkMessageBox.askyesno("Keep Frame?", "Would you like to save the current frame?")
			if addToEmo == True:
		   		emotions[emoCount] = findEmotion(landmarksG, frameG)
	   			emoCount = emoCount + 1
	   		else:
	   			for x in range(0,9):
		   			flag, frameG = cap.read()
			cv2.waitKey(25)
	   	if emoCount == 7:
	   		root.destroy()
	   		endProgram = True
	else:
		ready = True		

def keyListen():
	root.mainloop()

def trainSVM():
	feature_vector = np.array([ emotions[0], emotions[1], emotions[2], emotions[3], emotions[4], emotions[5], emotions[6] ])
	emotion_list = np.array(['neutral', 'happy', 'sad', 'anger', 'disgust', 'fear', 'surprise'])
	SVM = SVC()
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
	print distEyebrow

	# Dist mid eye to top eye for fear
	pt32 = landmarks[32]
	pt38 = landmarks[38]
	distmidEyeToTopEyelidLeft = ((pt38[0] - pt32[0])**2 + (pt38[1]-pt32[1]) ** 2) **.5
	pt39 = landmarks[39]
	pt42 = landmarks[42]
	distmidEyeToTopEyelidRight = ((pt42[0] - pt39[0])**2 + (pt42[1]-pt39[1]) ** 2) **.5
	distmidEyeToTopEyelidLeft = distmidEyeToTopEyelidLeft * scale
	distmidEyeToTopEyelidRight = distmidEyeToTopEyelidRight * scale
	avgDistMidEyetoTopEye = (distmidEyeToTopEyelidRight + distmidEyeToTopEyelidLeft)/2
	print avgDistMidEyetoTopEye

	return [distCornersMouth, distEyebrowToEye, avgDistEyeNose, distEyebrow, avgDistMidEyetoTopEye]

def main():
	global filename
	filename = os.getcwd()
	frame1.bind("<Key>", key)
	frame1.pack()
	frame1.focus_set()
	mystasm = pyasm.STASM()
	global cap
	global pos_frame
	frames=cv2.imread("/home/julian/sandbox/aurash/landmarking/white.jpg",1)
	cap, pos_frame = landmark.video_config()
	done = False
	start = True
	while done != True:
		global flag
		flag, frame = cap.read()
		if flag:
	        # The frame is ready and already captured
	        # save a tmp file because pystasm receive by parameter a filename
			try:
				image=rgb2gray(frame)
				image=img_as_ubyte(image)
			except IOError, exc:
				logging.error(exc.message, exc_info=True)
				raise IOError 
	        #cv2.imwrite(filename, frame)
	        # nasty fix .. pystasm should receive np array .. 
			if start == True: #and test % 3 == 1:
				mylandmarks = mystasm.s_search_single(image)
				start = False
			if start == False: #and test % 3 == 1:
				landmarksOLD = mylandmarks
				mylandmarks = mystasm.s_search_single(image)
				alpha = .85
				mylandmarks = (1-alpha)* landmarksOLD + alpha * mylandmarks
			landmark.draw_face(frame, mylandmarks, False)
			cv2.namedWindow("Live Landmarking", cv2.WINDOW_OPENGL)
			if ready == False:
				cv2.putText(frames, "If you are ready to begin, please make a neutral face", (15,200), cv2.FONT_HERSHEY_COMPLEX, .70, 255)
				cv2.putText(frames, "and press the space bar", (200,250), cv2.FONT_HERSHEY_COMPLEX, .70, 255)
				cv2.imshow("Live Landmarking", frames)
			# else:
				# cv2.imshow("Live Landmarking", frame)
			cv2.resizeWindow("Live Landmarking", 1000,  1000)
			cv2.waitKey(30)
			global landmarksG
			landmarksG = mylandmarks
			global frameG
			frameG = frame
			t2 = Thread(target = keyListen())
			t1 = Thread(target = trainSVM())
			t2.start()
			t1.start()
			if endProgram == True:
				cv2.destroyAllWindows()
				done = True


if __name__ == '__main__':
	main()
