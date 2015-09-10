(defclass CAUSAL (is-a USER)
 (slot cause (create-accessor read-write))
 (slot effect (create-accessor read-write))
 (slot context (create-accessor read-write))
 (slot prob (create-accessor read-write))
)

(defrule forward-chain-starter
 ?f <- (cause ?c)
=>
 (assert (forward-chain ?c 1))
)

(defrule backward-chain-starter
 ?f <- (effect ?e)
=>
 (assert (backward-chain ?e 1))
)

(defrule forward-chaining
 ?f <- (forward-chain $?s ?c ?p1)
 ?obj <- (object (is-a CAUSAL) (cause ?c) (prob ?p2))
=>
 (bind ?e (send ?obj get-effect))
 (assert (forward-chain $?s ?c ?e (* ?p1 ?p2)))
)

(defrule backward-chaining
 ?f <- (backward-chain $?s ?e ?p1)
 ?obj <- (object (is-a CAUSAL) (effect ?e) (prob ?p2))
=>
 (bind ?c (send ?obj get-cause))
 (assert (backward-chain $?s ?e ?c (* ?p1 ?p2)))
)

(deffacts starting-facts
 (effect E)
 (cause A)
)

(definstances starting-instances
 (a of CAUSAL (cause A) (effect B) (prob 0.5))
 (b of CAUSAL (cause B) (effect C) (prob 0.1))
 (c of CAUSAL (cause C) (effect D) (prob 0.2))
 (d of CAUSAL (cause B) (effect E) (prob 0.8))
 (e of CAUSAL (cause C) (effect E) (prob 0.3))
)
