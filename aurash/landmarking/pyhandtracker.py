#! /usr/bin/python

pose_to_use = 'Psi'

from openni import *

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
        skel_cap.start_tracking(id)
    else:
        print "ERR User {} failed to calibrate. Restarting process." .format(id)
        new_user(user, id)

def lost_user(src, id):
    print "--- User {} lost." .format(id)

def normalize(pos,loc):
    hand=pos
    head=loc
    result1=(hand[0]-head[0])
    result2=(hand[1]-head[1])
    result3=(hand[2]-head[2])
    result1s=(hand[0]-head[0])**2
    result2s=(hand[1]-head[1])**2
    result3s=(hand[2]-head[2])**2
    norm=(result1s+result2s+result3s)**.5

    list=[result1/norm,result2/norm,result3/norm]  
    print list

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

# while


