#!/usr/bin/env python2
import cv2
import numpy as np
import os



def main():
    filename = '/home/aurash/emoface'

    hm,hn,her,hel=face_detect(cv2.imread(filename+"/happy.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/anger.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/surprise.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/fear.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/contempt.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/disgust.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/sadness.jpg",1))
    hm,hn,her,hel=face_detect(cv2.imread(filename+"/neutral.jpg",1))

    recognizer=cv2.createLBPHFaceRecognizer()
    trainingImages = [aurash0, aurash1,aurash2,aurash3,aurash4,aurash5,aurash6,aurash7]
    trainingLabels = np.array([0,1,2,3,4,5,6,7])
    recognizer.train(trainingImages, trainingLabels)
    livedetect(recognizer)





def livedetect(recognizer):
    filename = os.getcwd()+'/Data'
    faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_alt2.xml")
 

    video_capture = cv2.VideoCapture(0)
    found=False
    while True:
        # Capture frame-by-frame
        ret, frame = video_capture.read()

        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        # Draw a rectangle around the faces
        faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
        for (x, y, w, h) in faces:
            cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
            roi_gray = gray[y-50:y+h+50, x-50:x+w+50]
            roi_color = frame[y-50:y+h+50, x-50:x+w+50]
            

        # Display the resulting frame
        
        if len(faces)==1:
            testLabel, distance=recognizer.predict(roi_gray)
            #video_capture.release()
            #cv2.destroyAllWindows()
            if testLabel==0:
                text="Happy" 
            if testLabel==1:
                text="Anger"
            if testLabel==2:
                text="Surprise"
            if testLabel==3:
                text="Fear"
            if testLabel==4:
                text="Contempt"
            if testLabel==5:
                text="Disgust"
            if testLabel==6:
                text="Sadness"
            if testLabel==7:
                text="Neutral"
            cv2.putText(frame, "This guy feels "+ text, (20,20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
            cv2.putText(frame, "My Confidence is:  "+ str(distance), (20,50), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
            #cv2.namedWindow("Video", cv2.WINDOW_NORMAL)
            cv2.imshow('Video', frame)
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break


def face_detect1(picture):
    frame = picture
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    return gray

def face_detect(picture):

        data = np.zeros( (480,640,3), dtype=np.uint8)
        filename = os.getcwd()+'/Data'
        res=cv2.resize(picture,(320,240))
        data[80:320,80:400]=res
        faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_alt2.xml")

      
        found=False
        while found==False:
            # Capture frame-by-frame
            frame = data

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            # Draw a rectangle around the faces
            faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
            for (x, y, w, h) in faces:
                cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
                roi_gray = gray[y-50:y+h+50, x-50:x+w+50]
                roi_color = frame[y-50:y+h+50, x-50:x+w+50]

            # Display the resulting frame
            cv2.imshow('Video', roi_color)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

            if len(faces)>0:
                found=True
                #video_capture.release()
                #cv2.destroyAllWindows()
                return roi_gray    

if __name__ == '__main__':
    main()
    