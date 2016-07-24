#!/usr/bin/env python

import sys
import rospy
import os
import ast
import soundcloud

def edit_music(song):
    f = open('/home/alex/catkin_ws/src/music/scripts/metadata.txt','r')
    lines = f.readlines()
    f.close()

    song_name = raw_input("Please enter new song name: ")
    artist_name = raw_input("Please enter new artist name: ")
    album_name = raw_input("Please enter new album name: ")
    genre = raw_input("Please enter new song genre: ")
    metadata = '{\'genre\': \'' + genre + '\', \'song_name\': \'' + song_name + '\', \'album_name\': \'' + album_name + '\', \'artist_name\': \''  + genre + '\'}'
    f = open('/home/alex/catkin_ws/src/music/scripts/metadata.txt','w')
    for line in lines:
	data = ast.literal_eval(line)
        if data['song_name'] != song:
    	    f.write(line)
    f.write(metadata)
#    print metadata
#    with open('/home/alex/catkin_ws/src/music/scripts/metadata.txt', 'a') as inputfile:
#	print 'cheese'
#	inputfile.write(metadata)

if __name__ == "__main__":
    x = len(sys.argv)
    y = sys.argv[1:x]
    z = ''
    for n in range(0, x-2):
	z += y[n] + ' '
    z += y[x-2]
    edit_music(z)
