#!/usr/bin/env python
import rospy
from skeleton.msg import Face

def callback(data):
	print data.name
	print data.llx
	print data.lly
	print data.urx
	print data.ury





def main():
    rospy.init_node('What_Face',anonymous=True) 
    rospy.Subscriber("known_faces",Face,callback)
    rospy.spin() 





if __name__ == '__main__':
	main()