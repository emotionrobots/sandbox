#!/usr/bin/env python
from skeleton.srv._festTTS import *
import sys
import rospy

def tts_client(string2):
    rospy.wait_for_service('tts')
    try:
        tts = rospy.ServiceProxy('tts', festTTS)
        req = festTTSRequest(string2)
        resp = tts(string2)
        print str(resp)[4:]
    except rospy.ServiceException, e:
        print "Service call failed: %s"%e


if __name__ == "__main__":
    x=sys.argv[1]
    tts_client(x)