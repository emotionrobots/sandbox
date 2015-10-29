from pocketsphinx import *
from sphinxbase import *
import rospy
from std_msgs.msg import String

class Asr(object):
    def RepMe(self):
        config = Decoder.default_config()
        config.set_string('-hmm','/home/aurash/cmusphinx-en-us-ptm-5.2')
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
        while True:
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
                                try:
                                	Publisher(stri)
                                except rospy.ROSInterruptException:
                                	pass
                                print ('Best hypothesis: ', decoder.hyp().hypstr, " model score: ", hypothesis.best_score, " confidence: ", hypothesis.prob)
                                print ([(seg.word, seg.prob, seg.start_frame, seg.end_frame) for seg in decoder.seg()])
                                stream.close(); # close stream
                                deco=False
                                return True
                        except AttributeError:
                            pass
                        decoder.start_utt()
        decoder.end_utt()
    

	def Publisher(self,stri):
	    pub = rospy.Publisher('chatter', String,queue_size=10)
	    rospy.init_node('sttpub', anonymous=True)
	    msg=String()
	    msg.data=str(stri.lower())
	    pub.publish(msg) 


if __name__=='__main__':        
    test=Asr()
    test.RepMe()
