#! /usr/bin/python


from sklearn.svm import SVC
from sklearn.externals import joblib
import numpy as np
import subprocess
import math
import os
import cv2
import rospy
import ast
from ros_rp2w.msg import CustomString
from std_msgs.msg import String, Header
from ros_rp2w.srv._festTTS import *



def normalize(pos,loc):
    hand=pos
    hand1n=(hand[0]**2+hand[1]**2+hand[2]**2)**.5
    hand1=(hand[0]/hand1n,hand[1]/hand1n,hand[2]/hand1n)
    pyth=(hand1[0]**2+hand1[1]**2)**.5
    dire=hand1[2]/pyth
    direc=hand1[1]/hand1[0]
    phi=math.atan(dire)
    theta=math.atan(direc)	
    phi=phi*(180/math.pi)
    theta=theta*(180/math.pi) 
    if theta<0:
        theta=abs(theta)
    else:
        theta=abs(theta-90)+90
    head=loc
    result1=(hand[0]-head[0])
    result2=(hand[1]-head[1])
    result3=(hand[2]-head[2])
    result1s=(hand[0]-head[0])**2
    result2s=(hand[1]-head[1])**2
    result3s=(hand[2]-head[2])**2
    norm=(result1s+result2s+result3s)**.5
    list=[result1/norm,result2/norm,result3/norm]
    #print list
    svm(list,phi,theta)

def svm(norm,phi,theta):
	featureVector = np.array([
	[-0.1481973487088052, -0.24924833721956716, -0.9570333391418305],#north
	[-0.15349991047435893, -0.2857182693400982, -0.9459401926388702],#north
	[0.31878462701915783, -0.26047582363480976, -0.9113334773166318],#northwest
	[0.25045210880362406, -0.2763346426363811, -0.9278539251815672],#northwest
	[-0.7162289983919157, -0.1959220731188152, -0.6697988975262195], #northeast
	[-0.28408222308788256, -0.18347550246395897, -0.941081309197587],#northeast
    [-0.9198956474341062, -0.35393120072672907, -0.16889257823812798],#east
    [-0.7949472467042304, -0.39038306547286544, -0.4643920080593244],#east
    [0.8379803918902592, -0.3699293004161426, -0.4011747443459904],#west
    [0.5708613707317903, -0.6626500195300341, -0.48478061741685463]#west
])
	direction = np.array(['north','north', 'northwest', 'northwest', 'northeast','northeast', 'east','east', 'west','west'])
	clf = SVC()
	clf.fit(featureVector, direction)
	currentDirection = norm
	myDirection = str(clf.predict(currentDirection))
	prettyprint(phi,theta,myDirection)
	#print myDirection


def prettyprint(phi,theta,myDirection):
    dir_pub = rospy.Publisher('direction', CustomString, queue_size=10)
    if myDirection=="['north']":
        msg=CustomString()
        msg.data="north"
        dir_pub.publish(msg)
       # tts("north")		
    if myDirection=="['northwest']":
        msg=CustomString()
        msg.data="northwest"
        dir_pub.publish(msg)
       # tts("northwest")
    if myDirection=="['northeast']":
        msg=CustomString()
        msg.data="northeast"
        dir_pub.publish(msg)
       # tts("northeast")
    if myDirection=="['east']":
        msg=CustomString()
        msg.data="east"
        dir_pub.publish(msg)
        #tts("east")
    if myDirection=="['west']":
        msg=CustomString()
        msg.data="west"
        dir_pub.publish(msg)
        #tts("west")
    print myDirection    

# def tts(string2):
#     rospy.wait_for_service('tts')
#     try:
#         tts = rospy.ServiceProxy('tts', festTTS)
#         req = festTTSRequest(string2)
#         resp = tts(string2)
#         print str(resp)[4:]
#     except rospy.ServiceException, e:
#         print "Service call failed: %s"%e     


# Register the callbacks
def listener():
    rospy.Subscriber('skeleton', CustomString, callback_skeleton)
    rospy.spin()

def callback_skeleton(msg):
    #print msg.header.stamp
    print "hello"
    sk_head = 0
    sk_left_hand= 3
    #sk_right_hand   = 7
    newpos_skeleton = ast.literal_eval(msg.data) 
    try: 
        hand=newpos_skeleton[sk_left_hand]
        print hand
        flag=True
        if flag:
            head=newpos_skeleton[sk_head]
            normalize(hand,head)
    except Exception, e:
    	print e


if __name__ == '__main__':
    rospy.init_node('handtracker', anonymous=True)
    listener()
