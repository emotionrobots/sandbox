#!/usr/bin/env python2
import clips
import os
import retest

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


def query_likely_cause(effect,files):
    
    f = open(files, "r")
    lines=f.readlines()
    f.close
    big=0.0
    count=0
    index=0
    list=[]
    for x in lines:
        try:
            forb, inital, prob= retest.info(x)
            if forb=='backward':
                ind=x.index(prob)
                cause=x[ind-3]
                list.append((forb, inital, cause, float(prob)))
                if big<prob:
                    big=prob
                    index=count
                count=count+1    
        except:
            pass  
    if big !=0: 
        return list[index][2]   




def query_unlikely_cause(effect,files):
    
    f = open(files, "r")
    lines=f.readlines()
    f.close
    small=50.0
    count=0
    index=0
    list=[]
    for x in lines:
        try:
            forb, inital, prob= retest.info(x)
            if forb=='backward':
                ind=x.index(prob)
                cause=x[ind-3]
                list.append((forb, inital, cause, float(prob)))
                if small>float(prob):
                    small=prob
                    index=count
                count=count+1    
        except:
            pass  
    if small !=0: 
        return list[index][2]  




def query_likely_effect(cause,files):
    
    f = open(files, "r")
    lines=f.readlines()
    f.close
    big=0.0
    count=0
    index=0
    list=[]
    for x in lines:
        try:
            forb, inital, prob= retest.info(x)
            if forb=='forward':
                ind=x.index(prob)
                cause=x[ind-3]
                list.append((forb, inital, cause, float(prob)))
                if big<float(prob):
                    big=prob
                    index=count
                count=count+1
        except:
            pass  
    if big !=10: 
        return list[index][2]



def query_unlikely_effect(cause,files):
    
    f = open(files, "r")
    lines=f.readlines()
    f.close
    small=50.0
    count=0
    index=0
    list=[]
    for x in lines:
        try:
            forb, inital, prob= retest.info(x)
            if forb=='forward':
                ind=x.index(prob)
                cause=x[ind-3]
                list.append((forb, inital, cause, float(prob)))
                if small>float(prob):
                    small=prob
                    index=count
                count=count+1    
        except:
            pass  
    if small !=0: 
        return list[index][2] 

    

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
    filename=os.getcwd()
    clips.SaveFacts(filename+"/savefacts.txt" )
    a=query_likely_cause("E",filename+"/savefacts.txt") 
    print "\n\nLikely Cause is:  "+a
    b=query_unlikely_cause("E",filename+"/savefacts.txt") 
    print "\n\nUnlikely Cause is:  "+b
    c=query_likely_effect("A",filename+"/savefacts.txt")
    print "\n\nLikely Effect is:  "+c
    d=query_unlikely_effect("A",filename+"/savefacts.txt")
    print "\n\nUnlikely Effect is:  "+d+"\n\n\n\n\n\n"

if __name__=="__main__":
    main()