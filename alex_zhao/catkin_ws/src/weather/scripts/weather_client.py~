#!/usr/bin/env python
# -*- coding: utf-8 -*-


#current issue=time zone differences; the database's timezone is 5 hours ahead of central, which may be diff from the targeted city's time zone
import sys
import rospy
import urllib2
import json
import ast

#city omits _ characters and is used for openweathermap, city2 has _ characters and is used for worldweatheronline
def weather_client(city, city2):
    locu_api = 'af1d8792d555bdb9c3cc06d2167780cf'
#url is for openweathermap, url2 is for worldweatheronline, same with json_obj and data
    global url 
    url = 'http://api.openweathermap.org/data/2.5/forecast/city?id='
    global url2
    url2 = 'http://api.worldweatheronline.com/premium/v1/weather.ashx?key=fd0609bb53f9423297214032162307&q=' + city2 +'&num_of_days=1&tp=1&format=json'
    global found
    found = 'false'

    with open('/home/alex/catkin_ws/src/weather/scripts/citylist.txt') as inputfile:
        for line in inputfile:
            dictionary = ast.literal_eval(line)
	    if(dictionary['name'] == city):
		url += str(dictionary['_id'])
		found = 'true'
		break
    if(found == 'false'):
	print 'City cannot be found'
	return

    url += '&APPID=af1d8792d555bdb9c3cc06d2167780cf'
    json_obj = urllib2.urlopen(url)
    json_obj2 = urllib2.urlopen(url2)
    data = json.load(json_obj)
    data2 = json.load(json_obj2)
    item = data['list']
    thing = item[1]
    main = thing['main']
    wind = thing['wind']
    thing2 = thing['weather']
    sky = thing2[0]
    wind_degrees = float(wind['deg'])

    item2 = data2['data']
    item3 = item2['current_condition']
    item4 = item3[0]
    observation_time = item4['observation_time']
#    print observation_time
#the first and second characters in that line denote the hour of observation
    time = int(observation_time[0:2])
#the sixth character in that line is the A or P in AM/PM
    am_or_pm = observation_time[6]
    if(am_or_pm == 'P'):
	time += 11
#    print time

    item5 = item2['weather']
    item6 = item5[0]
    item7 = item6['hourly']
    item8 = item7[time]
#    print item8['chanceofrain']

    print 'Listing weather data for ' + city
    print '-------------------------'
    print 'Chance of rain this hour: ' + item8['chanceofrain'] + '%'
    print 'Current temperature: ' + str(1.8 * (main['temp'] - 273) + 32) + '°' + 'F'
    print 'Humidity: ' + str(main['humidity']) + '%'
    print 'Condition: ' + sky['description']
    
    if(wind_degrees < 22.5 or wind_degrees > 337.5):
	print 'Wind: ' + str(wind['speed']) + ' mph North'
    elif(wind_degrees <= 67.5 and wind_degrees >= 22.5):
	print 'Wind: ' + str(wind['speed']) + ' mph Northeast'
    elif(wind_degrees < 112.5 and wind_degrees > 67.5):
	print 'Wind: ' + str(wind['speed']) + ' mph East'
    elif(wind_degrees <= 157.5 and wind_degrees >= 112.5):
	print 'Wind: ' + str(wind['speed']) + ' mph Southeast'
    elif(wind_degrees < 202.5 and wind_degrees > 157.5):
	print 'Wind: ' + str(wind['speed']) + ' mph South'
    elif(wind_degrees <= 247.5 and wind_degrees >= 202.5):
	print 'Wind: ' + str(wind['speed']) + ' mph Southwest'
    elif(wind_degrees < 292.5 and wind_degrees > 247.5):
	print 'Wind: ' + str(wind['speed']) + ' mph West'
    elif(wind_degrees <= 337.5 and wind_degrees >= 292.5):
	print 'Wind: ' + str(wind['speed']) + ' mph Northwest'
    
if __name__ == "__main__":
    if len(sys.argv) == 2:
        x = str(sys.argv[1])
	y = str(sys.argv[1])
    elif len(sys.argv) == 3:
        x = str(sys.argv[1]) + ' ' + str(sys.argv[2])
	y = str(sys.argv[1]) + '_' + str(sys.argv[2])
    elif len(sys.argv) == 4:
        x = str(sys.argv[1]) + ' ' + str(sys.argv[2]) + ' ' + str(sys.argv[3])
	y = str(sys.argv[1]) + '_' + str(sys.argv[2]) + '_' + str(sys.argv[3])
    elif len(sys.argv) == 5:
        x = str(sys.argv[1]) + ' ' + str(sys.argv[2]) + ' ' + str(sys.argv[3]) + ' ' + str(sys.argv[4])
	y = str(sys.argv[1]) + '_' + str(sys.argv[2]) + '_' + str(sys.argv[3]) + '_' + str(sys.argv[4])
    else:
        sys.exit(1)
    weather_client(x, y)
    
