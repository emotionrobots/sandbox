#!/usr/bin/env python

import sys
import rospy

def pick_music_client(song):
    rospy.wait_for_service('pick_music')
    try:
        add_two_ints = rospy.ServiceProxy('pickmusic', music_genre)
        return '-1'
    except rospy.ServiceException, e:
        print "Service call failed: %s"%e
if __name__ == "__main__":
    print 'okayyy'