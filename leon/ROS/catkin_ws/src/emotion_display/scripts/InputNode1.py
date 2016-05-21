import rospy
import std_msgs.msg
from vfacerec.msg import Face
from vfacerec.msg import UnknownFace
from vfacerec.srv import SetRate


def publisher(done):
	pub = rospy.Publisher('known_faces', Face, queue_size = 1)
	rospy.initNode('dummynode', anonymous = True)
	msg = Face();
	# msg.data = 
	r = rospy.Rate(1);
	if not rospy.is_shutdown():
		print("Publish Face Messages Sent")
		pub.publish(msg)
def main():
	while not rospy.is_shutdown():
		# res = 
		publisher(res)
if __name__ == '__main__':
	main()