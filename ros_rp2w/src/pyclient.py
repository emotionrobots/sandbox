#!/usr/bin/env python

import sys
import rospy
from ros_rp2w.srv import *

def client(a, b, c, d):
    rospy.wait_for_service('rp2w_command')
    try:
        rp2w_command = rospy.ServiceProxy('rp2w_command', Command)
        resp = rp2w_command(rightMotorSpeedCommand = True, rightMotorSpeed = a, 
                            leftMotorSpeedCommand = True, leftMotorSpeed = b, 
                            cameraTiltCommand = True, cameraTilt = c, 
                            cameraPanCommand = True, cameraPan = d)
        print resp.commandSuccessful
    except rospy.ServiceException, e:
        print "Service call failed: %s"%e

if __name__ == "__main__":
    if len(sys.argv) == 5:
        a = int(sys.argv[1])
        b = int(sys.argv[2])
        c = int(sys.argv[3])
        d = int(sys.argv[4])
    else:
        print "Need 5 arguments. Only have %s. "%len(sys.argv)
        sys.exit(1)
    client(a, b, c, d)

