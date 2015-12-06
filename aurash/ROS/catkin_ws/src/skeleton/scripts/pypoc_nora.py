#!/usr/bin/env python
from pocketsphinx import *
from sphinxbase import *
import os, sys
import pyaudio
import time
import cv2
import sys
import nltk
from nltk import pos_tag, word_tokenize
import rospy
import aiml
from std_msgs.msg import String
from std_msgs.msg import Int8
from nltk.tokenize import WhitespaceTokenizer

# Create a decoder with certain model
class Nora(object): #Main class for Speech Reccognition
    hypo=None
   
    def RecNora(self): # Method for recognnizing the name Nora 
        config = Decoder.default_config() #Create decoder
        config.set_string('-hmm','/usr/share/pocketsphinx/model/hmm/en_US/hub4wsj_sc_8k')# set hidden markov model
        #config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP') # set language model not needed for Keyword detection
        config.set_string('-dict', '/usr/share/pocketsphinx/model/lm/en_US/cmu07a.dic') # dictionary
        config.set_string('-logfn','\dev\\null') #Deletes log from terminal...cleaner
        config.set_string('-keyphrase', 'nora') # keyphrase set to "Nora"
        config.set_float('-kws_threshold', 1e+10) # keyword search threshold
        done=True 
        Nora.tts(self, "Hi! I'm Nora!")
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
                    Nora.tts(self,"yes?") # shell script for 
                    stream.close(); # close stream
                    deco=False # close loop
                    done=False # close loop
                    return True # return true for object if statement  
                    
    def RepMe(self):
        config = Decoder.default_config()
        config.set_string('-hmm','/usr/share/pocketsphinx/model/hmm/en_US/hub4wsj_sc_8k')
        config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP')
        config.set_string('-dict', '/usr/share/pocketsphinx/model/lm/en_US/cmu07a.dic')
        config.set_float('-vad_threshold', 3)
        #config.set_float('-vad_postspeech', 500)
        #config.set_float('-vad_prespeech', 20)
        #config.set_float('-vad_startspeech', 20)
        config.set_string('-logfn','\dev\\null')
        #config.set_string('-remove_noise','yes')
        #config.set_string('-remove_silence','yes')
        decoder = Decoder(config)

        p1 = pyaudio.PyAudio()
        stream = p1.open(format=pyaudio.paInt16, channels=1, rate=16000, input=True, frames_per_buffer=1024)
        stream.start_stream()
        in_speech_bf = True
        decoder.start_utt()
        deco=True
        parres=''
        while deco:
            buf = stream.read(1024)
            if buf:
                decoder.process_raw(buf, False, False)
                try:
                    if  decoder.hyp().hypstr != '':
                        parres=decoder.hyp().hypstr+' '
                        print('Partial decoding result:', decoder.hyp().hypstr)
                except AttributeError:
                    pass
                if decoder.get_in_speech():
                    sys.stdout.write('.')
                    sys.stdout.flush()
                if decoder.get_in_speech() != in_speech_bf:
                    in_speech_bf = decoder.get_in_speech()
                    if not in_speech_bf:
                        decoder.end_utt()
                        try:
                            if  decoder.hyp().hypstr != '':
                                hypothesis = decoder.hyp()
                                hyp1 = hypothesis.hypstr
                                print ('Best hypothesis: ', decoder.hyp().hypstr, " model score: ", hypothesis.best_score, " confidence: ", hypothesis.prob)
                                print ([(seg.word, seg.prob, seg.start_frame, seg.end_frame) for seg in decoder.seg()])
                                Nora.tts(self, "Did you mean to say")
                                Nora.tts(self, hyp1)
                                stream.close(); # close stream
                                deco=False
                                global hypo
                                hypo=hyp1
                                return True
                        except AttributeError:
                            pass
                        decoder.start_utt()
        decoder.end_utt()


   
    def yon(self): 

        config = Decoder.default_config() #Create decoder
        config.set_string('-hmm','/usr/share/pocketsphinx/model/hmm/en_US/hub4wsj_sc_8k')# set hidden markov model
        #config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP') # set language model not needed for Keyword detection
        config.set_string('-dict', '/usr/share/pocketsphinx/model/lm/en_US/cmu07a.dic') # dictionary
        #config.set_string('-logfn','\dev\\null') #Deletes log from terminal...cleaner
        #config.set_string('-kws', '/home/aurash/keyphrase.list') # keyphrase set to "Nora"
        config.set_float('-kws_threshold', 1e+10) # keyword search threshold
        config.set_float('-vad_threshold', 4)
        

        p = pyaudio.PyAudio()
        stream = p.open(format=pyaudio.paInt16, channels=1, rate=16000,input=True, frames_per_buffer=1024)
        stream.start_stream() 
        decoder = Decoder(config) 
        decoder.set_kws('keyphrase_search',"/home/aurash/keyphrase.list");
        decoder.set_search('keyphrase_search'); 
        decoder.get_config()
        decoder.start_utt() 
        in_speech_bf = True
        deco=True
        while deco:
            buf = stream.read(1024)
            if buf:
                decoder.process_raw(buf, False, False)
                try:
                    if decoder.hyp() != '' and decoder.hyp().hypstr=='no' and deco==True:
                        stream.close(); # close stream
                        return False
                except AttributeError:
                    pass   
                try:
                    if decoder.hyp() != '' and decoder.hyp().hypstr=='yes' and deco==True:
                        stream.close(); # close stream
                        return True
                except AttributeError:
                    pass          
                if decoder.get_in_speech():
                    sys.stdout.write('.')
                    sys.stdout.flush()
                if decoder.get_in_speech() != in_speech_bf:
                    in_speech_bf = decoder.get_in_speech()
                    if not in_speech_bf:
                        decoder.end_utt()
                        decoder.start_utt()
        decoder.end_utt()
                    

    
    def tts(self,str):
        text=str
        cm ='/home/aurash/simple-google-tts/./simple_google_tts -p en "'+ text + '"'
        os.system(cm)
    
    def talker(self,stri):
        pub = rospy.Publisher('chatter', String,queue_size=10)
        rospy.init_node('ASR', anonymous=True)
        msg=String()
        msg.data=str(hypo.lower())
        pub.publish(msg)   

    def ttsalker(self,strin):
        text = word_tokenize(strin)
        wa=len(text)*.05+len(text)/2.5
        pub = rospy.Publisher('speak', String, queue_size=10)
        rospy.init_node('ASR', anonymous=True)
        msg=String()
        msg=str(strin.lower())
        pub.publish(msg)    
        time.sleep(wa) 
         
    


    def RecTest(self):
        if Nora.RecNora(self)==True:
            recog=False
            while not recog:
                a=Nora.RepMe(self) #gstt or RepMe
                if a:
                    b=Nora.yon(self)
                    if b:
                        Nora.pyaiml(self,hypo)
                        recog=True
                        #Nora.tts(self, "Awesome!")
                        #Nora.talker(self,hypo)
                        #Nora.ttsalker(self,hypo)
                        #Nora.nlp(self)
                    else:
                        Nora.tts( self,"Please repeat your statement")
                        recog=False


    def RepTest(self):
            recog=False
            while not recog:
                a=Nora.RepMe(self) #gstt or RepMe
                if a:
                    b=Nora.yon(self)
                    if b:
                        Nora.pyaiml(self,hypo)
                        recog=True
                        #Nora.tts(self, "Awesome!")
                        #Nora.talker(self,hypo)
                        #Nora.ttsalker(self,hypo)
                        #Nora.nlp(self)
                    else:
                        Nora.tts( self,"Please repeat your statement")
                        recog=False                    

    def nlp(self):
        text = word_tokenize(hypo)
        temp=text[0]
        text[0]=text[1] 
        text[1]=temp
        text.append('?')
        Nora.tts(self,' '.join(text))
        config = Decoder.default_config() #Create decoder
        config.set_string('-hmm','/usr/share/pocketsphinx/model/hmm/en_US/hub4wsj_sc_8k')# set hidden markov model
        #config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP') # set language model not needed for Keyword detection
        config.set_string('-dict', '/usr/share/pocketsphinx/model/lm/en_US/cmu07a.dic') # dictionary
        #config.set_string('-logfn','\dev\\null') #Deletes log from terminal...cleaner
        #config.set_string('-kws', '/home/aurash/keyphrase.list') # keyphrase set to "Nora"
        config.set_float('-kws_threshold', 1e+10) # keyword search threshold
        

        p = pyaudio.PyAudio()
        stream = p.open(format=pyaudio.paInt16, channels=1, rate=16000,input=True, frames_per_buffer=1024)
        stream.start_stream() 
        decoder = Decoder(config) 
        decoder.set_kws('keyphrase_search',"/home/aurash/keyphrase.list");
        decoder.set_search('keyphrase_search'); 
        decoder.get_config()
        decoder.start_utt() 
        in_speech_bf = True
        deco=True
        while deco:
            buf = stream.read(1024)
            if buf:
                decoder.process_raw(buf, False, False)
                try:
                    if decoder.hyp() != '' and decoder.hyp().hypstr=='no' and deco==True:
                        Nora.tts(self,"well")
                        stream.close(); # close stream
                        deco=False
                except AttributeError:
                    pass   
                try:
                    if decoder.hyp() != '' and decoder.hyp().hypstr=='yes' and deco==True:
                        Nora.tts(self,"great!")
                        stream.close(); # close stream
                        deco=False
                except AttributeError:
                    pass          
                if decoder.get_in_speech():
                    sys.stdout.write('.')
                    sys.stdout.flush()
                if decoder.get_in_speech() != in_speech_bf:
                    in_speech_bf = decoder.get_in_speech()
                    if not in_speech_bf:
                        decoder.end_utt()
                        decoder.start_utt()
        decoder.end_utt() 
    
    def pyaiml(self, strig):
        os.chdir('/home/aurash/sets')
        mybot=aiml.Kernel()
        mybot.learn('std-startup.xml')
        mybot.respond('load aiml b')
        try:
            response=mybot.respond(strig)
            Nora.tts(self,response)
        except TypeError:
                    pass

    

def callback(data):
    text=str(data)
    text2 = WhitespaceTokenizer().tokenize(text)
    text2.remove("data:")
    print(text2)
    text2=' '.join(text2)
    if text2=="start1":
        test=Nora()
        try:
            test.RecTest()
        except rospy.ROSInterruptException:
            pass
    if text2=="start2":
        test=Nora()
        try:
            test.RepTest()
        except rospy.ROSInterruptException:
            pass
            
            

def listener():
    rospy.init_node('Speech') 
    rospy.Subscriber("strtspch",String,callback)
    rospy.spin()

if __name__=='__main__':
    listener();  



