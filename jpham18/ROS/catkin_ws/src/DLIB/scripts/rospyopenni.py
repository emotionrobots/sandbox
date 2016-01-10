#!/usr/bin/env python

import openni as opi
import numpy as np
import rospy
import cv2
from std_msgs.msg import String
from sensor_msgs.msg import Image
from cv_bridge import CvBridge, CvBridgeError
from DLIB.msg import Skeleton

if __name__ == '__main__':
    rospy.init_node('OpenNI', anonymous=True)
    rgb_pub = rospy.Publisher('rgb', String, queue_size=10)
    rgb_pub2 = rospy.Publisher('rgb2', Image, queue_size=10)
    depth_pub = rospy.Publisher('depth', String, queue_size=10) 
    gesture_pub = rospy.Publisher('gesture', String, queue_size=10)
    skeleton_pub = rospy.Publisher('skeleton', Skeleton, queue_size=10)
    skeleton_msg_pub = rospy.Publisher('skeleton_msg', String, queue_size=10)
    rate = rospy.Rate(30) # 30hz 

    # #### Create context and generators

    ctx = opi.Context()
    ctx.init() 

    depth_generator = opi.DepthGenerator()
    depth_generator.create(ctx)
    depth_generator.set_resolution_preset(opi.RES_VGA)
    depth_generator.fps = 30 

    image_generator = opi.ImageGenerator()
    image_generator.create(ctx) 

    # hands_generator = opi.HandsGenerator()
    # hands_generator.create(ctx)

    gesture_generator = opi.GestureGenerator()
    gesture_generator.create(ctx)
    gesture_generator.add_gesture("Click")
    gesture_generator.add_gesture('Wave') 

    user = opi.UserGenerator()
    user.create(ctx)
    skel_cap = user.skeleton_cap
    pose_cap = user.pose_detection_cap
    POSE2USE = 'Psi'
    name_joints = ['SKEL_HEAD', 'SKEL_LEFT_FOOT', 'SKEL_RIGHT_SHOULDER',
                       'SKEL_LEFT_HAND', 'SKEL_NECK',
                       'SKEL_RIGHT_FOOT', 'SKEL_LEFT_HIP', 'SKEL_RIGHT_HAND',
                       'SKEL_TORSO', 'SKEL_LEFT_ELBOW', 'SKEL_LEFT_KNEE',
                       'SKEL_RIGHT_HIP', 'SKEL_LEFT_SHOULDER',
                       'SKEL_RIGHT_ELBOW', 'SKEL_RIGHT_KNEE']

    bridge = CvBridge()


    # #### Write callbalks ...

    def gesture_detected(src, gesture, id, end_point):
        pass

    def gesture_progress(src, gesture, point, progress):
        gesture_pub.publish(""+gesture)

    # def create(src, id, pos, time):
    #     pass
    
    # def update(src, id, pos, time):
    #     if pos:
    #         tmp_pos = depth_generator.to_projective([pos])[0]
    #         print (type)(tmp_pos[0]) + " " + (type)(tmp_pos[1])

    # def destroy(src, id, time):
    #     pass

    def new_user(src, id):
        skeleton_msg_pub.publish("Hi User %s. Make the secret pose ..." %(id))
        pose_cap.start_detection(POSE2USE, id)

    def lost_user(src, id):
        skeleton_msg_pub.publish("Bye Bye User %s" %(id))

    def pose_detected(src, pose, id):
        skeleton_msg_pub.publish("The User %s is doing the secret pose %s, now do the calibration" %(id, pose))
        pose_cap.stop_detection(id)
        skel_cap.request_calibration(id, True)

    def calibration_complete(src, id, status):
        if status == opi.CALIBRATION_STATUS_OK:
            skeleton_msg_pub.publish("Congrats User %s! You're Calibrated" %(id))
            skel_cap.start_tracking(id)
        else:
            skeleton_msg_pub.publish("Something went wrong User %s :(" %(id))
            new_user(user, id)


    # #### Register callbacks ...

    gesture_generator.register_gesture_cb(gesture_detected, gesture_progress)
    # hands_generator.register_hand_cb(create, update, destroy)
    user.register_user_cb(new_user, lost_user)
    pose_cap.register_pose_detected_cb(pose_detected)
    skel_cap.register_c_complete_cb(calibration_complete)
    skel_cap.set_profile(opi.SKEL_PROFILE_ALL)


    # #### Converting and publishing captured data

    def capture_rgb():
        image_str = image_generator.get_raw_image_map_bgr()
        rgb_pub.publish(image_str)
        frame = np.fromstring(image_str, dtype=np.uint8).reshape(480, 640, 3)
        try:
            rgb_pub2.publish(bridge.cv2_to_imgmsg(frame, "bgr8"))
        except CvBridgeError as e:
            print(e)

    def capture_depth():
        depth_pub.publish(depth_generator.get_raw_depth_map_8()) 

    def get_joints():
        for id in user.users:
            if skel_cap.is_tracking(id) and skel_cap.is_calibrated(id):
                joints = [skel_cap.get_joint_position(id, j)
                      for j in map(lambda a: getattr(opi, a), name_joints)]

                newpos_skeleton = depth_generator.to_projective([j.point for j in joints])
                if newpos_skeleton:
                    skeleton_msg = Skeleton()
                    skeleton_msg.id = id
                    skeleton_msg.data = str(newpos_skeleton)
                    skeleton_pub.publish(skeleton_msg)

    ctx.start_generating_all() 


    # #### Main loop

    while not rospy.is_shutdown():
        capture_rgb()
        capture_depth()
        get_joints()
        ctx.wait_any_update_all()
        rate.sleep() 


    # #### Then we stop and close the context 

    ctx.stop_generating_all()
    ctx.shutdown() 