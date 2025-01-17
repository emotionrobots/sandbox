#!/usr/bin/env python2
import cv2
import sys
import os
import freenect
import frame_convert
#import cv2.cv as cv

filename = os.getcwd()+'/Data'
faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_default.xml")




while True:

    # Capture frame-by-frame
    frame = frame_convert.video_cv(freenect.sync_get_video()[0])
    print frame

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    faces = faceCascade.detectMultiScale(
        gray,
        scaleFactor=1.1,
        minNeighbors=5,
        minSize=(30, 30),
        flags=cv2.cv.CV_HAAR_SCALE_IMAGE
    )

    # Draw a rectangle around the faces
    for (x, y, w, h) in faces:
        cv2.rectangle(frame, (x, y), (x+w, y+h), (0, 255, 0), 2)

    # Display the resulting frame
    cv2.imshow('Video', frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# When everything is done, release the capture
video_capture.release()
cv2.destroyAllWindows()