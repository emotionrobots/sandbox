#!/usr/bin/env python2
# -*- coding: utf-8 -*-

"""In this example we can see how to call 
the s_search_single fuction of STASM from Python 
with a video and save the landmarks into txt"""

import cv2
import numpy as np
import pyasm
import skimage.io as io
from skimage import img_as_ubyte
from skimage.color import rgb2gray
import math
from sklearn.svm import SVC
from sklearn.externals import joblib 
import logging


FILENAME =  '/tmp/out.webm'
frames=None
def video_config():
	"""Initialize video capture, pass filename by
	param jic that remove var and pass by argv"""
	cap = cv2.VideoCapture(0)
	#cap.set(cv2.cv.CV_CAP_PROP_FPS, 60.0) 
	print "\t Framerate: ",cap.get(cv2.cv.CV_CAP_PROP_FPS)
	#cap.set(cv2.cv.CV_CAP_PROP_FRAME_HEIGHT, 1280) 
	#cap.set(cv2.cv.CV_CAP_PROP_FRAME_WIDTH, 720)
	#cap.set(11, 0)
	while not cap.isOpened():
	    cap = cv2.VideoCapture(0)
	    cap.set(cv2.CV_CAP_PROP_FPS, 60) 
	    #cap.set(cv2.CV_CAP_PROP_FRAME_HEIGHT, 1280) 
	    #cap.set(cv2.CV_CAP_PROP_FRAME_WIDTH, 720)
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
			if count == 52 or count == 14 or count == 13 or count == 15 or count == 65 or count == 59:
			# if count != -11111111:
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
				# cv2.putText(frame, str(scale * (landmarks[65][0] - landmarks[59][0])), (100,10), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
				# cv2.putText(frames, str(scale * (landmarks[65][0] - landmarks[59][0])), (100,10), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
			count = count + 1
	return
def displayEmotion(frame, landmarks):
	scale = normalize(frame, landmarks)

	# standard deltas on a Neutral Face
	stdNeutMouth = round(13.3, 0)
	stdNeutEyeBrow = round(7.5, 0)
	stdNeutNoseEye = round(13.7, 0)

	# standard deltas on a Happy Face
	stdHappMouth = round(18.5, 0)
	stdHappEyeBrow = round(7.9, 0)
	stdHappNoseEye = round(11.9, 0)

	# standard deltas on a Sad Face
	# stdSadMouth
	# stdSadEyeBrow
	# stdSadNoseEye
	
	# standard deltas on an Angry Face
	# stdAngryMouth
	# stdAngryEyeBrow = round(7, 0)
	# stdAngryNoseEye
	# stdAngryEyebrowAngle

	# standard deltas on a Disgusted Face
	stdDisgMouth = round(15.4, 0)
	stdDisgEyebrow = round(6.1, 0)
	stdDisgNoseEye = round(10.1, 0)

	# standard deltas on a Surprised Face
	stdSurpMouth = round(15.4, 0)
	stdSurpEyeBrow  = round(9.5, 0)
	stdSurpNoseEye = round(14.1 , 0)

	featureVector = np.array([[stdNeutMouth, stdNeutEyeBrow, stdNeutNoseEye], [stdHappMouth, stdHappEyeBrow, stdHappNoseEye],
	 [-9, 0, 0], [-9, 0, 0], [stdDisgMouth, stdDisgEyebrow, stdDisgNoseEye], [-9, 0, 0], [stdSurpMouth, stdSurpEyeBrow, stdSurpNoseEye]])
	emotion = np.array(['neutral', 'happy', 'sad', 'anger', 'disgust', 'fear', 'surprise'])
	clf = SVC()
	clf.fit(featureVector, emotion)
	currentEmotion = detectEmotion(frame, landmarks, scale)
	myEmotion = str(clf.predict(currentEmotion))
	print myEmotion
	cv2.putText(frame, myEmotion, (700, 50), cv2.FONT_HERSHEY_SIMPLEX, .6, 255)
	cv2.putText(frames, myEmotion, (550, 50), cv2.FONT_HERSHEY_SIMPLEX, .6, 255)
	return myEmotion

def detectEmotion(frame, landmarks, scale):
	# Returns an Array containing each feature vector in the following format
	# [Dist btw corners of mouth, Dist btw Eyebrows and eyes,]

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

	return [distCornersMouth, distEyebrowToEye, avgDistEyeNose]

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
		scale = 20 / distTOTAL
		return scale

def OverlayImage(src, x):
    if x=="['happy']":
        overlay='/home/aurash/emoji/happy.png'
    if x=="['neutral']":
        overlay='/home/aurash/emoji/neutral.png'
    if x=="['surprise']":
        overlay='/home/aurash/emoji/surprise.jpg'
    if x=="['disgust']":
        overlay='/home/aurash/emoji/disgust.jpg'
    l_img = src
    s_img = cv2.imread(overlay)
    s_img=cv2.resize(s_img,(150,150))
    x_offset=485
    y_offset=54
    l_img[y_offset:y_offset+s_img.shape[0], x_offset:x_offset+s_img.shape[1]] = s_img
    return l_img

def draw_face(frame, landmarks):
	draw_landmarks(frame, landmarks)
	draw_face_outline(frame, landmarks)
	draw_lefteye(frame, landmarks)
	draw_lefteyebrow(frame, landmarks)
	draw_righteye(frame, landmarks)
	draw_righteyebrow(frame, landmarks)
	draw_nosebridge(frame, landmarks)
	draw_nose(frame, landmarks)
	draw_mouth(frame, landmarks)
	x=displayEmotion(frame, landmarks)
	return x

def main():
	mystasm = pyasm.STASM()
	cap, pos_frame = video_config()
        done = False
        start = True	
	while done != True:
	    flag, frame = cap.read()
	    global frames
	    frames=cv2.imread("/home/aurash/sandbox/sandbox/aurash/landmarking/white.jpg",1)
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
	        if start == True:
	        	mylandmarks = mystasm.s_search_single(image)
	        	start = False
	        if start == False:
	        	landmarksOLD = mylandmarks
	        	mylandmarks = mystasm.s_search_single(image)
	        	alpha = .85
	        	#mylandmarks = (1-alpha)* landmarksOLD + alpha * mylandmarks
	        	
	        # draw the landmarks point as circles
			x=draw_face(frame, mylandmarks)


			
	        frame=OverlayImage(frame,x)
	        cv2.namedWindow("Live Landmarking", cv2.WINDOW_NORMAL)          
	        cv2.imshow("Live Landmarking", frame)
	        cv2.namedWindow('k', cv2.WINDOW_NORMAL)
	        cv2.imshow('k',frames)

	      	# cv2.waitKey(50)

	    #else:
	        # The next frame is not ready, so we try to read it again
	    #    cap.set(cv2.cv.CV_CAP_PROP_POS_FRAMES, pos_frame-1)
	    #    print "frame is not ready"
		#	 cv2.waitKey(10)

	    if cv2.waitKey(33) == 1048603:
                done = True 
            #k = cv2.waitKey(33)
            #print k, " ", ord(k)
 
	    #if cap.get(cv2.cv.CV_CAP_PROP_POS_FRAMES) == cap.get(cv2.cv.CV_CAP_PROP_FRAME_COUNT):
	    #    break
	
if __name__ == '__main__':
    main()
