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
global ready
ready = False
global endProgram
endProgram = False

def key(event):
	global picFile
	global endProgram
	global emoCount
	global ready
	if ready == True:
		for x in range(0,9):
			flag, frameG = cap.read()
		temp = frameG.copy()
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
				if emoCount == 0:
					cv2.imwrite("./EmotionDatabase/"+picFile + "_1N.jpg", temp)
				if emoCount == 1:
					cv2.imwrite("./EmotionDatabase/" +picFile + "_2H.jpg", temp)
				if emoCount == 2:
					cv2.imwrite("./EmotionDatabase/" +picFile + "_3S.jpg", temp)
				if emoCount == 3:
					cv2.imwrite("./EmotionDatabase/" +picFile + "_4A.jpg", temp)
				if emoCount == 4:
					cv2.imwrite("./EmotionDatabase/" +picFile + "_5D.jpg", temp)
				if emoCount == 5:
					cv2.imwrite("./EmotionDatabase/" +picFile + "_6F.jpg", temp)
				if emoCount == 6:
					cv2.imwrite("./EmotionDatabase/" +picFile + "_7S.jpg", temp)
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

def main():
	global filename
	filename = os.getcwd()
	frame1.bind("<Key>", key)
	frame1.pack()
	frame1.focus_set()
	mystasm = pyasm.STASM()
	global cap
	global pos_frame
	frames=cv2.imread("./white.jpg",1)
	cap, pos_frame = landmark.video_config()
	done = False
	start = True
	while done != True:
		# global flag
		# flag, frame = cap.read()
		# if flag:
	 #        # The frame is ready and already captured
	 #        # save a tmp file because pystasm receive by parameter a filename
		# 	try:
		# 		image=rgb2gray(frame)
		# 		image=img_as_ubyte(image)
		# 	except IOError, exc:
		# 		logging.error(exc.message, exc_info=True)
		# 		raise IOError 
	 #        #cv2.imwrite(filename, frame)
	 #        # nasty fix .. pystasm should receive np array .. 
		# 	if start == True: #and test % 3 == 1:
		# 		mylandmarks = mystasm.s_search_single(image)
		# 		start = False
		# 	if start == False: #and test % 3 == 1:
		# 		landmarksOLD = mylandmarks
		# 		mylandmarks = mystasm.s_search_single(image)
		# 		alpha = .85
		# 		mylandmarks = (1-alpha)* landmarksOLD + alpha * mylandmarks
		# 	landmark.draw_face(frame, mylandmarks, False)
			cv2.namedWindow("Live Landmarking", cv2.WINDOW_OPENGL)
			if ready == False:
				cv2.putText(frames, "If you are ready to begin, please make a neutral face", (15,200), cv2.FONT_HERSHEY_COMPLEX, .70, 255)
				cv2.putText(frames, "and press the space bar", (200,250), cv2.FONT_HERSHEY_COMPLEX, .70, 255)
				cv2.imshow("Live Landmarking", frames)
			# else:
				# cv2.imshow("Live Landmarking", frame)
			cv2.resizeWindow("Live Landmarking", 1000,  1000)
			cv2.waitKey(30)
			# global landmarksG
			# landmarksG = mylandmarks
			# global frameG
			# frameG = frame
			global picFile
			picFile = raw_input("Save images as?")
			t2 = Thread(target = keyListen())
			t2.start()
			if endProgram == True:
				cv2.destroyAllWindows()
				done = True

if __name__ == '__main__':
	main()
