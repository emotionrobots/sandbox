#!/usr/bin/env python2
import clips

def load_class():
    """ CAUSAL observation class """
    clips.Build("""
      (defclass CAUSAL (is-a USER)
         (slot cause (create-accessor read-write))
         (slot effect (create-accessor read-write))
         (slot context (create-accessor read-write))
         (slot prob (create-accessor read-write))
      )""")


def load_rules():
    """ Rules implementing forward and backward chaining """
    clips.RegisterPythonFunction(query_likely_cause)
    clips.SetExternalTraceback(True)
    clips.BuildRule('forward-chain-starter',
         '?f <- (cause ?c)',
         '(assert (forward-chain ?c 1))',
         'Forward Chaining Rule')

    clips.BuildRule('backward-chain-starter',
         '?f <- (effect ?e)',
         '(assert (backward-chain ?e 1))',
         'Backward Chaining Starter')

    clips.BuildRule('forward-chaining',
         '?f <- (forward-chain $?s ?c ?p1) \
          (python-call query_likely_cause "e") \
          ?obj <- (object (is-a CAUSAL) (cause ?c) (prob ?p2))',
         '(bind ?e (send ?obj get-effect)) \
          (assert (forward-chain $?s ?c ?e (* ?p1 ?p2)))',
         'Forward Chaining Rule')

    clips.BuildRule('backward-chaining',
         '?f <- (backward-chain $?s ?e ?p1) \
          ?obj <- (object (is-a CAUSAL) (effect ?e) (prob ?p2))',
         '(bind ?c (send ?obj get-cause)) \
          (assert (backward-chain $?s ?e ?c (* ?p1 ?p2)))',
         'Backward Chaining Rule')


def add_causal(name, cause, effect, context, prob):
    """ Add a new causal observation """
    i = clips.BuildInstance(name, clips.FindClass("CAUSAL"))
    i.Slots['cause'] = cause
    i.Slots['effect'] = effect
    i.Slots['context'] = context
    i.Slots['prob'] = prob   


def query_likely_cause(effect):
    print effect



    #li=clips.FactList()
    #for x in li:
    	#if (x['prob']==0)x['prob']==0):
    		#print "hello"



def main():
    """ test driver """
    load_class()
    load_rules()
    

    add_causal('a', "A", "B", "none", 0.5)
    add_causal('b', "B", "C", "none", 0.1)
    add_causal('c', "C", "D", "none", 0.2)
    add_causal('d', "D", "E", "none", 0.8)
    add_causal('e', "C", "E", "none", 0.3)

    clips.Assert('(cause "A")')
    clips.Assert('(effect "E")')
    clips.Run() 
    clips.PrintFacts()
    #query_likely_cause("E") 


if __name__=="__main__":
    main()