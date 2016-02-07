#!/usr/bin/env python

import numpy as np
import rospy
import cv2
import ast
from skeleton.msg import CustomString
from std_msgs.msg import String, Header
import Image
from skeleton.msg import face_p
from skeleton.msg import Face
import thread
import time
import sys
import os



global data
data = np.zeros( (960,1920,3), dtype=np.uint8)
global allframe
allframe= np.zeros((960,1280,3), dtype=np.uint8)




def callback_rgb(msg):
    #print msg.header.stamp
    #global data
    global frame
    frame = np.fromstring(msg.data, dtype=np.uint8).reshape(480, 640, 3)
    sframe= cv2.copyMakeBorder(frame,0,0,0,0,cv2.BORDER_REPLICATE)
    cv2.putText(sframe, "RGB Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
    #print frame
    #global res
    #res=cv2.resize(sframe,(1280,960))
    #data[0:960,0:1280]=res
    #display()

def landmark(data2):
	#face_points2=numpy.asarray(data.arr.data)
	#global data
	global mylandmarks
	global res
	#global frame
	aframe= cv2.copyMakeBorder(frame,0,0,0,0,cv2.BORDER_REPLICATE)
	mylandmarks = []
	alllandmarks = np.array(data2.arr.data)
	for x in range(0,len(alllandmarks) - 2):
		if x % 2 == 0:
			mylandmarks = mylandmarks + [[alllandmarks[x],alllandmarks[x+1]]]
	# print mylandmarks[0]
	mylandmarks = np.array(mylandmarks)
	res=cv2.resize(aframe,(1280,960))
	#print mylandmarks
	#print mylandmarks
    #try:
        #cv_image = bridge.imgmsg_to_cv2(data.image, "bgr8")
        #cv2.putText(cv_image, str(data.header.stamp), (100,100), cv2.FONT_HERSHEY_DUPLEX, 1, (0,0,255),1)
        #cv2.imshow("points2",cv_image)
    #excenewpos_skeleton[][]newpos_skeleton CvBridgeError as e:
    #    print(e)
	map(lambda p: cv2.circle(res, ((abs(p[0]*2)), (abs(p[1])*2)), 3, (58,98,255), -1), mylandmarks)
	#cv2.putText(aframe, "Dlib Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
	#res=cv2.resize(frame,(320,240))
	#data[0:480,640:1280]=aframe
	#data[0:960,0:1280]=res

def facer(data3):
	global res
	#print data3.name
	#print data3.llx
	#print data3.lly
	#print data3.urx
	#print data3.ury
	global xx
	global yy
	xx=data3.llx*2
	yy=data3.lly*2
	global w
	global h
	w=data3.urx*2-data3.llx*2
	global res
	global dap
	dap=data3
	h=data3.ury*2-data3.lly*2
	#global frame
	#nframe=cv2.copyMakeBorder(frame,0,0,0,0,cv2.BORDER_REPLICATE)
	#cv2.putText(nframe, "FaceRec Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
	cv2.putText(res, data3.name, ((data3.urx)*2+20,(data3.ury)*2+20), cv2.FONT_HERSHEY_DUPLEX, .9, (51,220,0))
	cv2.rectangle(res, (xx*2,yy*2),(xx*2+w*2,yy*2+h*2),(51,220,0),2)
	#data[0:960,0:1280]=res
	



def callback_gest(msg):
    global res
    # print msg.header.stamp
    #rframe= cv2.copyMakeBorder(frame,0,0,0,0,cv2.BORDER_REPLICATE)
    #cv2.putText(rframe, "Skel Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
    gesture1=msg.data
    try:
    	cv2.putText(res, gesture1, (128,158), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
    except Exception, e:
    	pass
    #data[0:960,0:1280]=res

def callback_chat(msg):
	text=str(msg)
	text=text[5:]
	filename = os.path.abspath(os.path.dirname(__file__))
	cframe= cv2.imread(filename+'/SpeechRec.png')
	if len(text)<39:
	    cv2.putText(cframe, text, (12,193), cv2.FONT_HERSHEY_DUPLEX, .9, (0,0,255))
	else:
		cv2.putText(cframe, text[0:39], (12,193), cv2.FONT_HERSHEY_DUPLEX, .9, (0,0,255))
		cv2.putText(cframe, text[40:], (12,213), cv2.FONT_HERSHEY_DUPLEX, .9, (0,0,255))
	cv2.putText(cframe, "STT Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (0,0,255))
	data[480:960,1280:1920]=cframe

def callback_skeleton(msg):
	#print msg.header.stamp
	global res
	newpos_skeleton = ast.literal_eval(msg.data) 
	#rframe= cv2.copyMakeBorder(frame,0,0,0,0,cv2.BORDER_REPLICATE)
	map(lambda p: cv2.circle(res, (int(p[0])*2, int(p[1])*2), 5, (255,0,0), 2), newpos_skeleton)
	#cv2.putText(rframe, "Skel Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
	sk_head = 0
	sk_left_foot = 1
	sk_right_shoulder= 2
	sk_left_hand= 3
	sk_neck	= 4
	sk_right_foot= 5
	sk_left_hip = 6
	sk_right_hand	= 7
	sk_torso = 8
	sk_left_elbow 	= 9
	sk_left_knee 	= 10 
	sk_right_hip 	= 11 
	sk_left_shoulder= 12 
	sk_right_elbow 	= 13 
	sk_right_knee 	= 14
	for lis in newpos_skeleton:
	    lis[0]=int(lis[0])*2
	    lis[1]=int(lis[1])*2
	cv2.line(res,(newpos_skeleton[0][0],newpos_skeleton[0][1]) , (newpos_skeleton[4][0],newpos_skeleton[4][1]),(255,0,0), 5) #head to neck 
	cv2.line(res,(newpos_skeleton[4][0],newpos_skeleton[4][1]) , (newpos_skeleton[2][0],newpos_skeleton[2][1]),(255,0,0), 5) #neck to right shoulder
	cv2.line(res,(newpos_skeleton[4][0],newpos_skeleton[4][1]) , (newpos_skeleton[12][0],newpos_skeleton[12][1]),(255,0,0), 5) #neck to left shoulder
	cv2.line(res,(newpos_skeleton[2][0],newpos_skeleton[2][1]) , (newpos_skeleton[13][0],newpos_skeleton[13][1]),(255,0,0), 5) #shoulder to elbow right
	cv2.line(res,(newpos_skeleton[13][0],newpos_skeleton[13][1]) , (newpos_skeleton[7][0],newpos_skeleton[7][1]),(255,0,0), 5) #elbow to hand right
	cv2.line(res,(newpos_skeleton[12][0],newpos_skeleton[12][1]) , (newpos_skeleton[9][0],newpos_skeleton[9][1]),(255,0,0), 5) # shoulder to elbow left
	cv2.line(res,(newpos_skeleton[9][0],newpos_skeleton[9][1]) , (newpos_skeleton[3][0],newpos_skeleton[3][1]),(255,0,0), 5) # elbow to left hand
	cv2.line(res,(newpos_skeleton[4][0],newpos_skeleton[4][1]) , (newpos_skeleton[8][0],newpos_skeleton[8][1]),(255,0,0), 5) # neck to torso
	cv2.line(res,(newpos_skeleton[8][0],newpos_skeleton[8][1]) , (newpos_skeleton[11][0],newpos_skeleton[11][1]),(255,0,0), 5) # torso to right hip
	cv2.line(res,(newpos_skeleton[8][0],newpos_skeleton[8][1]) , (newpos_skeleton[6][0],newpos_skeleton[6][1]),(255,0,0), 5) # torso to left hip
	cv2.line(res,(newpos_skeleton[11][0],newpos_skeleton[11][1]) , (newpos_skeleton[14][0],newpos_skeleton[14][1]),(255,0,0), 5) # right hip to right knee
	cv2.line(res,(newpos_skeleton[6][0],newpos_skeleton[6][1]) , (newpos_skeleton[10][0],newpos_skeleton[10][1]),(255,0,0), 5) # left hip to left knee
	cv2.line(res,(newpos_skeleton[14][0],newpos_skeleton[14][1]) , (newpos_skeleton[5][0],newpos_skeleton[5][1]),(255,0,0), 5) # right knee to right foot
	cv2.line(res,(newpos_skeleton[10][0],newpos_skeleton[10][1]) , (newpos_skeleton[1][0],newpos_skeleton[1][1]),(255,0,0), 5) # left knee to left foot
	#data[0:960,0:1280]=rframe

def callback_emotion(msg):
	emotion1=msg.data
	#face_points2=numpy.asarray(data.arr.data)
	#global data
	#global frame
	oframe= cv2.copyMakeBorder(frame,0,0,0,0,cv2.BORDER_REPLICATE)
	cv2.putText(oframe, "Emotion Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
	cv2.putText(oframe, emotion1, (48,48), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
	#res=cv2.resize(frame,(320,240))
	data[0:480,1280:1920]=oframe
	global res
	global emotion1
	#cv2.putText(res, "Emotion Frame", (28,28), cv2.FONT_HERSHEY_DUPLEX, .9, (255,0,0))
	cv2.putText(res, emotion1, (48,78), cv2.FONT_HERSHEY_DUPLEX, .9, (0,196,255))
	#res=cv2.resize(frame,(320,240))
	



def callback_skeleton_msg(msg):
    # print msg.header.stamp
    print msg.data

def display(arg1, arg2):
	global res
	global dap
	while True:
		try:
		    try:
		    	cv2.putText(res, dap.name, ((dap.urx)*2+20,(dap.ury)*2+20), cv2.FONT_HERSHEY_DUPLEX, .9, (51,220,0))
		    	cv2.rectangle(res, (xx*2,yy*2),(xx*2+w*2,yy*2+h*2),(51,220,0),2)
		    except Exception, e:
		    	print e
		    try:
		    	cv2.putText(res, emotion1, (48,78), cv2.FONT_HERSHEY_DUPLEX, .9, (0,196,255))
		    except Exception, e:
		    	pass
		    cv2.putText(res, "Image Processing", (30,30), cv2.FONT_HERSHEY_DUPLEX, .9, (225,0,0))
		    data[0:960,0:1280]=res
		    cv2.namedWindow("frame", cv2.WINDOW_NORMAL) 
		    cv2.imshow("frame",data)
		    if cv2.waitKey(27) and 0xFF == ord('q'):
		        sys.exit()	
		except Exception, e:
			print e



def run():
    rospy.init_node('listener2')
    rospy.Subscriber('rgb', CustomString, callback_rgb)
    rospy.Subscriber('face_points', face_p, landmark)
    rospy.Subscriber("known_faces",Face,facer)
    rospy.Subscriber('gesture', CustomString, callback_gest)
    rospy.Subscriber('skeleton', CustomString, callback_skeleton)
    rospy.Subscriber("chatter",String,callback_chat)
    rospy.Subscriber("emotion",String,callback_emotion)
    #rospy.Subscriber('skeleton_msg', CustomString, callback_skeleton_msg)
    rospy.spin()


if __name__ == '__main__':
	filename = os.path.abspath(os.path.dirname(__file__))
	cframe= cv2.imread(filename+'/SpeechRec.png')
	data[480:960,1280:1920]=cframe
	thread.start_new_thread(display,('data','fart') )
	run()
    
