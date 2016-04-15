#!/usr/bin/env python
# license removed for brevity
import rospy
from ros_rp2w.msg import AdvancedCommand

def talker():
    pub = rospy.Publisher('rp2w/advanced_command', AdvancedCommand, queue_size=10)
    rospy.init_node('rp2w_publisher', anonymous=True)
    rate = rospy.Rate(10) # 10hz
    msg = AdvancedCommand()
    msg.distanceCommand = True;
    msg.distance = 100;
    pub.publish(msg)

if __name__ == '__main__':
    try:
        talker()
    except rospy.ROSInterruptException:
        pass