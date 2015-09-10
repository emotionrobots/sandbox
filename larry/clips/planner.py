#!/usr/bin/python
import clips

clips.Build("""
   (defclass CAUSAL
      (is-a USER)
      (slot cause (create-accessor read-write))
      (slot effect (create-accessor read-write))
      (slot context (create-accessor read-write))  
      (slot prob (create-accessor read-write))) """)

c = clips.FindClass('CAUSAL')
print c.PPForm()
