#!/usr/bin/env python2
# -*- coding: utf-8 -*-

"""In this example we can see how to call 
the s_search_single fuction of STASM from Python 
with a video and save the landmarks into txt"""

import cv2
import numpy as np
import pyasm
import math


FILENAME =  '/tmp/out.webm'

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
	return

def draw_arc(frame, landmarks, start, end):
	count = 0
        (tempx, tempy) = landmarks[start]
	for (x, y) in landmarks[start+1:end+1]:
            cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
            count=count+1
            tempx=x
            tempy=y
	return

def draw_loop(frame, landmarks, start, end):
        count = 0
        (tempx, tempy) = landmarks[start]
	for (x, y) in landmarks[start+1:end+1]:
            cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
            count=count+1
            tempx=x
            tempy=y
        (x, y) = landmarks[start]
        cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
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
	map(lambda p: cv2.circle(frame, (int(p[0]), int(p[1])), 1, (512,512,255), -1), landmarks)
	count=0
	numbering = 0
        for (x, y) in landmarks:
        	# if count == 52 or count == 14 or count == 13 or count == 15 or count == 65 or count == 59:
        	if count == 20 or count == 22 or count == 38 or count == 39:
        		cv2.putText(frame, str(scale * (landmarks[38][1] - landmarks[17][1])), (50,50), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
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
	       		if count == 52:
	       			xcoor = 0
	       			ycoor = 0
       			coorPair = str(count) + ":   (" + str(round(xcoor, 2)) + ", " + str(round(ycoor, 2)) + ")"
         		cv2.putText(frame, str(count), (int(x)+5,int(y)+5), cv2.FONT_HERSHEY_SIMPLEX, .4, 255)
          		cv2.putText(frame, coorPair, (100, 115 + numbering * 20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
          		numbering = numbering + 1
           	count = count+1
        return

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
		scale = 10 / distTOTAL
		return scale

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
	return

def main():
	mystasm = pyasm.STASM()
	cap, pos_frame = video_config()
        done = False
        start = True	
	while done != True:
	    flag, frame = cap.read()
	    if flag:
	        # The frame is ready and already captured
	        # save a tmp file because pystasm receive by parameter a filename
	        filename = '/tmp/frame{}.jpg'.format(pos_frame)
	        cv2.imwrite(filename, frame)
	        # nasty fix .. pystasm should receive np array .. 
	        if start == True:
	        	mylandmarks = mystasm.s_search_single(filename)
	        	start = False
	        if start == False:
	        	landmarksOLD = mylandmarks
	        	mylandmarks = mystasm.s_search_single(filename)
	        	alpha = .95
	        	mylandmarks = (1-alpha)* landmarksOLD + alpha * mylandmarks

	        # draw the landmarks point as circles
		draw_face(frame, mylandmarks)

	        cv2.namedWindow("Live Landmarking", cv2.WINDOW_NORMAL)          
	        cv2.imshow("Live Landmarking", frame)
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
