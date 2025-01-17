#!/usr/bin/env python2
from pocketsphinx import *
from sphinxbase import *
import os, sys
import pyaudio
import rospy
from std_msgs.msg import String

def main(): # Method for recognnizing the name Nora 
        config = Decoder.default_config() #Create decoder
        config.set_string('-hmm','/home/emotion/Software/pocketsphinx2/model/hmm/en_US/hub4wsj_sc_8k')# set hidden markov model
        #config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP') # set language model not needed for Keyword detection
        config.set_string('-dict', '/home/emotion/Software/pocketsphinx2/model/lm/en_US/cmu07a.dic') # dictionary
        config.set_string('-logfn','\dev\\null') #Deletes log from terminal...cleaner
        config.set_string('-keyphrase', 'nora') # keyphrase set to "Nora"
        config.set_float('-kws_threshold', 1e+10) # keyword search threshold
        done=True 
        while done == True:
            p = pyaudio.PyAudio()
            stream = p.open(format=pyaudio.paInt16, channels=1, rate=16000,input=True, frames_per_buffer=1024)
            stream.start_stream() # create audio stream above and start thae stream
            decoder = Decoder(config) # configurate the decoder
            decoder.start_utt() # stare the decoder utterance
            deco=True
            while deco==True:
                buf = stream.read(1024) # create audio buffer
                if buf:
                    decoder.process_raw(buf, False, False) # process the audio
                else:
                    break
                if decoder.hyp() != None and decoder.hyp().hypstr=='nora': #check if it equals to Nora
                    decoder.end_utt() # end utterance
                    hypothesis = decoder.hyp()
                    hyp1 = hypothesis.hypstr # find hypothesis which is the string result
                    print hypothesis.hypstr 
                    stream.close(); # close stream
                    deco=False # close loop
                    done=False # close loop
                    return "Nora" # return true for object if statement
def publisher():
    pub = rospy.Publisher('Ready',String,queue_size=10)
    rate = rospy.Rate(10)
    while pub.get_num_connections() == 0:
        rate.sleep()
    msg=String()
    msg.data=str("go")
    pub.publish(msg) 

if __name__ == '__main__':
    count=0
    rospy.init_node('RecNora', anonymous=True)
    while True:
        x=main()
        publisher()


		# if count==0:
		# 	publisher()
		# 	publisher()
		# 	count=count+1
		# else:
		# 	publisher()	
