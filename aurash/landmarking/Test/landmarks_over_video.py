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
	        
	        # draw the landmarks
	        map(lambda p: cv2.circle(frame, (int(p[0]), int(p[1])), 1, (512,512,255), -1), mylandmarks)
	        count=0
                for (x, y) in mylandmarks:
                    cv2.putText(frame,str(count), (int(x)+5,int(y)+5), cv2.FONT_HERSHEY_SIMPLEX, .25, 255)
                    if count > 0 and count != 48 and count != 59 and count != 31 and count != 38 and count != 41 and count != 18 and count != 28 and count != 22:
                       cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
                    count=count+1
                    tempx=x
                    tempy=y
	        #cv2.namedWindow("Live Landmarking", cv2.WND_PROP_FULLSCREEN)          
	        #cv2.setWindowProperty("Live Landmarking", cv2.WND_PROP_FULLSCREEN, cv2.cv.CV_WINDOW_FULLSCREEN)    
	        cv2.namedWindow("Live Landmarking")          
	        #cv2.setWindowProperty("Live Landmarking", cv2.WND_PROP_FULLSCREEN, cv2.cv.CV_WINDOW_FULLSCREEN)    
	        cv2.imshow("Live Landmarking", frame)
	      

	    #else:
	        # The next frame is not ready, so we try to read it again
	    #    cap.set(cv2.cv.CV_CAP_PROP_POS_FRAMES, pos_frame-1)
	    #    print "frame is not ready"
	        #cv2.waitKey(10)

	    if cv2.waitKey(1) == 27:
                done = True 
            #k = cv2.waitKey(33)
            #print k, " ", ord(k)
 
	    #if cap.get(cv2.cv.CV_CAP_PROP_POS_FRAMES) == cap.get(cv2.cv.CV_CAP_PROP_FRAME_COUNT):
	    #    break
	
if __name__ == '__main__':
    main()
