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
	mystasm.s_init()
	cap, pos_frame = video_config()
	

	while True:
	    found=False
	    flag, frame = cap.read()
	    if flag:
	        # The frame is ready and already captured
	        # save a tmp file because pystasm receive by parameter a filename
	        filename = '/tmp/frame{}.jpg'.format(pos_frame)
	        cv2.imwrite(filename, frame)
	        # nasty fix .. pystasm should receive np array .. 
	        
            
            #landmark_array = mystasm.s_search_single(filename)
            #for (x, y) in landmark_array:
             #   print (x, y)
	        mystasm.s_open_image(filename)
	        (landmark_array, landmark_found) = mystasm.s_search_auto()
	        points=[]
	        count=1
	        while landmark_found.value == 1:
	            if landmark_found.value==1:
	                print("**************************************Face " +str(count)+"**************************************")
	                count=count+1
	                points.append(landmark_array)
	                for (x, y) in landmark_array:
	                    print (x, y)    
	                (landmark_array, landmark_found) = mystasm.s_search_auto()
	                print len(points)
	                found=True
	        if landmark_found.value==0 and found:
	            count2=0
	            size=len(points)-1
	            while(size>=0):
				    for (x, y) in points[size]:
				        if count2>76:
				            count2=0
				        #print (x, y)
				        cv2.putText(frame,str(count2), (int(x)+5,int(y)+5), cv2.FONT_HERSHEY_SIMPLEX, .25, 255)
				        cv2.circle(frame,(int(x),int(y)), 1, (0,0,255), -1)
				        if count2 > 0 and count2 != 48 and count2 != 59 and count2 != 31 and count2 != 38 and count2 != 41 and count2 != 18 and count2 != 28 and count2 != 22:
				                cv2.line(frame,(int(tempx),int(tempy)),(int(x),int(y)),(0,0,255), 1)
				        tempx=x
				        tempy=y        
				        count2=count2+1
				    size=size-1
	        cv2.namedWindow("Live Landmarking", cv2.WND_PROP_FULLSCREEN)          
	        cv2.setWindowProperty("Live Landmarking", cv2.WND_PROP_FULLSCREEN, cv2.cv.CV_WINDOW_FULLSCREEN)    
	        cv2.imshow("Live Landmarking", frame)    
	            #draw(points,'/tmp/frame{}.jpg'.format(pos_frame))
	            #mystasm.s_open_image(filename)
          

	    else:
	        # The next frame is not ready, so we try to read it again
	        cap.set(cv2.cv.CV_CAP_PROP_POS_FRAMES, pos_frame)
	        print "frame is not ready"
	        cv2.waitKey(10)

	    if cv2.waitKey(5) == 27:
	        break

	    #if cap.get(cv2.cv.CV_CAP_PROP_POS_FRAMES) == cap.get(cv2.cv.CV_CAP_PROP_FRAME_COUNT):
	     #   break

if __name__ == '__main__':
    main()
