#!/usr/bin/env python

import sys
import rospy
import os
import soundcloud
import urllib2
import json
import ast
import pygst
pygst.require("0.10")
import gst

#    os.system("aplay /home/alex/catkin_ws/src/music/scripts/Flight.wav -R 1")

def music():
    client = soundcloud.Client(client_id='51f459c0a6b2ee82ce31d9a691e2d6fb',
    client_secret = '08323d9fd262b0e667d35c68ff136848',
    username='azhao6@gmail.com',
    password = 'ecpl00')
    track = client.get('/tracks/258538227')
    stream_url = client.get(track.stream_url, allow_redirects=False)
    print stream_url.location
#    music_stream_uri = 'http://mp3channels.webradio.antenne.de/chillout'
#    player = gst.element_factory_make("playbin", "player")
#    player.set_property('uri', music_stream_uri)
#    player.set_state(gst.STATE_PLAYING)
#    print 'is it playing?'

#    raw_input('Press enter to stop playing...')

if __name__ == "__main__":
    music()
