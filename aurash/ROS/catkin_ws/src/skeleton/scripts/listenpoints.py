
#!/usr/bin/env python

import rospy










def landmark(data):
	print data






def listener():
    rospy.Subscriber('face_points', list, landmark)
    rospy.spin()





if __name__ == '__main__':
	rospy.init_node('listenerp', anonymous=True)
    listener()