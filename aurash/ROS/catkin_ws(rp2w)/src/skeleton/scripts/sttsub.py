#!/usr/bin/env python
import rospy
from std_msgs.msg import String

def callback(data):
    text=str(data)
    text=text[5:]
    print text
            

def listener():
    rospy.init_node('Speech',anonymous=True) 
    rospy.Subscriber("chatter",String,callback)
    rospy.spin()


if __name__ == '__main__':
	listener()