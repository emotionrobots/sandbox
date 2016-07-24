#!/usr/bin/env python

import sys
import rospy
import os
import soundcloud

def add_music():
    song_name = raw_input("Please enter song name: ")
    artist_name = raw_input("Please enter artist name: ")
    album_name = raw_input("Please enter album name: ")
    genre = raw_input("Please enter song genre: ")
    metadata = '{\'genre\': \'' + genre + '\', \'song_name\': \'' + song_name + '\', \'album_name\': \'' + album_name + '\', \'artist_name\': \''  + genre + '\'}'
#    print metadata
    with open('/home/alex/catkin_ws/src/music/scripts/metadata.txt', 'a') as inputfile:
#	print 'cheese'
	inputfile.write(metadata)

if __name__ == "__main__":
    add_music()
