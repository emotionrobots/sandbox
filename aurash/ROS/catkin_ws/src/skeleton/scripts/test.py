#!/usr/bin/env python
import os,sys
import rospy
from std_msgs.msg import String
import nltk
from nltk.tokenize import WhitespaceTokenizer

def callback(data):
    text=str(data)
    text2 = WhitespaceTokenizer().tokenize(text)
    text2.remove("data:")
    print(text2)
    text2=' '.join(text2)
    print(text2)
    cm ='/home/aurash/simple-google-tts/./simple_google_tts -p en "'+ text2 + '"'
    os.system(cm)
    



def listener():
    rospy.init_node('Speech') 
    a=rospy.Subscriber("strtspch",String,callback)
    print(a)
    rospy.spin()

if __name__=='__main__':
    listener();