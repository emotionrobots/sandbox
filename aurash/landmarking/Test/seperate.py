#!/usr/bin/env python2

import cv2
import numpy as np
import pyasm





FILENAME =  '/tmp/out.webm'


def main():
    #home made test TODO make unitttest
    mystasm = pyasm.STASM()
    mystasm.s_init()    
    img="/home/aurash/Test/img.jpg" #image to be passed to search function
    mystasm.s_open_image(img)# open image
    # landmarks_found_a=mystasm.s_search_auto()
    (landmark_array, landmark_found) = mystasm.s_search_auto()
    
    pointst=[]
    count=1
    print("\n\n\n")
    while landmark_found.value == 1:
        if landmark_found.value==1:
            print("**************************************Face " +str(count)+"**************************************")
            count=count+1
            pointst.append(landmark_array) 
            for (x, y) in landmark_array:
                print (x, y)   
            (landmark_array, landmark_found) = mystasm.s_search_auto()
    if landmark_found.value == 0:     
        print len(pointst)
        mystasm.draw(pointst,img)
           

    #landmarks_found_b = mystasm.s_search_single(img)
    #mystasm.s_convert_shape(landmarks_found_b,17)
    #mystasm.s_force_points_into_image(landmarks_found_b)
   
    #print len(landmarks_found_b)


if __name__ == '__main__':
    main()