#!/usr/bin/env python

import sys
import rospy
from ros_rp2w.srv import *

if __name__ == "__main__":
    if len(sys.argv) == 7:
        a = int(sys.argv[3])
        b = int(sys.argv[4])
        c = int(sys.argv[5])
        d = int(sys.argv[6])
    else:
        sys.exit(1)
    rospy.wait_for_service('rp2w_command')
    try:
        rp2w_command = rospy.ServiceProxy('rp2w_command', AddTwoInts)
        resp = rp2w_command(rightMotorSpeedCommand = false, rightMotorSpeed = a, 
                            leftMotorSpeedCommand = false, leftMotorSpeed = b, 
                            cameraTiltCommand = false, cameraTilt = c, 
                            cameraPanCommand = false, cameraPan = d)
        print resp.commandSuccessful
    except rospy.ServiceException, e:
        print "Service call failed: %s"%e