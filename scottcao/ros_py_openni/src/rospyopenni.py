#!/usr/bin/env python

import openni as opi
import numpy as np
import rospy
import cv2
from std_msgs.msg import String, Header
from sensor_msgs.msg import Image
from cv_bridge import CvBridge, CvBridgeError
from ros_py_openni.msg import CustomString

if __name__ == '__main__':
    rospy.init_node('OpenNI', anonymous=True)
    rgb_pub = rospy.Publisher('rgb', CustomString, queue_size=10)
    rgb_pub2 = rospy.Publisher('rgb2', Image, queue_size=10)
    depth_pub = rospy.Publisher('depth', CustomString, queue_size=10) 
    gesture_pub = rospy.Publisher('gesture', CustomString, queue_size=10)
    skeleton_pub = rospy.Publisher('skeleton', CustomString, queue_size=10)
    skeleton_msg_pub = rospy.Publisher('skeleton_msg', CustomString, queue_size=10)
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
        gesture_msg = CustomString()
        gesture_msg.data = gesture
        gesture_msg.header = Header()
        gesture_msg.header.stamp = rospy.Time.now()
        gesture_pub.publish(gesture_msg)

    def new_user(src, id):
        skeleton_msg = CustomString()
        skeleton_msg.header = Header()
        skeleton_msg.header.stamp = rospy.Time.now()
        skeleton_msg.data = "Hi User %s. Make the secret pose ..." %(id)
        skeleton_msg_pub.publish(skeleton_msg)
        pose_cap.start_detection(POSE2USE, id)

    def lost_user(src, id):
        skeleton_msg = CustomString()
        skeleton_msg.header = Header()
        skeleton_msg.header.stamp = rospy.Time.now()
        skeleton_msg.data = "Bye Bye User %s" %(id)
        skeleton_msg_pub.publish(skeleton_msg)

    def pose_detected(src, pose, id):
        skeleton_msg = CustomString()
        skeleton_msg.header = Header()
        skeleton_msg.header.stamp = rospy.Time.now()
        skeleton_msg.data = "The User %s is doing the secret pose %s, now do the calibration" %(id, pose)
        skeleton_msg_pub.publish(skeleton_msg)
        pose_cap.stop_detection(id)
        skel_cap.request_calibration(id, True)

    def calibration_complete(src, id, status):
        skeleton_msg = CustomString()
        skeleton_msg.header = Header()
        skeleton_msg.header.stamp = rospy.Time.now()
        if status == opi.CALIBRATION_STATUS_OK:
            skeleton_msg.data = "Congrats User %s! You're Calibrated" %(id)
            skeleton_msg_pub.publish(skeleton_msg)
            skel_cap.start_tracking(id)
        else:
            skeleton_msg.data = "Something went wrong User %s :(" %(id)
            skeleton_msg_pub.publish(skeleton_msg)
            new_user(user, id)


    # #### Register callbacks ...

    gesture_generator.register_gesture_cb(gesture_detected, gesture_progress)
    user.register_user_cb(new_user, lost_user)
    pose_cap.register_pose_detected_cb(pose_detected)
    skel_cap.register_c_complete_cb(calibration_complete)
    skel_cap.set_profile(opi.SKEL_PROFILE_ALL)


    # #### Converting and publishing captured data

    def capture_rgb():
        image = CustomString()
        image.header = Header()
        image.header.stamp = rospy.Time.now()
        image_str = image_generator.get_raw_image_map_bgr()
        image.data = image_str
        rgb_pub.publish(image)
        frame = np.fromstring(image_str, dtype=np.uint8).reshape(480, 640, 3)
        try:
            imgmsg = bridge.cv2_to_imgmsg(frame, "bgr8")
            imgmsg.header.stamp = rospy.Time.now()
            rgb_pub2.publish(imgmsg)
        except CvBridgeError as e:
            print(e)

    def capture_depth():
        depth = CustomString()
        depth.header = Header()
        depth.header.stamp = rospy.Time.now()
        depth.data = depth_generator.get_raw_depth_map_8()
        depth_pub.publish(depth) 

    def get_joints():
        for id in user.users:
            if skel_cap.is_tracking(id) and skel_cap.is_calibrated(id):
                joints = [skel_cap.get_joint_position(id, j)
                      for j in map(lambda a: getattr(opi, a), name_joints)]

                newpos_skeleton = depth_generator.to_projective([j.point for j in joints])
                if newpos_skeleton:
                    skeleton_msg = CustomString()
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
