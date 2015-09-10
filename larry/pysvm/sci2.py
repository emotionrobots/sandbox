#!/usr/bin/python

import numpy as np
from sklearn.svm import SVC
from sklearn.externals import joblib 

# X is a list of feature vector.
X = np.array([[-1, -1], [-2, -1], [1, 1], [2, 1], [-2, -2], [8, 4], [5, 3], [4, 8]])
# y is a list of labels. A label could be integer number or a string
y = np.array(['a', 'a', 'c', 'b', 'd', 'c', 'd', 'e'])


# clf is the SVM Classifier
clf = SVC()

# Train the SVC
clf.fit(X, y) 

# Print the prediction - note [-1.8, -2.1] is very close to [-2, -2], which 
# maps to 'd' positionally (5th position) 
print(clf.predict([-1.8, -2.1]))
# 'd' should be the output

# save file as binary
joblib.dump(clf, "mysvm_save.pkl", compress=3)

# reload saved file and run the svm
clf2 = joblib.load("mysvm_save.pkl")
print(clf2.predict([-1.8, -2.1]))
# 'd' should be the output
