#!/usr/bin/env python2
from pocketsphinx import *
from sphinxbase import *
import os, sys
import pyaudio
import rospy
from std_msgs.msg import String
import time
import rospy
from std_msgs.msg import String
import sys,os
import speech_recognition as sr
from ros_rp2w.srv._festTTS import *

def main(): # Method for recognnizing the name Nora 
        config = Decoder.default_config() #Create decoder
        config.set_string('-hmm','/home/emotion/Software/pocketsphinx2/model/hmm/en_US/hub4wsj_sc_8k')# set hidden markov model
        #config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP') # set language model not needed for Keyword detection
        config.set_string('-dict', '/home/emotion/Software/pocketsphinx2/model/lm/en_US/cmu07a.dic') # dictionary
        config.set_string('-logfn','\dev\\null') #Deletes log from terminal...cleaner
        #config.set_string('-keyphrase', 'go there') # keyphrase set to "Nora"
        config.set_float('-kws_threshold', 1e+4) # keyword search threshold
        config.set_float('-vad_threshold', 3)
        done=True 
        while done == True:
            ready=False
            filename = os.path.abspath(os.path.dirname(__file__))
            p = pyaudio.PyAudio()
            stream = p.open(format=pyaudio.paInt16, channels=1, rate=16000,input=True, frames_per_buffer=1024)
            stream.start_stream() # create audio stream above and start thae stream
            decoder = Decoder(config) # configurate the decoder
            decoder.set_kws('keyphrase_search',filename+'/keyphrase.list')
            decoder.set_search('keyphrase_search')
            decoder.get_config()
            decoder.start_utt() # stare the decoder utterance
            deco=True
            while deco==True:
                buf = stream.read(1024) # create audio buffer
                if buf:
                    decoder.process_raw(buf, False, False) # process the audio
                else:
                    break
                try:
                        if  decoder.hyp().hypstr=='yes' or decoder.hyp().hypstr=='no' or decoder.hyp().hypstr=='move there' or decoder.hyp().hypstr=='control' or decoder.hyp().hypstr=='bye nora' or decoder.hyp().hypstr=='follow' or decoder.hyp().hypstr=='find me' or decoder.hyp().hypstr=='turn' or decoder.hyp().hypstr=='music':
                            ready=True
                        if  ready and decoder.hyp()!='':    
                            decoder.end_utt() # end utterance
                            hypothesis = decoder.hyp()
                            hyp1 = hypothesis.hypstr # find hypothesis which is the string result
                            print hypothesis.hypstr 
                            stream.close(); # close stream
                            deco=False # close loop
                            done=False # close loop
                            return hyp1 # return true for object if statement
                except Exception, e:
                        pass 
# def main():
#     exit=False      
#     while exit==False:
#         r = sr.Recognizer()
#         #with sr.Microphone() as source: r.adjust_for_ambient_noise(source)
#         r.energy_threshold= 10000
#         hyper=None
#         with sr.Microphone() as source:                # use the default microphone as the audio source
#             audio = r.listen(source)                   # listen for the first phrase and extract it into audio data
#         # recognize speech using AT&T Speech to Text
#         ATT_APP_KEY = "v72tyvstxthasy1az2t40rzhrre1ufzc" # AT&T Speech to Text app keys are 32-character lowercase alphanumeric strings
#         ATT_APP_SECRET = "uloea3capyxwub1hbwg2vswhqxbynpr4" # AT&T Speech to Text app secrets are 32-character lowercase alphanumeric strings
#         try:
#             #hyper=r.recognize_att(audio, app_key=ATT_APP_KEY, app_secret=ATT_APP_SECRET)
#             hyper=r.recognize_sphinx(audio)           
#         except sr.UnknownValueError:
#             print("Speech to Text could not understand audio")
#             #exit=False
#             #Asr.tts_client(self,"I could not quite hear that!")
#         except sr.RequestError:
#             print("Could not request results from Speech to Text service")
#             #exit=False
#         if hyper != None:
#             return hyper
#             #exit=True
#    # print hyper



def publisher(data):
    pub = rospy.Publisher('Ready',String,queue_size=10)
    rate = rospy.Rate(10)
    while pub.get_num_connections() == 0:
        rate.sleep()
    msg=String()
    msg.data=str(data)
    pub.publish(msg) 

if __name__ == '__main__':
    count=0
    rospy.init_node('recmove', anonymous=True)
    now=time.time()
    while not rospy.is_shutdown():
        if time.time()-now<60:
            x=main()
            publisher(x)
        else:
            rospy.signal_shutdown("computer terminated for respawn")

