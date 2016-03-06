#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import sys,os
import speech_recognition as sr
from ros_rp2w.srv._festTTS import *
#import urllib2

class Asr(object):
    def RepMe(self):
        exit=False      
        while exit==False:
            r = sr.Recognizer()
            #with sr.Microphone() as source: r.adjust_for_ambient_noise(source)
            r.energy_threshold= 10000
            hyper=None
            with sr.Microphone() as source:                # use the default microphone as the audio source
                audio = r.listen(source)                   # listen for the first phrase and extract it into audio data
            # recognize speech using AT&T Speech to Text
            ATT_APP_KEY = "v72tyvstxthasy1az2t40rzhrre1ufzc" # AT&T Speech to Text app keys are 32-character lowercase alphanumeric strings
            ATT_APP_SECRET = "uloea3capyxwub1hbwg2vswhqxbynpr4" # AT&T Speech to Text app secrets are 32-character lowercase alphanumeric strings
            try:
                hyper=r.recognize_att(audio, app_key=ATT_APP_KEY, app_secret=ATT_APP_SECRET)
                #hyper=r.recognize_sphinx(audio)           
            except sr.UnknownValueError:
                print("Speech to Text could not understand audio")
                #exit=False
                #Asr.tts_client(self,"I could not quite hear that!")
            except sr.RequestError:
                print("Could not request results from Speech to Text service")
                #exit=False
            if hyper != None:
                Asr.publisher(self,hyper)
                Asr.tts_client(self,hyper)
                #exit=True
       # print hyper

    def publisher(self,stri):
        
        pub = rospy.Publisher('chatter', String,queue_size=10)
        msg=String()
        msg.data=str(stri.lower())
        pub.publish(msg) 



    def tts_client(self,string2):
        rospy.wait_for_service('tts')
        try:
            tts = rospy.ServiceProxy('tts', festTTS)
            req = festTTSRequest(string2)
            resp = tts(string2)
            print str(resp)[4:]
        except rospy.ServiceException, e:
            print "Service call failed: %s"%e     


if __name__=='__main__':        
    rospy.init_node('sttpub', anonymous=True)
    while True:
        test=Asr()
        test.RepMe()
