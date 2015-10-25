import pygame
import math
from array import*

screen = pygame.display.set_mode((640,480))
running = True
anger, neutral, surprise, disgust, fear, sadness, happy = (False, )*7
fear = True
eyecoordx, eyecoordy = 0, 0
happyamt, angeramt, surpriseamt, disgustamt, fearamt, sadnessamt = (0, )*6


while running:
	event = pygame.event.poll();
	if event.type == pygame.QUIT:
		running = False
	screen.fill((255, 255, 255))
	#Head of Robot
	pygame.draw.rect(screen, (0, 0, 0), (75, 60, 490, 360), 15)
	if(fearamt<1):
		fearamt += .0001
	#Eyes of Robot
	if(disgust):
		pygame.draw.circle(screen, (0, 0, 0), (190 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.circle(screen, (0, 0, 0), (440 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.line(screen, (0, 0, 0), (140, 130), (280, 180), 25)
		pygame.draw.line(screen, (0, 0, 0), (490, 110), (350, 160), 25)
	elif(surprise):
		pygame.draw.line(screen, (0, 0, 0), (130, 200), (270, 200), 5)
		pygame.draw.line(screen, (0, 0, 0), (130, 160), (270, 200), 5)
		pygame.draw.line(screen, (0, 0, 0), (130, 240), (270, 200), 5)
		pygame.draw.line(screen, (0, 0, 0), (350, 200), (490, 200), 5)
		pygame.draw.line(screen, (0, 0, 0), (350, 200), (490, 160), 5)
		pygame.draw.line(screen, (0, 0, 0), (350, 200), (490, 240), 5)
	elif(anger):
		pygame.draw.circle(screen, (0, 0, 0), (420 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.circle(screen, (0, 0, 0), (200 + eyecoordx, 200 + eyecoordy), 25)
		pygame.draw.lines(screen, (0, 0, 0), False, [(130, 120), (310, 170), (490, 120)], 20)
	elif(sadness):
		pygame.draw.line(screen, (0, 0, 0), (130, 150), (270, 150), 30);
		pygame.draw.line(screen, (0, 0, 0), (350, 150), (490, 150), 30);
		pygame.draw.line(screen, (0, 0, 0), (200, 150), (200, 150 + int(sadnessamt*90)), 20);
		pygame.draw.line(screen, (0, 0, 0), (420, 150), (420, 150 + int(sadnessamt*90)), 20);
	elif(fear):
		pygame.draw.circle(screen, (0, 0, 0), (200 + eyecoordx, 200 + eyecoordy), 10 + int(fearamt*70));
		pygame.draw.circle(screen, (0, 0, 0), (420 + eyecoordx, 200 + eyecoordy), 10 + int(fearamt*70));
	else :
		pygame.draw.circle(screen, (0, 0, 0), (190 + eyecoordx, 150 + eyecoordy), 25)
		pygame.draw.circle(screen, (0, 0, 0), (440 + eyecoordx, 150 + eyecoordy), 25)

	#Mouth of Robot
	if(neutral):
		pygame.draw.line(screen, (0, 0, 0), (160, 330), (480, 330), 30)
	elif(happy):
		pygame.draw.line(screen, (0, 0, 0), (320 - int(happyamt*160), 330), (320 + int(happyamt*160), 330), 30)
		pygame.draw.line(screen, (0, 0, 0), (320 - int(happyamt*160), 320 - happyamt*90), (320 - int(happyamt*160), 330), 30)
		pygame.draw.line(screen, (0, 0, 0), (320 + int(happyamt*160), 320 - happyamt*90), (320 + int(happyamt*160), 330), 30)
	elif(sadness):
		pygame.draw.line(screen, (0, 0, 0), (160, 300), (480, 300), 30)
		pygame.draw.line(screen, (0, 0, 0), (160, 300), (160, 360), 30)
		pygame.draw.line(screen, (0, 0, 0), (480, 300), (480, 360), 30)
	elif(disgust):
		for x in xrange(0,5):
			for y in xrange(1, 360):
				pygame.draw.line(screen, (0, 0, 0), (160 + 60*x + int(math.ceil(y*math.pi/180*10)), 330 + int(math.sin(y*math.pi/180)*int(5+15*disgustamt))), (160 + 60*x + int(math.ceil((y+1)*math.pi/180*10)), 330 + int(math.sin((y+1)*math.pi/180)*int(5+15*disgustamt))),20)
	elif(surprise):
		pygame.draw.circle(screen, (0, 0, 0), (315, 330), int(60*surpriseamt))
	elif(anger):
		pygame.draw.lines(screen, (0, 0, 0), False, [(160, int(310+50*angeramt)), (320, 310), (480, int(310+50*angeramt))], 20)
	elif(fear):
		pygame.draw.circle(screen, (0, 0, 0), (315, 330), 5 + int(40*fearamt))

	pygame.display.flip()

