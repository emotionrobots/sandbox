#!/usr/bin/env python
from ros_rp2w.srv._festTTS import *
import rospy


def handle_utterance(req):
    import subprocess
    text = str(req)
    text=text[5:]
    #count=0
    #for x in text:
     #   if x=="'":
      #      text=text[0:count]+"\'"+text[count+1:(len(text))]
       #     count=count+1
    print text        
    subprocess.call("""echo {}|festival --tts""".format(text.replace("'", '')), shell=True)
    return festTTSResponse("The deed has been done")

def tts_server():
    rospy.init_node('tts_server')
    s = rospy.Service('tts', festTTS, handle_utterance)
    print "Ready to talk!"
    rospy.spin()

if __name__ == "__main__":
    tts_server()
