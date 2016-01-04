#!/usr/bin/env python

import rospy
from ros_rp2w.msg import Packet
# from std_msgs.msg import String

def callback_rp2w(data):
    print data.rightMotorSpeed + " "
    print data.leftMotorSpeed + " "
    print cameraTilt + " "
    print cameraPan + " "
    print digital1 + " "
    print digital2 + " "
    print encoderA + " "
    print encoderB + " "
    print batteryVoltage + " "
    print frontSonar + " "
    print rearSonar + " "
    print bumper + "\n"

if __name__ == '__main__':
    rospy.init_node('listener')
    rospy.Subscriber('rp2w_packet', Packet, callback_rp2w)
    rospy.spin()