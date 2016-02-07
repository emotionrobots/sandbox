#!/usr/bin/env python2
import pygame
from pygame.locals import*
import math
import rospy
from array import*
from std_msgs.msg import String
import thread
import time

global emotion 
global emotions
global emotionamts
emotion = "happy"
emotions = ["anger", "disgust", "fear", "happy", "neutral", "sad", "surprise"]
emotionamts = [0, 0, 0, 0, 0, 0, 0]
pos = 4
direcpos = 1;
running = True
wanted = 0
global last
last=''

def callback(data):
	running = True
	count=0
	while running and count<1:
		for event in pygame.event.get():
		    if event.type == KEYDOWN and event.key == K_ESCAPE: 
		    	runnning=False

		text = data
		#print text
		index = text.index(' ',0,len(text))
		emotion = text[:index];
		global last
		#if emotion==last:
		#	break
		last=emotion
		#print emotion
		for x in xrange(0, 7):
			if(emotions[x] == emotion):
				global wanted
				wanted = float(text[index+1:])
				global direcpos
				direcpos = x
		global pos
		if(direcpos != pos):
			if(emotionamts[pos] == 0):
				pos = direcpos
			else:
				emotionamts[pos] -= .1
			if(emotionamts[pos] < 0):
				emotionamts[pos] = 0
		else:
			if(emotionamts[pos] < wanted):
				emotionamts[pos] += .1
			if(emotionamts[pos] > wanted):
				emotionamts[pos] = wanted

		display()
		pygame.display.update()
		pygame.display.flip()	
		count=count+1		
	    
	# print emotion +" "+ str(emotionamts) +" "+str(pos)

def listener():
	while True:
		emotiona=rospy.get_param("emotion")
		callback(emotiona)	
		

	
	#rospy.Subscriber("emotiondisplay2", String, callback)
	#rospy.spin()


def publisher(done):
	
	# rospy.init_node('emotiondisplay2', anonymous=True)
	global pub
	msg=String()
	msg.data= emotion
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)


def display():
	screen = pygame.display.set_mode((640,480))

	
	eyecoordx, eyecoordy = 0, 0

	currentemotion = ""



		# listener()
		# print str(pos)
	
	print str(pos) 
	print str(wanted)
	screen.fill((255, 255, 255))
	#Head of Robot
	pygame.draw.arc(screen, (0, 0, 255), (155, 145, 30, 30), math.pi/2, math.pi, 5)
	pygame.draw.arc(screen, (0, 0, 255), (425, 145, 30, 30), 0, math.pi/2, 5)
	pygame.draw.arc(screen, (0, 0, 255), (155, 415, 30, 30), math.pi, math.pi + math.pi/2, 5)
	pygame.draw.arc(screen, (0, 0, 255), (425, 415, 30, 30), math.pi+math.pi/2, math.pi*2, 5)
	pygame.draw.line(screen, (0, 0, 255), (170, 145), (440, 145), 5)
	pygame.draw.line(screen, (0, 0, 255), (155, 160), (155, 430), 5)
	pygame.draw.line(screen, (0, 0, 255), (455, 160), (455, 430), 5)
	pygame.draw.line(screen, (0, 0, 255), (170, 445), (440, 445), 5)

	#Antenna of Robot
	pygame.draw.line(screen, (0, 0, 255), (305, 145), (305, 115), 15)
	if(pos == 3 or pos == 6):
		pygame.draw.line(screen, (0, 0, 255), (305, 115), (305 + int(15 * (emotionamts[pos])), 65 + int(15 * (emotionamts[pos]))), 15)
		pygame.draw.circle(screen, (0, 0, 255), (305 + int(15 * (emotionamts[pos])), 65 + int(15 * (emotionamts[pos]))), 15)
	if(pos == 4):
		pygame.draw.line(screen, (0, 0, 255), (305, 115), (305, 65), 15)
		pygame.draw.circle(screen, (0, 0, 255), (305, 65), 15)
	else:
		pygame.draw.line(screen, (0, 0, 255), (305, 115), (305 - int(15 * emotionamts[pos]), 65 + int(15 * emotionamts[pos])), 15)
		pygame.draw.circle(screen, (0, 0, 255), (305 - int(15 * emotionamts[pos]), 65 + int(15 * emotionamts[pos])), 15)


	#Eyes of Robot
	if(pos == 1):
		pygame.draw.ellipse(screen, (0, 0, 255), (240 + eyecoordx, 250 + int(7 * emotionamts[1]) + eyecoordy, 30, 30 - int(15 * emotionamts[1])), 0)
		pygame.draw.ellipse(screen, (0, 0, 255), (340 + eyecoordx, 250 + int(7 * emotionamts[1]) + eyecoordy, 30, 30 - int(15 * emotionamts[1])), 0)
	elif(pos == 2):
		pygame.draw.ellipse(screen, (0, 0, 255), (233 + int(7 * (1 - emotionamts[2])) + eyecoordx, 235 + eyecoordy, 30 + int(15 * emotionamts[2]), 30), 0)
		pygame.draw.ellipse(screen, (0, 0, 255), (333 + int(7 * (1 - emotionamts[2])) + eyecoordx, 235 + eyecoordy, 30 + int(15 * emotionamts[2]), 30), 0)
	elif(pos == 6):
		pygame.draw.ellipse(screen, (0, 0, 255), (255 + eyecoordx, 235 + eyecoordy, 30, 30), 0)
		pygame.draw.ellipse(screen, (0, 0, 255), (355 + eyecoordx, 235 + eyecoordy, 30 ,30), 0)
	else:
		pygame.draw.circle(screen, (0, 0, 255), (255 + eyecoordx, 250 + eyecoordy), 15)
		pygame.draw.circle(screen, (0, 0, 255), (355 + eyecoordx, 250 + eyecoordy), 15)
		if(pos == 0):
			pygame.draw.polygon(screen, (255, 255, 255), [(270 + eyecoordx, 235 + (30 * emotionamts[0]) + eyecoordy), (270 + eyecoordx, 235 + eyecoordy), (270 - (30 * emotionamts[0]) + eyecoordx, 235 + eyecoordy)], 0)
			pygame.draw.polygon(screen, (255, 255, 255), [(340 + eyecoordx, 235 + eyecoordy), (340 + eyecoordx, 235 + (30 * emotionamts[0]) + eyecoordy), (340 + (30 * emotionamts[0]) + eyecoordx, 235 + eyecoordy)], 0)
		elif(pos == 5):
			pygame.draw.polygon(screen, (255, 255, 255), [(240 + eyecoordx, 235 + eyecoordy), (240 + (30 * emotionamts[5]) + eyecoordx, 235 + eyecoordy), (240 + eyecoordx, 235 + (30 * emotionamts[5]) + eyecoordy)], 0)
			pygame.draw.polygon(screen, (255, 255, 255), [(370 - (30 * emotionamts[5]) + eyecoordx, 235 + eyecoordy), (370 + eyecoordx, 235 + eyecoordy), (370 + eyecoordx, 235 + (30 * emotionamts[5]) + eyecoordy)], 0)


	# #Mouth of Robot
	if(pos == 0):
		pygame.draw.lines(screen, (0, 0, 255), False, [(240, 355 + int(20*emotionamts[0])), (305, 355), (370, 355+int(20*emotionamts[0]))], 5)
	elif(pos == 1):
		for x in xrange(0,4):
			for y in xrange(1, 360):
				pygame.draw.line(screen, (0, 0, 255), (240 + 10 * math.pi * x + int(math.ceil(y*math.pi/180*5)), 355 + int(math.sin(y*math.pi/180)*int(10*emotionamts[1]))), (240 + 10 * math.pi *x + int(math.ceil((y+1)*math.pi/180*5)), 355 + int(math.sin((y+1)*math.pi/180)*int(10*emotionamts[1]))),5)
	elif(pos == 2 or pos == 6):
		if(emotionamts[pos] == 0):
			pygame.draw.line(screen, (0, 0, 255), (240, 355), (370, 355), 5)
		else:	
			pygame.draw.ellipse(screen, (0, 0, 255), (240 + int(33 * emotionamts[2]), 355 - int(33 * emotionamts[2]), 130 - int(65 * emotionamts[2]), 0 + int(65 * emotionamts[2])), 0)
	elif(pos == 3):
		pygame.draw.line(screen, (0, 0, 255), (240, 355), (370, 355), 5)
		pygame.draw.line(screen, (0, 0, 255), (240, 355), (240, 355 - int(15 * emotionamts[3])), 5)
		pygame.draw.line(screen, (0, 0, 255), (370, 355), (370, 355 - int(15 * emotionamts[3])), 5)			
	elif(pos == 4):
		pygame.draw.line(screen, (0, 0, 255), (240, 355), (370, 355), 5)
	elif(pos == 5):
		pygame.draw.line(screen, (0, 0, 255), (240, 355), (370, 355), 5)
		pygame.draw.line(screen, (0, 0, 255), (240, 355), (240, 355 + int(15 * emotionamts[5])), 5)
		pygame.draw.line(screen, (0, 0, 255), (370, 355), (370, 355 + int(15 * emotionamts[5])), 5)

	#publisher(True)

def main():
	#display()
	pygame.init()
	global pub
	pub = rospy.Publisher('pygameFace', String,queue_size=1)
	rospy.init_node('emotiondisplay', anonymous=True)
	listener()

if __name__ == '__main__':
    main()
