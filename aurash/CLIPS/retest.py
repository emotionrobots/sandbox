import re
def info(chain):
	txt=chain

	re1='.*?'	# Non-greedy match on filler
	re2='((?:[a-z][a-z]+))'	# Word 1
	re3='.*?'	# Non-greedy match on filler
	re4='((?:[a-z][a-z]+))'	# Word 2
	re5='.*?'	# Non-greedy match on filler
	re6='(".*?")'	# Double Quote String 1
	re7='.*?'	# Non-greedy match on filler
	re8='([+-]?\\d*\\.\\d+)(?![-+0-9\\.])'	# Float 1

	rg = re.compile(re1+re2+re3+re4+re5+re6+re7+re8,re.IGNORECASE|re.DOTALL)
	m = rg.search(txt)
	if m:
	    forb=m.group(1)
	    word2=m.group(2)
	    inital=m.group(3)
	    prob=m.group(4)
	    return forb, inital, prob