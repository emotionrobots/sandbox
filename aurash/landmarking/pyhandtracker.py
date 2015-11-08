#! /usr/bin/python

pose_to_use = 'Psi'

from openni import *
from sklearn.svm import SVC
from sklearn.externals import joblib
import numpy as np
import subprocess
import math
import os
import cv2

context = Context()
context.init()

depth_generator = DepthGenerator()
depth_generator.create(context)
depth_generator.set_resolution_preset(RES_VGA)
depth_generator.fps = 30

gesture_generator = GestureGenerator()
gesture_generator.create(context)
gesture_generator.add_gesture('Wave')

hands_generator = HandsGenerator()
hands_generator.create(context)

user = UserGenerator()
user.create(context)

skel_cap = user.skeleton_cap
pose_cap = user.pose_detection_cap
global tempd

text="Please put your hands up and surrender"
subprocess.call("echo "+text+"|festival --tts", shell=True)
# Declare the callbacks
# gesture
def gesture_detected(src, gesture, id, end_point):
    print "Detected gesture:", gesture
    hands_generator.start_tracking(end_point)
# gesture_detected

def gesture_progress(src, gesture, point, progress): pass
# gesture_progress

def create(src, id, pos, time):
    print 'Create ', id, pos
# create

def update(src, id, pos, time):
    global posh
    posh=pos
   # print pos
# update

def destroy(src, id, time):
    print 'Destroy ', id
# destroy

def new_user(src, id):
    print "1/4 User {} detected. Looking for pose..." .format(id)
    pose_cap.start_detection(pose_to_use, id)

def pose_detected(src, pose, id):
    print "2/4 Detected pose {} on user {}. Requesting calibration..." .format(pose,id)
    pose_cap.stop_detection(id)
    skel_cap.request_calibration(id, True)

def calibration_start(src, id):
    print "3/4 Calibration started for user {}." .format(id)

def calibration_complete(src, id, status):
    if status == CALIBRATION_STATUS_OK:
        print "4/4 User {} calibrated successfully! Starting to track." .format(id)
        text="please, wave at the camera to begin tracking"
        subprocess.call("echo "+text+"|festival --tts", shell=True)
        skel_cap.start_tracking(id)
    else:
        print "ERR User {} failed to calibrate. Restarting process." .format(id)
        new_user(user, id)

def lost_user(src, id):
    print "--- User {} lost." .format(id)

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
	filename = os.getcwd()
	if myDirection=="['north']":
		frame=filename+'/directions/north.png'		
	if myDirection=="['northwest']":
		frame=filename+'/directions/northwest.png'
	if myDirection=="['northeast']":
		frame=filename+'/directions/northeast.png'
	if myDirection=="['east']":
		frame=filename+'/directions/east.png'
	if myDirection=="['west']":
		frame=filename+'/directions/west.png'
	frame=cv2.imread(frame)
	cv2.putText(frame, "Phi:  "  + str(round(phi)), (100,100), cv2.FONT_HERSHEY_DUPLEX, 1, (93,255,245),1)
	cv2.putText(frame, myDirection, (740,100), cv2.FONT_HERSHEY_DUPLEX, 1, (93,255,245),1)	
	cv2.putText(frame, "Theta:  "  + str(round(theta)), (100,180), cv2.FONT_HERSHEY_DUPLEX, 1, (93,255,245),1)
	pt=(512+(int(math.cos((theta*(math.pi/180)))*400)),512-(int(math.sin((theta*(math.pi/180)))*400)))
	cv2.line(frame,(512,512) , pt,(255,0,0), 5) 
	cv2.namedWindow("directions", cv2.WINDOW_NORMAL) 
	cv2.imshow("directions",frame)	
	if cv2.waitKey(1) & 0xFF == ord('q'):
		return


# Register the callbacks


user.register_user_cb(new_user, lost_user)
pose_cap.register_pose_detected_cb(pose_detected)
skel_cap.register_c_start_cb(calibration_start)
skel_cap.register_c_complete_cb(calibration_complete)

# Set the profile
skel_cap.set_profile(SKEL_PROFILE_ALL)
gesture_generator.register_gesture_cb(gesture_detected, gesture_progress)
hands_generator.register_hand_cb(create, update, destroy)

# Start generating
context.start_generating_all()

print 'Surrender to start Tracking!...'
print 'After you surrender wave hello!'

while True:
    flag=False
    context.wait_any_update_all()
    for id in user.users:
	    if skel_cap.is_tracking(id):
	        head = skel_cap.get_joint_position(id, SKEL_HEAD)
	        #print "  {}: head at ({loc[0]}, {loc[1]}, {loc[2]}) [{conf}]" .format(id, loc=head.point, conf=head.confidence)
	        try: 
	        	x=posh[0]
	        	flag=True
	        except:
	        	pass	
	        if flag:
	        	loc=head.point
	        	normalize(posh,loc)	



