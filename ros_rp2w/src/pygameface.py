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
infoObject = pygame.display.Info()
scalex = infoObject.current_w / 640.0
scaley = infoObject.current_h / 480.0
screen = pygame.display.set_mode((int(640*scalex),int(480*scaley)))


def callback(data):
	with lock:
		text = str(data.data)
		# print text
		index = text.index(" ",0,len(text))
		emotion = text[:index];
		print emotion
		for x in xrange(0, 7):
			if(emotions[x] == emotion):
				global wanted
				wanted = float(text[index+1:])/100.0
				global direcpos
				global pos
				direcpos = x
				if(pos == -1):
					pos = x
					emotionamts[pos] = wanted
				# print wanted
	# print emotion +" "+ str(emotionamts) +" "+str(pos) + " "+str(wanted)

def listener():
	rospy.init_node('emotiondisplay', anonymous=True)
	rospy.Subscriber("emotion", String, callback)
	rospy.spin()


def publisher(done):
	pub = rospy.Publisher('pygameFace', String,queue_size=1)
	msg=String()
	msg.data= str(done)
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		pub.publish(msg)


def display():
	global screen
	running = True
	eyecoordx, eyecoordy = 0, 0
	# while running:
	# 	for e in pygame.event.get():
	# 		if(e.type == KEYDOWN):
	# 			if(e.key == K_ESCAPE):
	# 				running = False
	# 		if(e.type == QUIT):
	# 			running = False

		# listener()
	# print str(pos)
	# print str(emotionamts[pos])
	screen.fill((255, 255, 255))
	#Head of Robot
	pygame.draw.arc(screen, (0, 0, 0), (int(153*scalex), int(143*scaley), int(35*scalex), int(35*scaley)), math.pi/2, math.pi, int(5*((scalex+scaley)/2)))
	pygame.draw.arc(screen, (0, 0, 0), (int(422*scalex), int(143*scaley), int(35*scalex), int(35*scaley)), 0, math.pi/2, int(5*((scalex+scaley)/2)))
	pygame.draw.arc(screen, (0, 0, 0), (int(153*scalex), int(413*scaley), int(35*scalex), int(35*scaley)), math.pi, math.pi + math.pi/2, int(5*((scalex+scaley)/2)))
	pygame.draw.arc(screen, (0, 0, 0), (int(422*scalex), int(413*scaley), int(35*scalex), int(35*scaley)), math.pi+math.pi/2, math.pi*2, int(5*((scalex+scaley)/2)))
	pygame.draw.line(screen, (0, 0, 0), (int(170*scalex), int(145*scaley)), (int(440*scalex), int(145*scaley)), int(5*((scalex+scaley)/2)))
	pygame.draw.line(screen, (0, 0, 0), (int(155*scalex), int(160*scaley)), (int(155*scalex), int(430*scaley)), int(5*((scalex+scaley)/2)))
	pygame.draw.line(screen, (0, 0, 0), (int(455*scalex), int(160*scaley)), (int(455*scalex), int(430*scaley)), int(5*((scalex+scaley)/2)))
	pygame.draw.line(screen, (0, 0, 0), (int(170*scalex), int(445*scaley)), (int(440*scalex), int(445*scaley)), int(5*((scalex+scaley)/2)))

	#Antenna of Robot
	pygame.draw.line(screen, (0, 0, 0), (int(305*scalex), int(145*scaley)), (int(305*scalex), int(115*scaley)), int(15*((scalex+scaley)/2)))
	if(pos == 3 or pos == 6):
		pygame.draw.line(screen, (0, 0, 0), (int(305*scalex), int(115*scaley)), (int((305 + 15 * emotionamts[pos])*scalex), int((65 + 15 * (emotionamts[pos]))*scaley)), int(15*((scalex+scaley)/2)))
		pygame.draw.circle(screen, (0, 0, 0), (int((305 + 15 * (emotionamts[pos]))*scalex), int((65 + 15 * emotionamts[pos])*scaley)), int(15*((scalex+scaley)/2.0)))
	elif(pos == 4):
		pygame.draw.line(screen, (0, 0, 0), (int(305*scalex), int(115*scaley)), (int(305*scalex), int(65*scaley)), int(15*((scalex+scaley)/2)))
		pygame.draw.circle(screen, (0, 0, 0), (int(305*scalex), int(65*scaley)), int(15*((scalex+scaley)/2)))
	else:
		pygame.draw.line(screen, (0, 0, 0), (int(305*scalex), int(115*scaley)), (int((305 - 15 * emotionamts[pos])*scalex), int((65 + 15 * emotionamts[pos])*scaley)), int(15*((scalex+scaley)/2)))
		pygame.draw.circle(screen, (0, 0, 0), (int((305 - int(15 * emotionamts[pos]))*scalex), int((65 + 15 * emotionamts[pos])*scaley)), int(15*((scalex+scaley)/2.0)))


	#Eyes of Robot
	if(pos == 1):
		pygame.draw.ellipse(screen, (0, 0, 0), (int((240 + eyecoordx)*scalex), int((235 + 8 * emotionamts[1] + eyecoordy)*scaley), int(30*scalex), int((30 - 16 * emotionamts[1])*scaley)), 0)
		pygame.draw.ellipse(screen, (0, 0, 0), (int((340 + eyecoordx)*scalex), int((235 + 8 * emotionamts[1] + eyecoordy)*scaley), int(30*scalex), int((30 - 16 * emotionamts[1])*scaley)), 0)
	elif(pos == 2):
		pygame.draw.ellipse(screen, (0, 0, 0), (int((240 - 8 * emotionamts[2] + eyecoordx)*scalex), int((235 + eyecoordy)*scaley), int((30 + 16 * emotionamts[2])*scalex), int(30*scaley)), 0)
		pygame.draw.ellipse(screen, (0, 0, 0), (int((340 - 8 * emotionamts[2] + eyecoordx)*scalex), int((235 + eyecoordy)*scaley), int((30 + 16 * emotionamts[2])*scalex), int(30*scaley)), 0)
	elif(pos == 6):
		pygame.draw.ellipse(screen, (0, 0, 0), (int((240 + eyecoordx)*scalex), int((235 - 6 * emotionamts[6] + eyecoordy)*scaley), int(30*scalex), int((30 + 12 * emotionamts[6])*scaley)), 0)
		pygame.draw.ellipse(screen, (0, 0, 0), (int((340 + eyecoordx)*scalex), int((235 - 6 * emotionamts[6] + eyecoordy)*scaley), int(30*scalex), int((30 + 12 * emotionamts[6])*scaley)), 0)
	else:
		pygame.draw.circle(screen, (0, 0, 0), (int((255 + eyecoordx)*scalex), int((250 + eyecoordy)*scaley)), int(15*((scalex+scaley)/2)))
		pygame.draw.circle(screen, (0, 0, 0), (int((355 + eyecoordx)*scalex), int((250 + eyecoordy)*scaley)), int(15*((scalex+scaley)/2)))
		if(pos == 0):
			pygame.draw.polygon(screen, (255, 255, 255), [(int((270 + eyecoordx)*scalex), int((235 + 30 * emotionamts[0] + eyecoordy)*scaley)), (int((270 + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2)) + eyecoordy)*scaley)), (int((270 - 30 * emotionamts[0] + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2)) + eyecoordy)*scaley))], 0)
			pygame.draw.polygon(screen, (255, 255, 255), [(int((340 + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2))  + eyecoordy)*scaley)), (int((340 + eyecoordx)*scalex), int((235 + 30 * emotionamts[0] + eyecoordy)*scaley)), (int((340 + (30 * emotionamts[0]) + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2))+ eyecoordy)*scaley))], 0)
		elif(pos == 5):
			pygame.draw.polygon(screen, (255, 255, 255), [(int((240 + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2)) + eyecoordy)*scaley)), (int((240 + (40 * emotionamts[5]) + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2)) + eyecoordy)*scaley)), (int((240 + eyecoordx)*scalex), int((235 + 30 * emotionamts[5] + eyecoordy)*scaley))], 0)
			pygame.draw.polygon(screen, (255, 255, 255), [(int((370 - (40 * emotionamts[5]) + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2)) + eyecoordy)*scaley)), (int((370 + eyecoordx)*scalex), int((235 - int(7.5*((scalex+scaley)/2)) + eyecoordy)*scaley)), (int((370 + eyecoordx)*scalex), int((235 + 30 * emotionamts[5] + eyecoordy)*scaley))], 0)

	# #Mouth of Robot
	if(pos == 0):
		pygame.draw.lines(screen, (0, 0, 0), False, [(int(240*scalex), int((355 + 20*emotionamts[0])*scaley)), (int(305*scalex), int(355*scaley)), (int(370*scalex), int((355+20*emotionamts[0])*scaley))],  int(5*((scalex+scaley)/2)))
	elif(pos == 1):
		for x in xrange(0,4):
			for y in xrange(1, 360):
				pygame.draw.line(screen, (0, 0, 0), (int((240 + 10 * math.pi * x + int(math.ceil(y*math.pi/180*5)))*scalex), int((355 + math.sin(y*math.pi/180)*10*emotionamts[1])*scaley)), (int((240 + 10 * math.pi *x + math.ceil((y+1)*math.pi/180*5))*scalex), int((355 + math.sin((y+1)*math.pi/180)*10*emotionamts[1])*scaley)),int(5*((scalex+scaley)/2)))
	elif(pos == 2 or pos == 6):
		if(emotionamts[pos] == 0):
			pygame.draw.line(screen, (0, 0, 0), (int(240*scalex), int(355*scaley)), (int(370*scalex), int(355*scaley)), int(5*((scalex+scaley)/2)))
		else:	
			pygame.draw.ellipse(screen, (0, 0, 0), (int((240 + 33 * emotionamts[pos])*scalex), int((355 - (33* emotionamts[pos]))*scaley), int((125 - 65 * emotionamts[pos])*scalex), int((5 + 65 * emotionamts[pos])*scaley)), 0)
	elif(pos == 3):
		pygame.draw.line(screen, (0, 0, 0), (int(240*scalex), int(355*scaley)), (int(370*scalex), int(355*scaley)), int(5*((scalex+scaley)/2)))
		pygame.draw.line(screen, (0, 0, 0), (int(240*scalex), int(355*scaley)), (int(240*scalex), int((355 - 25 * emotionamts[3])*scaley)), int(5*((scalex+scaley)/2)))
		pygame.draw.line(screen, (0, 0, 0), (int(370*scalex), int(355*scaley)), (int(370*scalex), int((355 - 25 * emotionamts[3])*scaley)), int(5*((scalex+scaley)/2)))			
	elif(pos == 4):
		pygame.draw.line(screen, (0, 0, 0), (int(240*scalex), int(355*scaley)), (int(370*scalex), int(355*scaley)), int(5*((scalex+scaley)/2)))
	elif(pos == 5):
		pygame.draw.line(screen, (0, 0, 0), (int(240*scalex), int(355*scaley)), (int(370*scalex), int(355*scaley)), int(5*((scalex+scaley)/2)))
		pygame.draw.line(screen, (0, 0, 0), (int(240*scalex), int(355*scaley)), (int(240*scalex), int((355 + 15 * emotionamts[5])*scaley)), int(5*((scalex+scaley)/2)))
		pygame.draw.line(screen, (0, 0, 0), (int(370*scalex), int(355*scaley)), (int(370*scalex), int((355 + 15 * emotionamts[5])*scaley)), int(5*((scalex+scaley)/2)))

	pygame.display.flip()

def func(delay):
	print "Function start"
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
	print "pygameFace start"
	try:
		thread.start_new_thread(func, (.05,)) 
		listener()
	except Exception, e:
		print str(e)

if __name__ == '__main__':
    main()