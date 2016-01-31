#!/usr/bin/env python2
import pygame
from pygame.locals import*
import math
import rospy
from array import*
from std_msgs.msg import String
import thread
import time
import threading

global emotion 
global emotions
global emotionamts
global pos
emotion = "happy"
emotions = ["anger", "disgust", "fear", "happy", "neutral", "sad", "surprise"]
emotionamts = [0, 0, 0, 0, 0, 0, 0]
pos = 4
direcpos = 4
running = True
wanted = 0
lock = threading.Lock()
pygame.init()
screen = pygame.display.set_mode((640,480))


def callback(data):
	with lock:
		text = str(data.data)
		index = text.index(" ",0,len(text))
		emotion = text[:index];
		print emotion
		for x in xrange(0, 7):
			if(emotions[x] == emotion):
				global wanted
				wanted = float(text[index+1:])
				global direcpos
				global pos
				direcpos = x
				if(pos == -1):
					pos = x
					emotionamts[pos] = wanted
	# print emotion +" "+ str(emotionamts) +" "+str(pos)

def listener():
	rospy.init_node('emotiondisplay', anonymous=True)
	rospy.Subscriber("emotiondisplay2", String, callback)
	rospy.spin()


def publisher(done):
	pub = rospy.Publisher('pygameFace', String,queue_size=1)
	# rospy.init_node('emotiondisplay2', anonymous=True)
	msg=String()
	msg.data= str(done)
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)


def display():
	
	global screen
	running = True
	eyecoordx, eyecoordy = 0, 0
	print str(pos)+ " "+ str(emotionamts[pos])+ " "+str(direcpos)+ " " + str(wanted)
	# while running:
	# 	for e in pygame.event.get():
	# 		if(e.type == KEYDOWN):
	# 			if(e.key == K_ESCAPE):
	# 				running = False
	# 		if(e.type == QUIT):
	# 			running = False

		# listener()
		# print str(pos)
	screen.fill((255, 255, 255))
	#Head of Robot
	pygame.draw.arc(screen, (0, 0, 0), (155, 145, 30, 30), math.pi/2, math.pi, 5)
	pygame.draw.arc(screen, (0, 0, 0), (425, 145, 30, 30), 0, math.pi/2, 5)
	pygame.draw.arc(screen, (0, 0, 0), (155, 415, 30, 30), math.pi, math.pi + math.pi/2, 5)
	pygame.draw.arc(screen, (0, 0, 0), (425, 415, 30, 30), math.pi+math.pi/2, math.pi*2, 5)
	pygame.draw.line(screen, (0, 0, 0), (170, 145), (440, 145), 5)
	pygame.draw.line(screen, (0, 0, 0), (155, 160), (155, 430), 5)
	pygame.draw.line(screen, (0, 0, 0), (455, 160), (455, 430), 5)
	pygame.draw.line(screen, (0, 0, 0), (170, 445), (440, 445), 5)

	#Antenna of Robot
	pygame.draw.line(screen, (0, 0, 0), (305, 145), (305, 115), 15)
	if(pos == 3 or pos == 6):
		pygame.draw.line(screen, (0, 0, 0), (305, 115), (305 + int(15 * (emotionamts[pos])), 65 + int(15 * (emotionamts[pos]))), 15)
		pygame.draw.circle(screen, (0, 0, 0), (305 + int(15 * (emotionamts[pos])), 65 + int(15 * (emotionamts[pos]))), 15)
	elif(pos == 4):
		pygame.draw.line(screen, (0, 0, 0), (305, 115), (305, 65), 15)
		pygame.draw.circle(screen, (0, 0, 0), (305, 65), 15)
	else:
		pygame.draw.line(screen, (0, 0, 0), (305, 115), (305 - int(15 * emotionamts[pos]), 65 + int(15 * emotionamts[pos])), 15)
		pygame.draw.circle(screen, (0, 0, 0), (305 - int(15 * emotionamts[pos]), 65 + int(15 * emotionamts[pos])), 15)


	#Eyes of Robot
	if(pos == 1):
		pygame.draw.ellipse(screen, (0, 0, 0), (240 + eyecoordx, 235 + int(8 * emotionamts[1]) + eyecoordy, 30, 30 - int(16 * emotionamts[1])), 0)
		pygame.draw.ellipse(screen, (0, 0, 0), (340 + eyecoordx, 235 + int(8 * emotionamts[1]) + eyecoordy, 30, 30 - int(16 * emotionamts[1])), 0)
	elif(pos == 2):
		pygame.draw.ellipse(screen, (0, 0, 0), (240 - int(8 * emotionamts[2]) + eyecoordx, 235 + eyecoordy, 30 + int(16 * emotionamts[2]), 30), 0)
		pygame.draw.ellipse(screen, (0, 0, 0), (340 - int(8 * emotionamts[2]) + eyecoordx, 235 + eyecoordy, 30 + int(16 * emotionamts[2]), 30), 0)
	elif(pos == 6):
		pygame.draw.ellipse(screen, (0, 0, 0), (240 + eyecoordx, 235 - int(6 * emotionamts[6]) + eyecoordy, 30, 30 + int(12 * emotionamts[6])), 0)
		pygame.draw.ellipse(screen, (0, 0, 0), (340 + eyecoordx, 235 - int(6 * emotionamts[6]) + eyecoordy, 30 ,30 + int(12 * emotionamts[6])), 0)
	else:
		pygame.draw.circle(screen, (0, 0, 0), (255 + eyecoordx, 250 + eyecoordy), 15)
		pygame.draw.circle(screen, (0, 0, 0), (355 + eyecoordx, 250 + eyecoordy), 15)
		if(pos == 0):
			pygame.draw.polygon(screen, (255, 255, 255), [(270 + eyecoordx, 235 + (30 * emotionamts[0]) + eyecoordy), (270 + eyecoordx, 235 + eyecoordy), (270 - (30 * emotionamts[0]) + eyecoordx, 235 + eyecoordy)], 0)
			pygame.draw.polygon(screen, (255, 255, 255), [(340 + eyecoordx, 235 + eyecoordy), (340 + eyecoordx, 235 + (30 * emotionamts[0]) + eyecoordy), (340 + (30 * emotionamts[0]) + eyecoordx, 235 + eyecoordy)], 0)
		elif(pos == 5):
			pygame.draw.polygon(screen, (255, 255, 255), [(240 + eyecoordx, 235 + eyecoordy), (240 + (30 * emotionamts[5]) + eyecoordx, 235 + eyecoordy), (240 + eyecoordx, 235 + (30 * emotionamts[5]) + eyecoordy)], 0)
			pygame.draw.polygon(screen, (255, 255, 255), [(370 - (30 * emotionamts[5]) + eyecoordx, 235 + eyecoordy), (370 + eyecoordx, 235 + eyecoordy), (370 + eyecoordx, 235 + (30 * emotionamts[5]) + eyecoordy)], 0)


	# #Mouth of Robot
	if(pos == 0):
		pygame.draw.lines(screen, (0, 0, 0), False, [(240, 355 + int(20*emotionamts[0])), (305, 355), (370, 355+int(20*emotionamts[0]))], 5)
	elif(pos == 1):
		for x in xrange(0,4):
			for y in xrange(1, 360):
				pygame.draw.line(screen, (0, 0, 0), (240 + 10 * math.pi * x + int(math.ceil(y*math.pi/180*5)), 355 + int(math.sin(y*math.pi/180)*int(10*emotionamts[1]))), (240 + 10 * math.pi *x + int(math.ceil((y+1)*math.pi/180*5)), 355 + int(math.sin((y+1)*math.pi/180)*int(10*emotionamts[1]))),5)
	elif(pos == 2 or pos == 6):
		if(emotionamts[pos] == 0):
			pygame.draw.line(screen, (0, 0, 0), (240, 355), (370, 355), 5)
		else:	
			pygame.draw.ellipse(screen, (0, 0, 0), (240 + int(33 * emotionamts[pos]), 355 - int(33 * emotionamts[pos]), 125 - int(65 * emotionamts[pos]), 5 + int(65 * emotionamts[pos])), 0)
	elif(pos == 3):
		pygame.draw.line(screen, (0, 0, 0), (240, 355), (370, 355), 5)
		pygame.draw.line(screen, (0, 0, 0), (240, 355), (240, 355 - int(25 * emotionamts[3])), 5)
		pygame.draw.line(screen, (0, 0, 0), (370, 355), (370, 355 - int(25 * emotionamts[3])), 5)			
	elif(pos == 4):
		pygame.draw.line(screen, (0, 0, 0), (240, 355), (370, 355), 5)
	elif(pos == 5):
		pygame.draw.line(screen, (0, 0, 0), (240, 355), (370, 355), 5)
		pygame.draw.line(screen, (0, 0, 0), (240, 355), (240, 355 + int(15 * emotionamts[5])), 5)
		pygame.draw.line(screen, (0, 0, 0), (370, 355), (370, 355 + int(15 * emotionamts[5])), 5)

	pygame.display.flip()

def func(delay):
	print "hey"
	done = False
	first = True
	while(not done):
		with lock:
			display()	
			if(not first):
				global pos
				if(direcpos != pos):
					emotionamts[pos] -= .05
				else:
					if(emotionamts[pos] < wanted+.05 and emotionamts[pos] > wanted-.05):
						emotionamts[pos] = wanted
					if(emotionamts[pos] < wanted):
						emotionamts[pos] += .05
					if(emotionamts[pos] > wanted):
						emotionamts[pos] -= .05
				if(emotionamts[pos] <= 0):
					pos = direcpos
			else:
				first = False
		time.sleep(delay)

def main():
	print "hi"
	try:
		thread.start_new_thread(func, (.01,)) 
		listener()
	except Exception, e:
		print str(e)

if __name__ == '__main__':
    main()