#!/usr/bin/python

import numpy as np
import math 
import cv2
import os
import logging

svm = cv2.SVM()
svmparams = dict( kernel_type = cv2.SVM_LINEAR, svm_type = cv2.SVM_C_SVC, C = 1 )

samples = np.zeros([100, 2], dtype = np.float32)
labels = np.zeros(100, dtype = np.float32)

count = 0
for x in range(0, 100):
    x1 = np.random.random()
    x2 = np.random.random()
    samples[x] = [x1, x2]
    labels[count] = np.sign(math.sin(2*np.pi*x1)*np.sign(math.sin(2*np.pi*x2))) 
    print "x1 = ", x1, "x2 = ", x2, "label = ", labels[count] 
    count = count + 1


svm.train(samples, labels, params = svmparams)

testresult = np.float32([svm.predict(s) for s in samples])

print samples
print labels
print testresult



