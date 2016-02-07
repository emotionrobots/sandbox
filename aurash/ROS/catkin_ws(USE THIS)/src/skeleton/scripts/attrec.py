#!/usr/bin/env python
import rospy
import pyaudio
import speech_recognition as sr
import urllib2
from std_msgs.msg import String



def gstt():
    exit=False      
    while exit==False:
        r = sr.Recognizer()
        r.energy_threshold= 10000
        hyper=None
        with sr.Microphone() as source:                # use the default microphone as the audio source
            audio = r.listen(source)                   # listen for the first phrase and extract it into audio data
        # recognize speech using AT&T Speech to Text
        ATT_APP_KEY = "v72tyvstxthasy1az2t40rzhrre1ufzc" # AT&T Speech to Text app keys are 32-character lowercase alphanumeric strings
        ATT_APP_SECRET = "uloea3capyxwub1hbwg2vswhqxbynpr4" # AT&T Speech to Text app secrets are 32-character lowercase alphanumeric strings
        try:
            hyper=r.recognize_ATT(audio, app_key=ATT_APP_KEY, app_secret=ATT_APP_SECRET)
        except sr.UnknownValueError:
            print("Speech to Text could not understand audio")
            #Nora.tts(self,"I didn't quite get that!")
            #exit=False
        except sr.RequestError:
            print("Could not request results from Speech to Text service")
            #exit=False
        if hyper != None:
            publisher(hyper)
    


def publisher(stri):
    msg=String()
    msg.data=str(stri.lower())
    pub.publish(msg) 

if __name__ == '__main__':
    pub = rospy.Publisher('chatter', String,queue_size=10)
    rospy.init_node('sttpub', anonymous=True)
    gstt()

    