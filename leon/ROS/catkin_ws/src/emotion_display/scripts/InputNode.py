import rospy
from std_msgs.msg import String

def publisher(done):
	pub = rospy.Publisher("rp2w_packet", Packet, queue_size=10)
	rospy.init_node("dummynode", anonymous = True)
	msg = Packet()
	# msg.data = 
	r = rospy.Rate(1)
	if not rospy.is_shutdown():
		print("RP2W Packet Published")
		pub.publish(msg)

def main():
	while not rospy.is_shutdown():
		# res
		publisher(res)
if __name__ == '__main__':
	main()