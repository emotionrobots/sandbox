#!/usr/bin/env python

import numpy as np
import rospy
import pygame
import cv2
import ast
from skeleton.msg import CustomString
from std_msgs.msg import String, Header
from pygame.locals import KEYDOWN, K_ESCAPE
import Image


def callback_rgb(msg):
    #print msg.header.stamp
    global frame
    frame = np.fromstring(msg.data, dtype=np.uint8).reshape(480, 640, 3)
    cv2.imshow('Frame', frame)
    cv2.waitKey(3)

def callback_depth(msg):
    # print msg.header.stamp
    frame = np.fromstring(msg.data, dtype=np.uint8).reshape(480, 640)

def callback_gest(msg):
    # print msg.header.stamp
    print msg.data

def callback_skeleton(msg):
    #print msg.header.stamp
    newpos_skeleton = ast.literal_eval(msg.data) 
    screen = pygame.display.set_mode((640, 480), pygame.HWSURFACE | pygame.DOUBLEBUF)
    pygame.display.set_caption('Skeleton View')
    running=True
    count=0
    while running and count<1:
	    for event in pygame.event.get():
	        if event.type == KEYDOWN and event.key == K_ESCAPE: 
	        	runnning=False
	    screen.blit(capture_rgb(), (0, 0))
	    draw_skeleton(screen, newpos_skeleton)
	    pygame.display.update()
	    pygame.display.flip()
	    count=count+1


def draw_skeleton(screen, pos):
	if (pos):
		# Upper extremeties
		sk_head = 0
		sk_left_foot = 1
		sk_right_shoulder= 2
		sk_left_hand= 3
		sk_neck	= 4
		sk_right_foot= 5
		sk_left_hip = 6
		sk_right_hand	= 7
		sk_torso = 8
		sk_left_elbow 	= 9
		sk_left_knee 	= 10 
		sk_right_hip 	= 11 
		sk_left_shoulder= 12 
		sk_right_elbow 	= 13 
		sk_right_knee 	= 14 
		startpos = pos[sk_right_shoulder] 
		endpos =   pos[sk_left_shoulder] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		pygame.draw.circle(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), 10, 4) 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		startpos = pos[sk_left_shoulder] 
		endpos =   pos[sk_torso] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_torso] 
		endpos =   pos[sk_right_shoulder] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_right_shoulder] 
		endpos =   pos[sk_neck] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_left_shoulder] 
		endpos =   pos[sk_neck] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_neck] 
		endpos =   pos[sk_head] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_right_shoulder] 
		endpos =   pos[sk_right_elbow] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_right_elbow] 
		endpos =   pos[sk_right_hand] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_left_shoulder] 
		endpos =   pos[sk_left_elbow] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_left_elbow] 
		endpos =   pos[sk_left_hand] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 

		# Lower extremeties
		startpos = pos[sk_torso] 
		endpos =   pos[sk_right_hip] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_torso] 
		endpos =   pos[sk_left_hip] 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_right_hip] 
		endpos =   pos[sk_left_hip] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		startpos = pos[sk_right_hip] 
		endpos =   pos[sk_right_knee] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		startpos = pos[sk_right_knee] 
		endpos =   pos[sk_right_foot] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		startpos = pos[sk_left_hip] 
		endpos =   pos[sk_left_knee] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
		startpos = pos[sk_left_knee] 
		endpos =   pos[sk_left_foot] 
		pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
		pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 

def capture_rgb():
    im = Image.fromarray(frame)
    b, g, r = im.split()
    im = Image.merge("RGB", (r, g, b))
    return pygame.image.frombuffer(im.tostring(), im.size, 'RGB')

def callback_skeleton_msg(msg):
    # print msg.header.stamp
    print msg.data

if __name__ == '__main__':
    pygame.init()
    rospy.init_node('listener')
    rospy.Subscriber('rgb', CustomString, callback_rgb)
    rospy.Subscriber('depth', CustomString, callback_depth)
    rospy.Subscriber('gesture', CustomString, callback_gest)
    rospy.Subscriber('skeleton', CustomString, callback_skeleton)
    rospy.Subscriber('skeleton_msg', CustomString, callback_skeleton_msg)
    rospy.spin()
