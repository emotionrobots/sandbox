#!/usr/bin/env python

import rospy
from ros_rp2w.msg import Packet
# from std_msgs.msg import String

def callback_rp2w(data):
    print "Left motor speed: %s"%data.leftMotorSpeed
    print "Right motor speed: %s"%data.rightMotorSpeed
    print "Camera Tilt: %s"%data.cameraTilt
    print "Camera Pan: %s"%data.cameraPan;
    print "Digital1: %s"%data.digital1;
    print "Digital2: %s"%data.digital2;

    print "Encoder A: %s"%data.encoderA
    print "Encoder B: %s"%data.encoderB
    print "Battery Voltage: %s"%data.batteryVoltage
    print "Front Sonar: %s"%data.frontSonar
    print "Rear Sonar: %s"%data.rearSonar
    print "Bumper: %s"%data.bumper

if __name__ == '__main__':
    rospy.init_node('listener')
    rospy.Subscriber('rp2w_packet', Packet, callback_rp2w)
    rospy.spin()