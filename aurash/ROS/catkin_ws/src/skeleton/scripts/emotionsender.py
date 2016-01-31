#!/usr/bin/env python2
import rospy
from std_msgs.msg import String



def main():
	    pub = rospy.Publisher('emotiondisplay2', String,queue_size=1)
	    rospy.set_param('emotion', 'neutral .9996')
	    rate = rospy.Rate(1)
	    while not rospy.is_shutdown():
	    	emotion=rospy.get_param('emotion')
	    	print emotion
	    	pub.publish(emotion)
            rate.sleep()





if __name__ == '__main__':
	rospy.init_node("emotion_creator")
	main()