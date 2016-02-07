#!/usr/bin/env python2
import rospy
from std_msgs.msg import String
import sys,os
import speech_recognition as sr
import time
from skeleton.srv._festTTS import *
#import urllib2

class Asr(object):
    def RepMe(self):
        exit=False      
        r = sr.Recognizer()
        m = sr.Microphone()
        #while exit==False:
        print("A moment of silence, please...")
        with m as source: r.adjust_for_ambient_noise(source)
        print("Set minimum energy threshold to {}".format(r.energy_threshold))
        while True:
            hyper=None
            print("Say something!")
            with m as source: audio = r.listen(source)
            print("Got it! Now to recognize it...")
            #try:               # listen for the first phrase and extract it into audio data
            # recognize speech using AT&T Speech to Text
                #ATT_APP_KEY = "v72tyvstxthasy1az2t40rzhrre1ufzc" # AT&T Speech to Text app keys are 32-character lowercase alphanumeric strings
                #ATT_APP_SECRET = "uloea3capyxwub1hbwg2vswhqxbynpr4" # AT&T Speech to Text app secrets are 32-character lowercase alphanumeric strings
            try:
                hyper=r.recognize_sphinx(audio)
                if hyper != None:
                    Asr.publisher(self,hyper)
                    Asr.tts_client(self,hyper)
            except sr.UnknownValueError:
                print("Sphinx could not understand audio")
            except sr.RequestError as e:
                print("Sphinx error; {0}".format(e))
            time.sleep(0.5)    
                #hyper=r.recognize_att(audio, app_key=ATT_APP_KEY, app_secret=ATT_APP_SECRET)
            #except sr.UnknownValueError:
              #  print("Speech to Text could not understand audio")
                #exit=False
                #Asr.tts_client(self,"I could not quite hear that!")
            #except sr.RequestError:
             #   print("Could not request results from Speech to Text service")
                #exit=False
           # if hyper != None:
           #     Asr.publisher(self,hyper)
           #     Asr.tts_client(self,hyper)
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