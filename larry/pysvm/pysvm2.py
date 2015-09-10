#!/usr/bin/python

import numpy as np
import math 
import cv2
import os

svm = cv2.SVM()
svmparams = dict( kernel_type = cv2.SVM_RBF, svm_type = cv2.SVM_C_SVC, C = 1 )

# X is a list of feature vector.
X = np.array([[-1, -1], [-2, -1], [1, 1], [2, 1], [-2, -2], [8, 4], [5, 3], [4, 8]], dtype = np.float32 )
 
# y is a list of labels. A label could be integer number or a string
y = np.array([1, 1, 3, 2, 4, 3, 4, 5], dtype = np.float32)

svm.train(X, y, params = svmparams)

print svm.predict(np.array([-1.8, -2.1], dtype = np.float32)) 

# 4 should be the output

