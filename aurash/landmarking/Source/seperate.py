#!/usr/bin/env python2

import cv2
import numpy as np
import pyasm
import argparse

FILENAME =  '/tmp/out.webm'


def main():


    parser = argparse.ArgumentParser("-h",description='Provide an image for landmarking with\n one or more faces to be found(click delete to close the picture closing the picture out without use of "delete" will brick the terminal')
    parser.add_argument('-i','--i', help='Please provide an image location (path)', type=str,required=True)
    parser.add_argument('-m','--m', help='multiple face search feature')
    parser.add_argument('-s','--s', help='single face search feature')
    args = parser.parse_args()
    img=args.i
    #home made test TODO make unitttest
    if args.m:
        mystasm = pyasm.STASM()
        mystasm.s_init()    
         #image to be passed to search function
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
    if args.s:
        mystasm = pyasm.STASM()
        mystasm.s_init()   
        landmarks_found_b=[]        
        landmarks_found_b.append(mystasm.s_search_single(img))
        mystasm.draw(landmarks_found_b,img)
        #mystasm.s_convert_shape(landmarks_found_b,17)
        #mystasm.s_force_points_into_image(landmarks_found_b)
if __name__ == '__main__':
    main()
