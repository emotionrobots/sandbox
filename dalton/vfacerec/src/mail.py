#!/usr/bin/env python

import sys
import imaplib
import email
import rospy
from std_msgs.msg import String

mail = imaplib.IMAP4_SSL('imap.gmail.com')
(retcode, capabilities) = mail.login('emotionrobotstest@gmail.com','emotioneering')
mail.list()
mail.select('inbox')

typ, data = mail.search(None, 'ALL')
ids = data[0]
id_list = ids.split()
#get the most recent email id
latest_email_id = int( id_list[-1] )


#iterate through 15 messages in decending order starting with latest_email_id
#the '-1' dictates reverse looping order

def pubMail():
	for i in range( latest_email_id, latest_email_id-15, -1 ):
	   typ, data = mail.fetch( i, '(RFC822)' )

	   for response_part in data:
	      if isinstance(response_part, tuple):
	          msg = email.message_from_string(response_part[1])
	          varSubject = msg['Subject']
	          varFrom = msg['From']
	          typ, data = mail.store(i,'+FLAGS','\\Seen')

	   #remove the brackets around the sender email address
	   #varFrom = varFrom.replace('<', '')
	   #varFrom = varFrom.replace('>', '')

	   #add ellipsis (...) if subject length is greater than 35 characters
	   if len( varSubject ) > 55:
	      varSubject = varSubject[0:55] + '...'

	   test = varFrom.split()[0].replace("\"", "")
	   newmailpublisher.publish("New email from " + test + " " + varFrom.split()[-1] + ": " + varSubject +"\n")
	   print "New email from " + test + " " + varFrom.split()[-1] + ": " + varSubject +"\n"

def publisher():
	rospy.init_node('publisher')
	global newmailpublisher
	newmailpublisher = rospy.Publisher('get_mail', String, queue_size=10)
	pubMail()
publisher()
# (retcode, messages) = mail.search(None, '(UNSEEN)')
# if retcode == 'OK':

#    for num in messages[0].split() :
#       print 'Processing '
#       n=n+1
#       typ, data = mail.fetch(num,'(RFC822)')
#       for response_part in data:
#          if isinstance(response_part, tuple):
#              original = email.message_from_string(response_part[1])

#              print original['From']
#              print original['Subject']
#              typ, data = mail.store(num,'+FLAGS','\\Seen')