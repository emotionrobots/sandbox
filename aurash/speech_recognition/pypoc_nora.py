from pocketsphinx import *
from sphinxbase import *
import os, sys
import pyaudio
import time
import cv2
import sys
import nltk
import aiml
from nltk import pos_tag, word_tokenize
import speech_recognition as sr
# Create a decoder with certain model
class Nora(object): #Main class for Speech Reccognition
    hypo=None
    filename=None
    def RecNora(self): # Method for recognnizing the name Nora 
        config = Decoder.default_config() #Create decoder
        config.set_string('-hmm','/usr/share/pocketsphinx/model/hmm/en_US/hub4wsj_sc_8k')# set hidden markov model
        #config.set_string('-lm', '/usr/share/pocketsphinx/model/lm/en_US/hub4.5000.DMP') # set language model not needed for Keyword detection
        config.set_string('-dict', '/usr/share/pocketsphinx/model/lm/en_US/cmu07a.dic') # dictionary
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
                    Nora.tts(self, "yes?") # shell script for 
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
        decoder.set_kws('keyphrase_search',"filename/keyphrase.list");
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
    
   
    

    def main(self,ints):
        print ints
        if ints=="cmu":
            count=0
            while True:
                print "hello"
                if count==0:
                    global r
                    r=None
                    a=Nora.face_detect(self)
                    Nora.tts(self,"Hi! I'm Nora")
                    if a:
                        r=Nora.RecNora(self)==True
                if count>0 or r:
                        recog=False
                        print "hello"
                        while not recog:
                            a=Nora.RepMe(self) #gstt or RepMe
                            if a:
                                b=Nora.yon(self)
                                if b:
                                    print('NLTK') # Implement NLTK method
                                    recog=True
                                    #Nora.tts(self, "Awesome!")
                                    Nora.pyaiml(self,hypo)
                                    count=count+1
                                    print "hello"
                                else:
                                    Nora.tts( self,"Please repeat your statement")
                                    recog=False
                                    count=count+1
        else:
            a=Nora.face_detect(self)
            Nora.tts(self,"Hi! I'm Nora")
            if a:
                if Nora.RecNora(self)==True:
                    while True:
                        a=Nora.gstt(self)
                        Nora.pyaiml(self,a)                        

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
        decoder.set_kws('keyphrase_search',"filename/keyphrase.list");
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
    	os.chdir('/filename/sets')
        mybot=aiml.Kernel()
        mybot.learn('std-startup.xml')
        mybot.respond('load aiml b')
        try:
            response=mybot.respond(strig)
            Nora.tts(self,response)
        except TypeError:
                    pass
            


    def gstt(self):
    	r = sr.Recognizer()
        r.energy_threshold= 30000
        hyper=None
        with sr.Microphone() as source:                # use the default microphone as the audio source
            audio = r.listen(source)                   # listen for the first phrase and extract it into audio data
        try:
        	hyper=r.recognize(audio)
        except LookupError:                            # speech is unintelligible
            print("Could not understand audio")
        if(hyper != None):
            print(hyper)
            return hyper   


    def face_detect(self):
        global filename
        filename = os.getcwd()      
        print filename
        cascPath = filename+"/cascades/haarcascade_frontalface_default.xml"
        eye_cascade = cv2.CascadeClassifier(filename+"/cascades/haarcascade_eye.xml")
        faceCascade = cv2.CascadeClassifier(cascPath)
        smile_cascade=cv2.CascadeClassifier(filename+"/cascades/haarcascade_smile.xml")
        nose_cascade=cv2.CascadeClassifier(filename+"/cascades/haarcascade_mcs_nose.xml")

        video_capture = cv2.VideoCapture(0)
        found=False
        while found==False:
            # Capture frame-by-frame
            ret, frame = video_capture.read()

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            faces = faceCascade.detectMultiScale(
                gray,
                scaleFactor=1.2,
                minNeighbors=5,
                minSize=(30, 30),
                flags=cv2.cv.CV_HAAR_SCALE_IMAGE
            )

    

            # Draw a rectangle around the faces
            for (x, y, w, h) in faces:
                cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
                roi_gray = gray[y:y+h, x:x+w]
                roi_color = frame[y:y+h, x:x+w]
                eyes = eye_cascade.detectMultiScale(roi_gray,1.1,50,minSize=(20, 20),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
                for (ex,ey,ew,eh) in eyes:
                    cv2.rectangle(roi_color,(ex,ey),(ex+ew,ey+eh),(0,255,0),2)
                smile = smile_cascade.detectMultiScale(roi_gray, scaleFactor=1.3, minNeighbors=500, minSize=(20, 20), flags=cv2.cv.CV_HAAR_SCALE_IMAGE)    
                smile = smile_cascade.detectMultiScale(roi_gray,1.3,20)
                for (sx,sy,sw,sh) in smile:
                    cv2.rectangle(roi_color,(sx,sy),(sx+sw,sy+sh),(255,255,0),2)
                nose = nose_cascade.detectMultiScale(roi_gray,1.1,5,minSize=(20, 20),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
                nose = nose_cascade.detectMultiScale(roi_gray,1.3,20)
                for (nx,ny,nw,nh) in nose:
                    cv2.rectangle(roi_color,(nx,ny),(nx+nw,ny+nh),(0,255,255),2)



            # Display the resulting frame
            cv2.imshow('Video', frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

            if len(faces)>0 and len(eyes)>1 and len(smile)>0 and len(nose)>0:
                found=True
                #video_capture.release()
                #cv2.destroyAllWindows()
                return True    

if __name__ == '__main__':
    test=Nora()
    while True:
        test.main(sys.argv[1])