#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import time
import thread
import threading
from ros_rp2w.msg import Packet
# from vfacerec.msg import Face
# from vfacerec.msg import UnknownFace
# from vfacerec.srv import SetRate

global name
name = ""
first = True
global val
val = 0
global lock
lock = threading.Lock()
global command
command = -1
global prev
prev = -1
commands= [False for i in range(11)]

def publisher(done):
	pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	msg=String()
	msg.data= done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		print("Message published")
		pub.publish(msg)

def publishMove(done):
	commands[5] = True
	pub2 = rospy.Publisher("rp2w/advanced_command", AdvancedCommand, queue_size=1)
    rospy.init_node('rp2w_publisher', anonymous=True)
    rate = rospy.Rate(10) # 10hz
    # while not rospy.is_shutdown():
    while pub.get_num_connections() == 0:
        rate.sleep()
    msg = AdvancedCommand()
    msg.theta = 360
    msg.distance = 2
    pub.publish(msg)
    print msg

# def callbackFaceMsg(data):
# 	with lock:
# 		print("PublishFaceMsgs received")
# 		commands[0] = False
# 		commands[1] = False
# 		commands[2] = False
# 		commands[8] = False
# 		if(data.data[0] != name):
# 			commands[0] = True
# 		elif(data.data > 1):
# 			commands[1] = True
# 		elif(data.data == 0):
# 			commands[2] = True
# 		elif(data.data[0] == name):
# 			commands[8] = True
# 		func(.5)

def callbackPacket(data):
	with lock:
		print("Packet received")
		commands[3] = False
		com0ommands[4] = False
		if(data.batteryVoltage < 30):
			commands[3] = True
		elif(data.rearSonar <= 2 or data.frontSonar<= 2 or data.bumper<=2):
			commands[4] = True
		elif(data.rightMotorSpeed == 0 and data.leftMotorSpeed == 0):
			commands[5] = False
			publishMove()
		# elif(data.rightMotorSpeed > 0 or data.leftMotorSpeed > 0)
		# 	commands[5] = True
		func(.5)

def callbackSpeech(data):
	with lock:
		print("Chatter received")
		commands[6] = False
		commands[7] = False
		commands[9] = False
		if(data.data == "i quit"):
			commands[6] = True
			print("i quit")
		elif(data.data == "bye"): 
			commands[7] = True
			print("bye")
		elif(data.data == "where would you like me to go next"):
			commands[9] = True
			print("where to?")
		func(.5)


def listener():
	#Dalton input for non-master(disgust), zero faces(increasing sad), and number of faces(surprise)
	rospy.Subscriber("known_faces", Face, callback)
	# Scott RP2W battery(sad), obstacle(surprise), and movement(neutral)
	rospy.Subscriber("rp2w_packet", Packet, callback2)
	# Aurash "I quit"(disgust = 1), master "where would you like me to go"(smile), ask for directions(increasing disgust), "Bye"(happy)
	rospy.Subscriber("chatter", String, callback3)
	rospy.spin()

def func(delay):
	res = ""
	global prev
	global command
	global val
	print (str(prev)+" "+str(command))
	if(not prev == command):
		val = 0
	prev = command
	if(commands[3]):
		res = "sad 1"
		command = 3
	elif(commands[4]):
		res = "surprise 1"
		command = 4
	elif(commands[0]):
		res = "disgust 1"
	elif(commands[2]):
		val+=.2
		if(val >= 1):
			val = 1
		res = "sad " + str(val)
		command = 2
	elif(commands[1]):
		res = "surprise 1"
		command = 1
	elif(commands[5]):
		res = "neutral 1"
		command = 5
	elif(commands[6]):
		res =  "disgust 1"
		command = 6
	elif(commands[7]):
		res = "happy 1"
		command = 7
	elif(commands[8]):
		res = "happy 1"
		command = 8
	elif(commands[9]):
		val+=.33
		if(val >= 1):
			val = 1
		res = "disgust "+str(val)
		command = 9 
	print(res)
	publisher(res)

		
def main():
	rospy.init_node('main_loop', anonymous=True)
	listener() 

if __name__ == '__main__':
	main()

