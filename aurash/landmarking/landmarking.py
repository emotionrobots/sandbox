#!/usr/bin/python
# The contents of this file are in the public domain. See LICENSE_FOR_EXAMPLE_PROGRAMS.txt
#
#   This example program shows how to find frontal human faces in an image and
#   estimate their pose.  The pose takes the form of 68 landmarks.  These are
#   points on the face such as the corners of the mouth, along the eyebrows, on
#   the eyes, and so forth.
#
#   This face detector is made using the classic Histogram of Oriented
#   Gradients (HOG) feature combined with a linear classifier, an image pyramid,
#   and sliding window detection scheme.  The pose estimator was created by
#   using dlib's implementation of the paper:
#      One Millisecond Face Alignment with an Ensemble of Regression Trees by
#      Vahid Kazemi and Josephine Sullivan, CVPR 2014
#   and was trained on the iBUG 300-W face landmark dataset.
#
#   Also, note that you can train your own models using dlib's machine learning
#   tools. See train_shape_predictor.py to see an example.
#
#   You can get the shape_predictor_68_face_landmarks.dat file from:
#   http://sourceforge.net/projects/dclib/files/dlib/v18.10/shape_predictor_68_face_landmarks.dat.bz2
#
# COMPILING THE DLIB PYTHON INTERFACE
#   Dlib comes with a compiled python interface for python 2.7 on MS Windows. If
#   you are using another python version or operating system then you need to
#   compile the dlib python interface before you can use this file.  To do this,
#   run compile_dlib_python_module.bat.  This should work on any operating
#   system so long as you have CMake and boost-python installed.
#   On Ubuntu, this can be done easily by running the command:
#       sudo apt-get install libboost-python-dev cmake
#
#   Also note that this example requires scikit-image which can be installed
#   via the command:
#       pip install -U scikit-image
#   Or downloaded from http://scikit-image.org/download.html. 

import sys
import dlib
import cv2
import numpy as np
import Image
import random


predictor_path = sys.argv[1]


detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor(predictor_path)
win = dlib.image_window()
win2 = dlib.image_window()

video_capture = cv2.VideoCapture(0)

        
def main1():
    while True:
        # Capture frame-by-frame
        ret, img = video_capture.read()
        list=[]
        b,g,r = cv2.split(img)
        img = cv2.merge([r,g,b])
         #Ask the detector to find the bounding boxes of each face. The 1 in the
         #second argument indicates that we should upsample the image 1 time. This
        # will make everything bigger and allow us to detect more faces.
        dets = detector(img, 1)
        #print("Number of faces detected: {}".format(len(dets)))
        for i in dets:
            shape = predictor(img, i)
            for a in xrange(68):
                b=shape.part(a)
                list.append((b.x,b.y))
            #map(lambda p: cv2.circle(img, (int(p[0]), int(p[1])), 1, (512,512,255), -1), list)
            try:
                #x,y=delauney(list,img)
                win.clear_overlay()
                win.add_overlay(shape)
                win.set_image(img)
                #win2.clear_overlay()
                #win2.add_overlay(shape)
                #win2.set_image(y)
                #cv2.imshow("vor",y)
            except Exception, e:
                pass
        #Draw the face landmarks on the screen.
      

            #win.add_overlay(dets)

def main2():
    while True:
        # Capture frame-by-frame
        ret, img = video_capture.read()
        list=[]
        b,g,r = cv2.split(img)
        img = cv2.merge([r,g,b])
         #Ask the detector to find the bounding boxes of each face. The 1 in the
         #second argument indicates that we should upsample the image 1 time. This
        # will make everything bigger and allow us to detect more faces.
        dets = detector(img, 1)
        #print("Number of faces detected: {}".format(len(dets)))
        for i in dets:
            shape = predictor(img, i)
            for a in xrange(68):
                b=shape.part(a)
                list.append((b.x,b.y))
            #map(lambda p: cv2.circle(img, (int(p[0]), int(p[1])), 1, (512,512,255), -1), list)
            try:
                x,y=delauney(list,img)
                win.clear_overlay()
                win.add_overlay(shape)
                win.set_image(img)
                win2.clear_overlay()
                win2.add_overlay(shape)
                win2.set_image(y)
                cv2.imshow("vor",y)
            except Exception, e:
                pass            


def rect_contains(rect, point) :
    if point[0] < rect[0] :
        return False
    elif point[1] < rect[1] :
        return False
    elif point[0] > rect[2] :
        return False
    elif point[1] > rect[3] :
        return False
    return True
 
# Draw a point
def draw_point(img, p, color ) :
    cv2.circle( img, p, 2, color, cv2.cv.CV_FILLED, cv2.CV_AA, 0 )
 
 
# Draw delaunay triangles
def draw_delaunay(img, subdiv, delaunay_color ) :
 
    triangleList = subdiv.getTriangleList();
    size = img.shape
    r = (0, 0, size[1], size[0])
 
    for t in triangleList :
         
        pt1 = (t[0], t[1])
        pt2 = (t[2], t[3])
        pt3 = (t[4], t[5])
         
        if rect_contains(r, pt1) and rect_contains(r, pt2) and rect_contains(r, pt3) :
         
            cv2.line(img, pt1, pt2, delaunay_color, 1, cv2.CV_AA, 0)
            cv2.line(img, pt2, pt3, delaunay_color, 1, cv2.CV_AA, 0)
            cv2.line(img, pt3, pt1, delaunay_color, 1, cv2.CV_AA, 0)
 
 
# Draw voronoi diagram
def draw_voronoi(img, subdiv) :
 
    ( facets, centers) = subdiv.getVoronoiFacetList([])
 
    for i in xrange(0,len(facets)) :
        ifacet_arr = []
        for f in facets[i] :
            ifacet_arr.append(f)
         
        ifacet = np.array(ifacet_arr, np.int)
        color = (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
 
        cv2.fillConvexPoly(img, ifacet, color, cv2.CV_AA, 0);
        ifacets = np.array([ifacet])
        cv2.polylines(img, ifacets, True, (0, 0, 0), 1, cv2.CV_AA, 0)
        cv2.circle(img, (centers[i][0], centers[i][1]), 3, (0, 0, 0), cv2.cv.CV_FILLED, cv2.CV_AA, 0)
 
def delauney(list,img):
    # Define window names
    win_delaunay = "Delaunay Triangulation"
    win_voronoi = "Voronoi Diagram"
 
    # Turn on animation while drawing triangles
 #   animate = True
     
    # Define colors for drawing.
    delaunay_color = (0,0,255)
    points_color = (0, 0, 255)
 
    # Read in the image.
    img = img
     
    # Keep a copy around
    img_orig = img.copy();
     
    # Rectangle to be used with Subdiv2D
    size = img.shape
    rect = (0, 0, size[1], size[0])
     
    # Create an instance of Subdiv2D
    subdiv = cv2.Subdiv2D(rect);
 
    # Create an array of points.
    points = list
 
    # Insert points into subdiv
    for p in points :
        subdiv.insert(p)
         
 #       # Show animation
  #      if animate :
   #         img_copy = img_orig.copy()
    #        # Draw delaunay triangles
     #       draw_delaunay( img_copy, subdiv, (255, 255, 255) );
      #      cv2.imshow(win_delaunay, img_copy)
       #     cv2.waitKey(100)
 
    # Draw delaunay triangles
    draw_delaunay( img, subdiv, delaunay_color);
 
    # Draw points
   # for p in points :
   #     draw_point(img, p, (0,0,255))
 
    # Allocate space for Voronoi Diagram
    img_voronoi = np.zeros(img.shape, dtype = img.dtype)
 
    # Draw Voronoi diagram
    draw_voronoi(img_voronoi,subdiv)
 
    # Show results
    #cv2.imshow(win_delaunay,img)
    #cv2.imshow(win_voronoi,img_voronoi)
    #cv2.waitKey(0)        
    return img,img_voronoi



if __name__ == '__main__':
	try:	
	    if sys.argv[2]=='del':
	        main2()
	    else:
	        main1()
	except Exception, e:
			pass            