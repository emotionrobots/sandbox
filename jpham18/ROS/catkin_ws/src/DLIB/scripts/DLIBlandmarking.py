#!/usr/bin/env python2
# -*- coding: utf-8 -*-

"""In this example we can see how to call 
the s_search_single fuction of STASM from Python 
with a video and save the landmarks into txt"""

import cv2
import numpy as np
import pyasm
import math
from sklearn.svm import SVC
from sklearn.externals import joblib
import skimage.io as io
# from skimage import img_as_ubyte
# from skimage.color import rgb2gray 
import logging
import os
import rospy
from std_msgs.msg import String
from DLIB.msg import face_p

FILENAME =  '/tmp/out.webm'
frames=None
global odd
odd = 1

def video_config():
	"""Initialize video capture, pass filename bya
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

def draw_landmarks(frame, landmarks, printScale):	
	# print landmarks
	scale = normalize(landmarks)
	if printScale:	
		cv2.putText(frame, "Scale:  "  + str(scale), (100,100), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
		cv2.putText(frames, "Scale:  "  + str(scale), (100,100), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
	map(lambda p: cv2.circle(frame, (int(p[0]), int(p[1])), 1, (512,512,255), -1), landmarks)
	count=0
	numbering = 0
        for (x, y) in landmarks:
			if count != 30:
				xcoor = (x - landmarks[30][0])*scale
				ycoor = (landmarks[30][1] - y)*scale
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
       			Xdist27toPoint = (x - landmarks[27][0]) * scale
       			Ydist27toPoint = (landmarks[27][1] - y) * scale
       			absDist27toPoint = (Xdist27toPoint**2 + Ydist27toPoint**2)**.5
       			stdDistX = (landmarks[30][0] - landmarks[27][0]) * scale
       			stdDistY = (landmarks[30][1] - landmarks[27][1]) * scale
       			stdDist = (stdDistX**2 + stdDistY**2)**.5
       			theta = (absDist27toPoint**2 - dist**2 - stdDist**2)/(-2* dist * stdDist)
       			if abs(theta - 1) < .0000001:
       				theta = 0
       			elif abs(theta - (-1)) < .0000001:
       				theta = 180
       			else:
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
			if count == 30:
				xcoor = 0
				ycoor = 0
			numbering = numbering + 1
			# cv2.putText(frame, str(count), ((x+5),(y+5)), cv2.FONT_HERSHEY_SIMPLEX,.3,255)
			count = count + 1
	return

def displayEmotion(landmarks, frame):
	featureVector = joblib.load("emotionDataBase.bin")
	featureVector.set_params(probability=True)
	# print featureVector
	detect = detectEmotion(landmarks)
	# print detect
	myEmotion = str(featureVector.predict(detect))
	# print myEmotion	+ "   " + str(x)
	probArr = featureVector.predict_proba(detect)
	probArr = probArr * 100
	# print landmarks
	# print myEmotion
	# print probArr
	# probability = "Neutral: " + str(probArr[0][4]) + "  Happy: " + str(probArr[0][3]) + "  Sad: " + str(probArr[0][5]) + "  Angry: " + str(probArr[0][0])
	# probability2 = "Disgust: " + str(probArr[0][1]) + "  Fear: " + str(probArr[0][2]) + "  Surprise: " + str(probArr[0][6])
	cv2.putText(frame, myEmotion, (mylandmarks[0][0] - 50, mylandmarks[0][1]-50), cv2.FONT_HERSHEY_SIMPLEX, .6, 255)
	# cv2.putText(frames, myEmotion, (550, 50), cv2.FONT_HERSHEY_SIMPLEX, .6, 255)
	publisher(myEmotion, probArr.max())
	return #[myEmotion, probArr]

def detectEmotion(landmarks):
	# Returns an Array containing each feature vector
	scale = normalize(landmarks)
		
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

	return [distCornersMouth, distEyebrowToEye, avgDistEyeNose, distEyebrow, avgDistbotEyetoTopEye, avgInside]

def draw_face_outline(frame, landmarks):
        return draw_loop(frame, landmarks, 0, 15)

def normalize(landmarks):
		# print landmarks
		mid = 28
		left = 0
		right = 16
		arrayRM = landmarks[mid] - landmarks[left]
		arrayLM = landmarks[right] - landmarks[mid]
		distRM = arrayRM[0]**2 + arrayRM[1]**2
		distRM = distRM**.5
		distLM = arrayLM[0]**2 + arrayLM[1]**2
		distLM = distLM**.5
		distTOTAL = distLM + distRM
		if distTOTAL != 0:
			scale = 50 / distTOTAL
		else:
			scale = 0
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

def draw_face(landmarks, frame):
	draw_landmarks(frame, landmarks, False)
	# draw_face_outline(frame, landmarks)
	# draw_lefteye(frame, landmarks)
	# draw_lefteyebrow(frame, landmarks)
	# draw_righteye(frame, landmarks)
	# draw_righteyebrow(frame, landmarks)
	# draw_nosebridge(frame, landmarks)
	# draw_nose(frame, landmarks)
	# draw_mouth(frame, landmarks)
	# if notTraining == True:
		# x = displayEmotion(frame, landmarks)
		# return x
	# else:
		# detectEmotion(frame, landmarks)
	# Returns
	displayEmotion(landmarks, frame)

def main():
	listener()
	
def publisher(myEmotion, prob):
	pub = rospy.Publisher('emotion', String,queue_size=1)
	# rospy.init_node('emotionpub', anonymous=True)
	msg=String()
	msg.data= myEmotion + ", " + str(prob)
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)

def listener():

	rospy.init_node('listener')
	rospy.Subscriber("rgb", String, callback)
	rospy.Subscriber("face_points", face_p, callback2)
	rospy.spin()
	cv2.namedWindow("Live Landmarking", cv2.WINDOW_NORMAL)


def callback(data):
	global frame
	frame = np.fromstring(data.data, dtype=np.uint8).reshape(480, 640, 3)



def callback2(data):

	global mylandmarks
	mylandmarks = []
	alllandmarks = np.array(data.arr.data)
	global filename
	filename = os.getcwd()
	done = False
	# print len(alllandmarks)
	# print alllandmarks
	global frame
	numFaces = len(alllandmarks) / 135
	faceArr = [];
	for num in range (0,numFaces):
		faceArr.append([])
		for num2 in range(0,135):
			print num2 +(num * 135)
			faceArr[num].append(alllandmarks[num2 +(num * 135)])
	# print "faceArr" + str(faceArr)	
	count = 0
	for arr in faceArr:
		print arr
		for x in range(0,len(arr) - 2):
			if count % 2 == 0:
				if x % 2 == 0:
					mylandmarks = mylandmarks + [[arr[x],arr[x+1]]]
			else:
				if x % 2 == 0:
					mylandmarks = mylandmarks + [[arr[x+1],arr[x]]]
		count = count + 1
		# print (mylandmarks)
		# print "person" + str(y)
		mylandmarks = np.array(mylandmarks)
		# print "mylandmarks " + str(mylandmarks)
		draw_face(mylandmarks, frame)
		mylandmarks = []
		# cv2.putText(frame, "person: " + str(y + 1), (), cv2.FONT_HERSHEY_SIMPLEX, .45, 255)
	cv2.imshow("Live Landmarking", frame)
	cv2.waitKey(1)



if __name__ == '__main__':
    main()
