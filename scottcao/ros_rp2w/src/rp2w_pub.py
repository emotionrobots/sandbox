#!/usr/bin/env python
# license removed for brevity
import sys
import rospy
from ros_rp2w.msg import AdvancedCommand

if __name__ == '__main__':
    if len(sys.argv) == 3:
        theta = float(sys.argv[1])
        distance = float(sys.argv[2])
    pub = rospy.Publisher("rp2w/advanced_command", AdvancedCommand, queue_size=1)
    rospy.init_node('rp2w_publisher', anonymous=True)
    rate = rospy.Rate(10) # 10hz
    while pub.get_num_connections() == 0:
        rate.sleep()
    msg = AdvancedCommand()
    msg.theta = theta
    msg.distance = distance
    pub.publish(msg)
    print msg
