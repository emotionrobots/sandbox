import cv2
import numpy
import os



def main():
    filename = os.getcwd()+'/facepics'
    aurash0=face_detect(cv2.imread(filename+"/aurash.jpeg",1))
    aurash1=face_detect(cv2.imread(filename+"/aurash2.jpeg",1))
    aurash2=face_detect(cv2.imread(filename+"/aurash3.jpeg",1))
    aurash3=face_detect(cv2.imread(filename+"/aurash4.jpeg",1))
    aurash4=face_detect(cv2.imread(filename+"/aurash5.jpeg",1))
    aurash5=face_detect(cv2.imread(filename+"/aurash6.jpeg",1))
    aurash6=face_detect(cv2.imread(filename+"/aurash7.jpeg",1))
    aurash7=face_detect(cv2.imread(filename+"/aurash8.jpeg",1))
    aurash8=face_detect(cv2.imread(filename+"/aurash9.jpeg",1))
    aurash9=face_detect(cv2.imread(filename+"/aurash9.jpeg",1))
    aurash10=face_detect(cv2.imread(filename+"/aurash10.jpeg",1))
    emma0=face_detect(cv2.imread(filename+"/emma.jpg",1))
    emma1=face_detect(cv2.imread(filename+"/emma3.jpg",1))
    recognizer=cv2.createLBPHFaceRecognizer()
    trainingImages = [aurash0, aurash1,aurash2,aurash3,aurash4,aurash5,aurash6,aurash7,aurash8,aurash9,aurash10, emma0, emma1]
    trainingLabels = numpy.array([1,1,1,1,1,1,1,1,1,1,1,2, 2])
    recognizer.train(trainingImages, trainingLabels)
    livedetect(recognizer)






def livedetect(recognizer):
    filename = os.getcwd()+'/Data'
    faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_default.xml")
 

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
            roi_gray = gray[y:y+h, x:x+w]
            roi_color = frame[y:y+h, x:x+w]

        # Display the resulting frame
        

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

        if len(faces)==1:
            testLabel, distance=recognizer.predict(roi_gray)
            #video_capture.release()
            #cv2.destroyAllWindows()
            if testLabel==2:
                text="Emma"
            if testLabel==1:
                text="Aurash"   
            cv2.putText(frame, "Person is:  "+ text, (20,20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
            cv2.putText(frame, "My Confidence is:  "+ str(distance), (20,50), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
            cv2.namedWindow("Video", cv2.WINDOW_NORMAL)
            cv2.imshow('Video', frame)



def face_detect(picture):
        filename = os.getcwd()+'/Data'

        faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_default.xml")

      
        found=False
        while found==False:
            # Capture frame-by-frame
            frame = picture

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            # Draw a rectangle around the faces
            faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(10, 10),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
            for (x, y, w, h) in faces:
                cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
                roi_gray = gray[y:y+h, x:x+w]
                roi_color = frame[y:y+h, x:x+w]

            # Display the resulting frame
            cv2.imshow('Video', frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

            if len(faces)>0:
                found=True
                #video_capture.release()
                #cv2.destroyAllWindows()
                return roi_gray    

if __name__ == '__main__':
    #main()
    face_detect(cv2.imread("/home/aurash/sandbox/sandbox/aurash/landmarking/facepics/aurash2.jpeg"))
