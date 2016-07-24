#!/usr/bin/env python

from weather.srv import *
import rospy

def handle_weather(city):
    print city + ' is chosen'

def weather_server():
    rospy.init_node('weather_server')
    s = rospy.Service('weather', response, handle_weather)
    print "What city would you like to know about?"
    rospy.spin()

if __name__ == "__main__":
    weather_server()
