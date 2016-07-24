#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import time

def publisher(done):
	pub = rospy.Publisher('chatter', String, queue_size = 10)
	rospy.init_node('dummynode2', anonymous = True)
	msg = String()
	msg.data = done
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		print("Chatter Message Sent")
		pub.publish(msg)
def main():
	cnt = 0
	global st
	st = ""
	while(not rospy.is_shutdown()):
		cnt = (cnt+1)%5
		if(cnt == 0):
			st = "i quit"
		elif(cnt == 1):
			st = "bye"
		elif(cnt >= 2):
			st = "where would you like me to go next"
		publisher(st)
		time.sleep(1)
if __name__ == '__main__':
	for i in xrange(5):
		print i
	main()