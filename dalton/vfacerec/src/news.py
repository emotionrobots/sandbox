#!/usr/bin/env python

import sys
import rospy
from std_msgs.msg import String
import feedparser
from newspaper import Article

feed = feedparser.parse('http://feeds.reuters.com/reuters/topNews')

for e in feed['entries']:
		print ""
		print e.get('title','')
		print ""
		print e.get('published', '')
		url = e.get('link','')
		a = Article(url)
		a.download()
		a.parse()
		print(a.text[:400])