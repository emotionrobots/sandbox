#!/usr/bin/env python
# license removed for brevity
import rospy
from ros_rp2w.msg import AdvancedCommand

if __name__ == '__main__':
    pub = rospy.Publisher("rp2w/advanced_command", AdvancedCommand, queue_size=1)
    rospy.init_node('rp2w_publisher', anonymous=True)
    rate = rospy.Rate(10) # 10hz
    # while not rospy.is_shutdown():
    while pub.get_num_connections() == 0:
        rate.sleep()
    msg = AdvancedCommand()
    msg.distanceCommand = True
    msg.distance = 1
    msg.thetaCommand = False
    msg.theta = 0
    pub.publish(msg)
    print msg
