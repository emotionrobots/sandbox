#!/usr/bin/env python2
# -*- coding: utf-8 -*-

# """In this example we can see how to call 
# the s_search_single fuction of STASM from Python 
# with a video and save the landmarks into txt"""

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
import rospy
from std_msgs.msg import String

FILENAME =  '/tmp/out.webm'
frames=None
global odd
odd = 10
global sameSpeed
sameSpeed = True

def video_config():
	"""Initialize video capture, pass filename by
	param jic that remove var and pass by argv"""
	cap = cv2.VideoCapture(0)
	while not cap.isOpened():
	    cap = cv2.VideoCapture(0)
	    cv2.waitKey(10)
	    print "Wait for the header"
	pos_frame = cap.get(cv2.cv.CV_CAP_PROP_POS_FRAMES)
	
	return cap, pos_frame

def draw_seg(frame, landmarks, start, end):
	(startx, starty) = landmarks[start]
	(endx, endy) = landmarks[end]
        cv2.line(frame,(int(startx),int(starty)),(int(endx),int(endy)),(0,0,255), 1)
        cv2.line(frames,(int(startx),int(starty)),(int(endx),int(endy)),(0,0,255), 1)
	return

def draw_arc(frame, landmarks, start, end):
	count = 0
        (tempx, tempy) = landmarks[start]
	for (x, y) in landmarks[start+1:end+1]:
            cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
            cv2.line(frames,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
            count=count+1
            tempx=x
            tempy=y
	return

def draw_loop(frame, landmarks, start, end):
        count = 0
        (tempx, tempy) = landmarks[start]
	for (x, y) in landmarks[start+1:end+1]:
            cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
            cv2.line(frames,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
            count=count+1
            tempx=x
            tempy=y
        (x, y) = landmarks[start]
        cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
        cv2.line(frames,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
	return

def draw_righteyebrow(frame, landmarks):
	return draw_loop(frame, landmarks, 16, 21)

def draw_lefteyebrow(frame, landmarks):
	return draw_loop(frame, landmarks, 22, 27)

def draw_righteye(frame, landmarks):
	return draw_loop(frame, landmarks, 30, 37)

def draw_lefteye(frame, landmarks):
	return draw_loop(frame, landmarks, 40, 47)

def draw_nose(frame, landmarks):
	return draw_loop(frame, landmarks, 51, 58)

def draw_mouth(frame, landmarks):
	draw_arc(frame, landmarks, 59, 68)
	draw_seg(frame, landmarks, 59, 68)
 	draw_seg(frame, landmarks, 59, 69)
 	draw_arc(frame, landmarks, 69, 71)
 	draw_seg(frame, landmarks, 65, 71)
	draw_seg(frame, landmarks, 65, 72)
	draw_arc(frame, landmarks, 72, 76)
	draw_seg(frame, landmarks, 59, 76)
	return

def draw_nosebridge(frame, landmarks):
	return draw_arc(frame, landmarks, 48, 50)

def draw_landmarks(frame, landmarks):
	scale = normalize(frame, landmarks)
	cv2.putText(frame, "Scale:  "  + str(scale), (100,100), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
	cv2.putText(frames, "Scale:  "  + str(scale), (100,100), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
	map(lambda p: cv2.circle(frame, (int(p[0]), int(p[1])), 1, (512,512,255), -1), landmarks)
	map(lambda p: cv2.circle(frames, (int(p[0]), int(p[1])), 1, (512,512,255), -1), landmarks)
	count=0
	numbering = 0
        for (x, y) in landmarks:
			# if count == 52 or count == 14 or count == 13 or count == 15 or count == 65 or count == 59:
			if count == 1111111:
				if count != 52:
					xcoor = (x - landmarks[52][0])*scale
					ycoor = (landmarks[52][1] - y)*scale
					quadrant = 0
					if xcoor > 0 and ycoor > 0:
						quadrant = 1
					if xcoor < 0 and ycoor > 0:
						quadrant = 2
	       			if xcoor < 0 and ycoor < 0:
	       				quadrant = 3
	       			if xcoor > 0 and ycoor < 0:
	       				quadrant = 4
	       			dist = (xcoor**2 + ycoor**2)**.5
	       			Xdist14toPoint = (x - landmarks[14][0]) * scale
	       			Ydist14toPoint = (landmarks[14][1] - y) * scale
	       			absDist14toPoint = (Xdist14toPoint**2 + Ydist14toPoint**2)**.5
	       			stdDistX = (landmarks[52][0] - landmarks[14][0]) * scale
	       			stdDistY = (landmarks[52][1] - landmarks[14][1]) * scale
	       			stdDist = (stdDistX**2 + stdDistY**2)**.5
	       			theta = (absDist14toPoint**2 - dist**2 - stdDist**2)/(-2* dist * stdDist)
	       			theta = math.degrees(math.acos(theta))
	       			if quadrant == 2 or quadrant == 3:
						theta = 90+theta
						xcoor = dist * math.cos(math.radians(theta))
						theta = theta - 90
	       			if quadrant == 4:
	       				theta = 450 - theta
	       				xcoor = dist * math.cos(math.radians(theta))
	       				theta = -1 * theta + 450
	       			if quadrant == 1:	
	       				xcoor = dist * math.cos(math.radians(90-theta))
	       			ycoor = dist * math.sin(math.radians(90 - theta))
	       			# print ycoor
				if count == 52:
					xcoor = 0
					ycoor = 0
				# print str(xcoor) + "  " + str(ycoor)
				coorPair = str(count) + ":   (" + str(round(xcoor, 2)) + ", " + str(round(ycoor, 2)) + ")"
				cv2.putText(frame, str(count), (int(x)+5,int(y)+5), cv2.FONT_HERSHEY_SIMPLEX, .4, 255)
				cv2.putText(frame, coorPair, (100, 115 + numbering * 20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
				cv2.putText(frames, str(count), (int(x)+5,int(y)+5), cv2.FONT_HERSHEY_SIMPLEX, .4, 255)
				cv2.putText(frames, coorPair, (100, 115 + numbering * 20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
				numbering = numbering + 1
			count = count + 1
	return

def displayEmotion(frame, landmarks):
	featureVector = joblib.load("emotionData.bin")
	featureVector.set_params(probability=True)
	detect = detectEmotion(frame, landmarks)
	myEmotion = str(featureVector.predict(detect))
	probArr = featureVector.predict_proba(detect)
	probArr = probArr * 100
	probability = "Neutral: " + str(probArr[0][4]) + "  Happy: " + str(probArr[0][3]) + "  Sad: " + str(probArr[0][5]) + "  Angry: " + str(probArr[0][0])
	probability2 = "Disgust: " + str(probArr[0][1]) + "  Fear: " + str(probArr[0][2]) + "  Surprise: " + str(probArr[0][6])
	cv2.putText(frame, myEmotion, (700, 50), cv2.FONT_HERSHEY_SIMPLEX, .6, 255)
	cv2.putText(frames, myEmotion, (550, 50), cv2.FONT_HERSHEY_SIMPLEX, .6, 255)
	global odd
	if odd % 2 == 1:
		cv2.putText(frame, probability, (25, 390), cv2.FONT_HERSHEY_SIMPLEX, .45, 255)
		cv2.putText(frames, probability, (25, 390), cv2.FONT_HERSHEY_SIMPLEX, .45, 255)	
		cv2.putText(frame, probability2, (25, 410), cv2.FONT_HERSHEY_SIMPLEX, .45, 255)
		cv2.putText(frames, probability2, (25, 410), cv2.FONT_HERSHEY_SIMPLEX, .45, 255)
	odd = odd + 1
	publisher(myEmotion, probArr.max())
	return myEmotion

def detectEmotion(frame, landmarks):
	# Returns an Array containing each feature vector
	scale = normalize(frame, landmarks)
		
	# Happiness (Dist btw corners of mouth)
	pt59 = landmarks[59]
	pt65 = landmarks[65]
	distCornersMouth  = ((pt65[0] - pt59[0])**2 + (pt65[1] - pt59[1])**2) ** .5
	distCornersMouth = distCornersMouth * scale
	# print distCornersMouth

	# Surprise (Dist btw eyebrows and eyes)
	pt17 = landmarks[17]	
	pt38 = landmarks[38]
	distEyebrowToEye = ((pt38[0] - pt17[0])**2 + (pt38[1] - pt17[1])**2) ** .5
	distEyebrowToEye = distEyebrowToEye * scale
	# print distEyebrowToEye

	# Disgust (Dist btw nose and eyes)
	
	# Left Eye
	pt58 = landmarks[58]
	pt38 = landmarks[38]
	distLeftEyeNose = ((pt58[0] - pt38[0])**2 + (pt58[1] - pt38[1])**2) ** .5
	distLeftEyeNose = distLeftEyeNose * scale
	# Right Eye
	pt54 = landmarks[54]
	pt39 = landmarks[39]
	distRightEyeNose = ((pt54[0] - pt39[0])**2 + (pt54[1] - pt39[1])**2) ** .5
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
	pt32 = landmarks[32]
	pt35 = landmarks[35]
	distbotEyeToTopEyelidLeft = ((pt35[0] - pt32[0])**2 + (pt35[1]-pt32[1]) ** 2) **.5
	pt39 = landmarks[39]
	pt47 = landmarks[47]
	distbotEyeToTopEyelidRight = ((pt47[0] - pt39[0])**2 + (pt47[1]-pt39[1]) ** 2) **.5
	distbotEyeToTopEyelidLeft = distbotEyeToTopEyelidLeft * scale
	distbotEyeToTopEyelidRight = distbotEyeToTopEyelidRight * scale
	avgDistbotEyetoTopEye = (distbotEyeToTopEyelidRight + distbotEyeToTopEyelidLeft)/2
	# print avgDistMidEyetoTopEye

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

def draw_face_outline(frame, landmarks):
        return draw_loop(frame, landmarks, 0, 15)

def normalize(frame, landmarks):
		mid = 14
		left = 13
		right = 15
		arrayRM = landmarks[mid] - landmarks[left]
		arrayLM = landmarks[right] - landmarks[mid]
		distRM = arrayRM[0]**2 + arrayRM[1]**2
		distRM = distRM**.5
		distLM = arrayLM[0]**2 + arrayLM[1]**2
		distLM = distLM**.5
		distTOTAL = distLM + distRM
		scale = 50 / distTOTAL
		return scale

def OverlayImage(src, x):
	
    if x=="['happy']":
        overlay=filename+"/emoji/happy.png"
    if x=="['neutral']":
        overlay=filename+"/emoji/neutral.png"
    if x=="['surprise']":
        overlay=filename+"/emoji/surprise.jpg"
    if x=="['disgust']":
        overlay=filename+"/emoji/disgust.jpg"
    if x=="['anger']":
        overlay=filename+"/emoji/anger.jpg"
    if x=="['fear']":
        overlay=filename+"/emoji/fear.png"
    if x=="['sad']":
        overlay=filename+"/emoji/sad.png"
    l_img = src
    s_img = cv2.imread(overlay,)
    s_img=cv2.resize(s_img,(150,150))
    x_offset=550
    y_offset=70
    l_img[y_offset:y_offset+s_img.shape[0], x_offset:x_offset+s_img.shape[1]] = s_img
    return l_img

def draw_face(frame, landmarks, notTraining):
	draw_landmarks(frame, landmarks)
	draw_face_outline(frame, landmarks)
	draw_lefteye(frame, landmarks)
	draw_lefteyebrow(frame, landmarks)
	draw_righteye(frame, landmarks)
	draw_righteyebrow(frame, landmarks)
	draw_nosebridge(frame, landmarks)
	draw_nose(frame, landmarks)
	draw_mouth(frame, landmarks)
	if notTraining == True:
		x = displayEmotion(frame, landmarks)
		return x
	else:
		detectEmotion(frame, landmarks)
	return
	
def publisher(myEmotion, prob):
	global sameSpeed
	pub = rospy.Publisher('landmark', String,queue_size=1)
	# rospy.init_node('emotionpub', anonymous=True)
	msg=String()
	msg.data= myEmotion + ", " + str(prob)
	r = rospy.Rate(1)
	if not rospy.is_shutdown() and sameSpeed == True:
		pub.publish(msg)
		# sameSpeed = False
		print str(myEmotion)+" "+str(prob)

		# r.sleep()
	# pub.publish(msg)

def callback(data):
	global sameSpeed
	if data.data == "True":
		sameSpeed = True

		

	#print emotion +" "+ str(emotionamts) +" "+str(pos)

def listener():
	rospy.init_node('sameSpeed', anonymous=True)
	rospy.Subscriber("pygameFace", String, callback)

def main():
	global filename
	filename = os.getcwd()
	test = 1
	mystasm = pyasm.STASM()
	cap, pos_frame = video_config()
	done = False
	start = True	
	while done != True:
		listener()
		flag, frame = cap.read()
		global frames
		frames=cv2.imread("white.jpg",1)
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
			if start == True:# and test % 3 == 1:
				mylandmarks = mystasm.s_search_single(image)
				start = False
			if start == False:# and test % 3 == 1:
				landmarksOLD = mylandmarks
				mylandmarks = mystasm.s_search_single(image)
				alpha = .85
				mylandmarks = (1-alpha)* landmarksOLD + alpha * mylandmarks
			# draw the landmarks point as circles
			if  mylandmarks[0][0] != 0.0:
				x=draw_face(frame, mylandmarks, True)
				frame=OverlayImage(frame,x)
	        
			x=draw_face(frame, mylandmarks, True)
			frame=OverlayImage(frame,x)
			cv2.namedWindow("Live Landmarking", cv2.WINDOW_NORMAL)
			cv2.namedWindow('k', cv2.WINDOW_NORMAL)
			cv2.imshow("Live Landmarking", frame)
			cv2.imshow('k',frames)	
			# if cv2.waitKey(150) == 1048603: 
				# done = True 
			cv2.waitKey(10)	
		test = test + 1
	
if __name__ == '__main__':
		main()