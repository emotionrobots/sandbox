#!/usr/bin/env python2
# -*- coding: utf-8 -*-

# ctypes implementation for STASM library
# http://www.milbo.users.sonic.net/stasm/

import ctypes
import numpy as np
import skimage.io as io
from skimage import img_as_ubyte
import os
import logging
import cv2

F = os.path.dirname(__file__)
FILENAMESTASM = os.path.join(F, "stasm/libstasm.so")
FILENAMEDATA = os.path.join(F, "Data")


class STASM(object):
    """This is a Python wrapper for the STASM library"""

    def __init__(self):
        """Loads de .so file """
        self.stasm = ctypes.cdll.LoadLibrary(FILENAMESTASM)

    def s_init(self, path2data=FILENAMEDATA, debug=0):
        """Gives the location of the Haar Cascade files"""
        self.stasm.stasm_init(path2data, debug)

    def s_search_single(self, filename, numberlandmarks=77,
                        path2data=FILENAMEDATA):
        """Search face and landmarks in picture"""
        
        image=self.s_img2_ubyte(filename)

        self.stasm.stasm_search_single.restypes = [ctypes.c_int]
        self.stasm.stasm_search_single.argtypes = [ctypes.POINTER(
            ctypes.c_int),
            ctypes.POINTER(ctypes.c_float),
            ctypes.POINTER(ctypes.c_char),
            ctypes.c_int,
            ctypes.c_int,
            ctypes.POINTER(ctypes.c_char),
            ctypes.POINTER(ctypes.c_char)]
        foundface = ctypes.c_int()
        xys = 2 * numberlandmarks
        landmarks = (ctypes.c_float * xys)()

        self.stasm.stasm_search_single(ctypes.byref(foundface), landmarks,
            image.ctypes.data_as(ctypes.POINTER(ctypes.c_char)),
            ctypes.c_int(image.shape[1]), ctypes.c_int(image.shape[0]),
            filename, FILENAMEDATA)
        points = np.array(list(landmarks)).reshape((77, 2))

        return points

    def s_open_image(self, filename):

        image=self.s_img2_ubyte(filename)
        self.stasm.stasm_open_image.restypes = [ctypes.c_int]
        self.stasm.stasm_open_image.argtypes = [ctypes.POINTER(
            ctypes.c_char),
            ctypes.c_int,
            ctypes.c_int,
            ctypes.POINTER(ctypes.c_char),
            ctypes.c_int,
            ctypes.c_int]

        self.stasm.stasm_open_image(image.ctypes.data_as(ctypes.POINTER(ctypes.c_char)),
            ctypes.c_int(image.shape[1]), ctypes.c_int(image.shape[0]),
            filename,ctypes.c_int(1),ctypes.c_int(60))

    def s_img2_ubyte(self,filename):
        try:
            image = img_as_ubyte(io.imread(filename, as_grey=True))
            return image
            
        except IOError, exc:
            logging.error(exc.message, exc_info=True)
            raise IOError    

    def s_search_auto(self,numberlandmarks=77):
        """Call repeatedly to find all faces
        out: 0=no more faces, 1=found face.
        out: np array with landmarks.
        """
        #TODO need to be tested
        self.stasm.stasm_search_auto.restypes = [ctypes.c_int]
        self.stasm.stasm_search_auto.argtypes = [ctypes.POINTER(
            ctypes.c_int),
            ctypes.POINTER(ctypes.c_float)
            ]
        foundface = ctypes.c_int()
        xys = 2 * numberlandmarks
        landmarks = (ctypes.c_float * xys)()

        self.stasm.stasm_search_auto(ctypes.byref(foundface), landmarks)
        points = np.array(list(landmarks)).reshape((77, 2))
        
        return points, foundface

    def s_search_pinned(self, pinned, filename):
        """
        Find landmarks, no OpenCV face detect, call after the user has pinned some points
        out: x0, y0, x1, y1, ... (numpy array like)
        in: pinned points (numpy array like)
        in: filename 
        """
        #TODO take this out and make it a method .. after demo expressions ends
        #TODO need to be tested
        image=self.s_img2_ubyte(filename)

        self.stasm.stasm_search_pinned.restypes = [ctypes.c_int]
        self.stasm.stasm_search_pinned.argtypes = [ctypes.POINTER(
            ctypes.c_float),
            ctypes.POINTER(ctypes.c_float),
            ctypes.POINTER(ctypes.c_char),
            ctypes.c_int,
            ctypes.c_int,
            ctypes.POINTER(ctypes.c_char)
            ]
        foundface = ctypes.c_int()
        xys = 2 * numberlandmarks
        landmarks = (ctypes.c_float * xys)()
    
        ctypes_pinned = [np.ctypeslib.as_ctypes(array) for array in pinned]
        pointer_pinned = (ctypes.POINTER(ctypes.c_float) * (pinned.shape[0]* pinned.shape[1]))(*ctypes_pinned)

        self.stasm.stasm_search_pinned(landmarks, pointer_pinned,
            image.ctypes.data_as(ctypes.POINTER(ctypes.c_char)),
            ctypes.c_int(image.shape[1]), ctypes.c_int(image.shape[0]),
            filename, FILENAMEDATA)

        points = np.array(list(landmarks)).reshape((77, 2))

        return points

    def s_stasm_lasterr(self):
        """Return string describing last error"""
        #TODO need to be tested
        self.stasm.stasm_lasterr.restypes = [ctypes.POINTER(ctypes.c_char)]
        self.stasm.stasm_lasterr.argtypes = []

        err = self.stasm.stasm_lasterr()

        return err


    

    def s_force_points_into_image(self, landmarks):
        """Force landmarks into image boundary"""
        self.stasm.s_force_points_into_image= [ctypes.POINTER(
            ctypes.c_float),
            ctypes.c_int,
            ctypes.c_int,]

        self.stasm.stasm_force_points_into_image(landmarks.ctypes.data_as(ctypes.POINTER(ctypes.c_float)),ctypes.c_int(200),ctypes.c_int(200))    

    def s_landmarks_as_shape(self, landmarks):
        #self.stasm.s_landmarks_as_shape.restypes=[ctypes.shape]
        self.stasm.s_landmarks_as_shape.argtypes=[ctypes.POINTER(
            ctypes.c_float)]
        self.stasm.LandmarksAsShape(landmarks.ctypes.data_as(ctypes.POINTER(ctypes.c_float)))



    def s_convert_shape(self,landmarks,int):
        """Convert stasm 77 to 77=nochange
        76=stasm3 68=xm2vts 22=ar 20=bioid 17=me17"""
        self.stasm.s_convert_shape= [ctypes.POINTER(
        ctypes.c_float),
        ctypes.c_int]
 
        self.stasm.stasm_convert_shape(landmarks.ctypes.data_as(ctypes.POINTER(ctypes.c_float)),ctypes.c_int(int))

    def draw(self,landmarks_found_b): 
        img2 = cv2.imread('/home/aurash/Test/test.jpg',1)    
        for (x, y) in landmarks_found_b:
            print (x, y)
            cv2.circle(img2,(int(x),int(y)), 1, (0,0,255), -1)
        cv2.imshow("Display face",img2)
        cv2.waitKey(0)
        cv2.destroyAllWindows()


#-----------------------------------------------------------------


def main():
    #home made test TODO make unitttest
    mystasm = STASM()
    mystasm.s_init()
    
    
    img="/home/aurash/Test/test.jpg" #image to be passed to search function
    mystasm.s_open_image(img)# open image
    landmarks_found_a=mystasm.s_search_auto()
    x=ctypes.c_int(2)
    print x
    print landmarks_found_a[1]
    while(landmarks_found_a[1]==ctypes.c_int(1)):
        print("hello")
        for (x, y) in landmarks_found_a[0]:
            print (x, y)
        landmarks_found_a=mystasm.s_search_auto()
    
    

    landmarks_found_b = mystasm.s_search_single(img)
    mystasm.s_convert_shape(landmarks_found_b,17)
    #mystasm.s_force_points_into_image(landmarks_found_b)
    mystasm.draw(landmarks_found_b)
    print len(landmarks_found_b)


if __name__ == '__main__':
    main()
