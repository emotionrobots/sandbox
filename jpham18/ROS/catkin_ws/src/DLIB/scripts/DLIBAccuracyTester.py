#!/usr/bin/env python2

import cv2
import numpy as np
import dlib
import math
from sklearn.svm import SVC
from sklearn.externals import joblib
import skimage.io as io
from skimage import img_as_ubyte
from skimage.color import rgb2gray 
import logging
import os
import DLIBlandmarking
detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor("/home/julian/dlib/examples/build/shape_predictor_68_face_landmarks.dat")

def main():
	svm = joblib.load("emotionDataBase.bin")
	files = os.listdir("./jaffe")
	inte = 5
	# print files
	files.sort()
	for x in files:
		# print x
		list = []
		image = cv2.imread("./jaffe/" + x)
		dets = detector(image, 0)
		for i in dets:
			shape = predictor(image, i)
			for a in xrange(68):
				b=shape.part(a)
				list.append([b.x,b.y])
		list = np.array(list)
		DLIBlandmarking.draw_face(list, image)
		cv2.putText(image, str(x), (10,10), cv2.FONT_HERSHEY_SIMPLEX, .5, (0,0,255))
		cv2.putText(image, str(svm.predict(DLIBlandmarking.detectEmotion(list))), (10,30), cv2.FONT_HERSHEY_SIMPLEX, .4, (255,0,0))
		cv2.namedWindow("test", cv2.WINDOW_NORMAL)
		cv2.imshow("test", image)
		print "./jaffe/" + x
		print DLIBlandmarking.detectEmotion(list)
		print svm.predict(DLIBlandmarking.detectEmotion(list))
		cv2.waitKey(3000)


if __name__ == '__main__':
	main()