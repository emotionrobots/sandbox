#!/usr/bin/env python

import sys
import rospy
import os
import soundcloud
import ast
import pygame
from random import randint

def play_music(song):
#    print song
    lines = 0
    global dictionary #holds all the info about 1 song in the metadata file which is all on 1 line
    global num #holds the number of the line randomly picked from metadata file
    global counter #used to determine when the program loop has reached 'num'
    global song_found #used to determine if the requested song has been found, will print song not found if this variable is false
    song_found = False
    counter = 0
#if the person wants to play a random song
    if(song == 'Random' or song == 'random'):
#counts the number of lines in the metadata file
	with open('/home/alex/catkin_ws/src/music/scripts/metadata.txt') as inputfile:
	    for line in inputfile:
		lines += 1
		num = randint(0, lines - 1)
#goes through the metadata file until it reaches the desired line and reassigns the song name (currently random) to the song name in that line
	with open('/home/alex/catkin_ws/src/music/scripts/metadata.txt') as inputfile:
	    for line in inputfile:
		dictionary = ast.literal_eval(line)
		if(counter == num):
		    song = dictionary['song_name']
		    break;
		counter += 1
#    print dictionary
#   print num

    with open('/home/alex/catkin_ws/src/music/scripts/metadata.txt') as inputfile:
        for line in inputfile:
            dictionary = ast.literal_eval(line)   
	    if(dictionary['song_name'] == song and os.path.exists("/home/alex/catkin_ws/src/music/scripts/" + song + ".wav")):
		print 'Song: ' + dictionary['song_name']
		print 'Artist: ' + dictionary['artist_name']
		print 'Album: ' + dictionary['album_name']
		print 'Genre: ' + dictionary['genre']
		song_found = True
		pygame.mixer.init()
		pygame.mixer.music.load('/home/alex/catkin_ws/src/music/scripts/' + song + '.wav')
		pygame.mixer.music.play()
		while pygame.mixer.music.get_busy() == True:
		    continue
    if(song_found == False):
	print 'Song not found'
#		os.system("aplay /home/alex/catkin_ws/src/music/scripts/Flight.wav -R 1")

if __name__ == "__main__":
    x = len(sys.argv)
    y = sys.argv[1:x]
    z = ''
    for n in range(0, x-2):
	z += y[n] + ' '
    z += y[x-2]
    play_music(z)
