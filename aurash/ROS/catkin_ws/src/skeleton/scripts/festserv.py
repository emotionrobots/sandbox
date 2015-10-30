#!/usr/bin/env python
from skeleton.srv._festTTS import *
import rospy


def handle_utterance(req):
    import subprocess
    text = str(req)
    text=text[4:]
    print text
    subprocess.call("echo "+text+"|festival --tts", shell=True)
    return festTTSResponse("The deed has been done")

def tts_server():
    rospy.init_node('tts_server')
    s = rospy.Service('tts', festTTS, handle_utterance)
    print "Ready to talk!"
    rospy.spin()

if __name__ == "__main__":
    tts_server()