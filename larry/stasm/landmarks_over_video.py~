#!/usr/bin/env python2
# -*- coding: utf-8 -*-

"""In this example we can see how to call 
the s_search_single fuction of STASM from Python 
with a video and save the landmarks into txt"""

import cv2
import numpy as np
import pyasm



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
	map(lambda p: cv2.circle(frame, (int(p[0]), int(p[1])), 1, (512,512,255), -1), landmarks)
	count=0
        for (x, y) in landmarks:
           cv2.putText(frame,str(count), (int(x)+5,int(y)+5), cv2.FONT_HERSHEY_SIMPLEX, .25, 255)
           count=count+1
        return

def draw_face_outline(frame, landmarks):
        return draw_loop(frame, landmarks, 0, 15)

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
	while done != True:
	    flag, frame = cap.read()
	    if flag:
	        # The frame is ready and already captured
	        # save a tmp file because pystasm receive by parameter a filename
	        filename = '/tmp/frame{}.jpg'.format(pos_frame)
	        cv2.imwrite(filename, frame)
	        # nasty fix .. pystasm should receive np array .. 
	        mylandmarks = mystasm.s_search_single(filename)
	        
	        # draw the landmarks point as circles
		draw_face(frame, mylandmarks)

	        cv2.namedWindow("Live Landmarking", cv2.WINDOW_NORMAL)          
	        cv2.imshow("Live Landmarking", frame)
	      

	    #else:
	        # The next frame is not ready, so we try to read it again
	    #    cap.set(cv2.cv.CV_CAP_PROP_POS_FRAMES, pos_frame-1)
	    #    print "frame is not ready"
	        #cv2.waitKey(10)

	    if cv2.waitKey(1) == 1048603:
                done = True 
            #k = cv2.waitKey(33)
            #print k, " ", ord(k)
 
	    #if cap.get(cv2.cv.CV_CAP_PROP_POS_FRAMES) == cap.get(cv2.cv.CV_CAP_PROP_FRAME_COUNT):
	    #    break
	
if __name__ == '__main__':
    main()
