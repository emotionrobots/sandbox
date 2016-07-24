#!/usr/bin/env python

from music.srv import *
import rospy

def choose_music(req):
    print 'choose music'

def pick_music_server():
    rospy.init_node('pick_music_server')
    s = rospy.Service('pickmusic', musicgenre, choose_music)
    print "What genre do you want"
    rospy.spin()

if __name__ == "__main__":
    pick_music_server()
