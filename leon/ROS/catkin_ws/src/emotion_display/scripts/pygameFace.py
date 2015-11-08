#!/usr/bin/env python2
import pygame
from pygame.locals import*
import math
import rospy
from array import*
from std_msgs.msg import String

global emotion 
global emotions
global emotionamts
emotion = "happy"
emotions = ["anger", "disgust", "fear", "happy", "neutral", "sad", "surprise"]
emotionamts = [0, 0, 0, 0, 0, 0, 0]
pos = -1


def callback(data):
	text = str(data)
	if(text.index(" ",0,len(text))>-1):
		index = text.index("['",0,len(text))+2
		index2 = text.index(", ", 0, len(text))
		emotion = text[index:index2-2]
		for x in xrange(0, 7):
			if(emotions[x] == emotion):
				emotionamts[x] = float(text[index2+2:])/float(100)
				global pos
				pos = x

	display()
	print emotion +" "+ str(emotionamts) +" "+str(pos)

def listener():
	rospy.init_node('emotiondisplay', anonymous=True)
	rospy.Subscriber("landmark", String, callback)
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
	screen = pygame.display.set_mode((640,480))

	running = True
	eyecoordx, eyecoordy = 0, 0

	currentemotion = ""
# 	while running:
	# for e in pygame.event.get():
	# 	if(e.type == KEYDOWN):
	# 		if(e.key == K_ESCAPE):
	# 			running = False
	# 	if(e.type == QUIT):
	# 		running = False

	# listener()
	print str(pos)
	screen.fill((255, 255, 255))
	#Head of Robot
	pygame.draw.rect(screen, (0, 0, 255), (75, 60, 490, 360), 15)
	#Eyes of Robot
	if(pos == 0):
		pygame.draw.circle(screen, (0, 0, 255), (420 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.circle(screen, (0, 0, 255), (200 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.lines(screen, (0, 0, 255), False, [(130, 120), (310, 170), (490, 120)], 20)
	elif(pos == 1):
		pygame.draw.circle(screen, (0, 0, 255), (190 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.circle(screen, (0, 0, 255), (440 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.line(screen, (0, 0, 255), (140, 130), (280, 180), 25)
		pygame.draw.line(screen, (0, 0, 255), (490, 110), (350, 160), 25)
	elif(pos == 2):
		pygame.draw.circle(screen, (0, 0, 255), (220, 200), 10 + int(emotionamts[3]*40))
		pygame.draw.circle(screen, (0, 0, 255), (400, 200), 10 + int(emotionamts[3]*40))
		pygame.draw.circle(screen, (255, 255, 255), (220 + eyecoordx, 200 + eyecoordy), 2+ int(emotionamts[3]*8))
		pygame.draw.circle(screen, (255, 255, 255), (400 + eyecoordx, 200 + eyecoordy), 2+ int(emotionamts[3]*8))
	elif(pos == 5):
		pygame.draw.line(screen, (0, 0, 255), (130, 150), (270, 150), 30);
		pygame.draw.line(screen, (0, 0, 255), (350, 150), (490, 150), 30);
		pygame.draw.line(screen, (0, 0, 255), (200, 150), (200, 150 + int(emotionamts[6]*90)), 20);
		pygame.draw.line(screen, (0, 0, 255), (420, 150), (420, 150 + int(emotionamts[6]*90)), 20);
	elif(pos == 6):
		pygame.draw.line(screen, (0, 0, 255), (130, 200), (270, 200), 5)
		pygame.draw.line(screen, (0, 0, 255), (130, 160), (270, 200), 5)
		pygame.draw.line(screen, (0, 0, 255), (130, 240), (270, 200), 5)
		pygame.draw.line(screen, (0, 0, 255), (350, 200), (490, 200), 5)
		pygame.draw.line(screen, (0, 0, 255), (350, 200), (490, 160), 5)
		pygame.draw.line(screen, (0, 0, 255), (350, 200), (490, 240), 5)
	else :
		pygame.draw.circle(screen, (0, 0, 255), (190 + eyecoordx, 150 + eyecoordy), 25)
		pygame.draw.circle(screen, (0, 0, 255), (440 + eyecoordx, 150 + eyecoordy), 25)

	#Mouth of Robot
	if(pos == 0):
		pygame.draw.lines(screen, (0, 0, 255), False, [(160, int(310+50*emotionamts[5])), (320, 310), (480, int(310+50*emotionamts[5]))], 20)
	elif(pos == 1):
		for x in xrange(0,5):
			for y in xrange(1, 360):
				pygame.draw.line(screen, (0, 0, 255), (160 + 60*x + int(math.ceil(y*math.pi/180*10)), 330 + int(math.sin(y*math.pi/180)*int(5+15*emotionamts[0]))), (160 + 60*x + int(math.ceil((y+1)*math.pi/180*10)), 330 + int(math.sin((y+1)*math.pi/180)*int(5+15*emotionamts[0]))),20)
	elif(pos == 2):
		pygame.draw.circle(screen, (0, 0, 255), (305, 330), 10 + int(55*emotionamts[3]))
	elif(pos == 3):
		pygame.draw.line(screen, (0, 0, 255), (280 - int(emotionamts[2]*120), 330), (360 + int(emotionamts[2]*120), 330), 30)
		pygame.draw.line(screen, (0, 0, 255), (280 - int(emotionamts[2]*120), 290 - emotionamts[2]*60), (280 - int(emotionamts[2]*120), 330), 30)
		pygame.draw.line(screen, (0, 0, 255), (360 + int(emotionamts[2]*120), 290 - emotionamts[2]*60), (360 + int(emotionamts[2]*120), 330), 30)
	elif(pos == 4):
		pygame.draw.line(screen, (0, 0, 255), (160, 330), (480, 330), 30)
	elif(pos == 5):
		pygame.draw.line(screen, (0, 0, 255), (160, 300), (480, 300), 30)
		pygame.draw.line(screen, (0, 0, 255), (160, 300), (160, 360), 30)
		pygame.draw.line(screen, (0, 0, 255), (480, 300), (480, 360), 30)
	elif(pos == 6):
		pygame.draw.circle(screen, (0, 0, 255), (315, 330), 5 + int(55*emotionamts[4]))
	pygame.display.flip()
	publisher(True)

def main():
	# display()
	listener()

if __name__ == '__main__':
    main()