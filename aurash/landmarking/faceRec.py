import cv2
import numpy
import os



def main():
    filename = os.getcwd()+'/facepics'
    aurash0=face_detect(cv2.imread(filename+"/aurian.jpg",1))
    aurash1=face_detect(cv2.imread(filename+"/aurian.jpg",1))
    emma0=face_detect(cv2.imread(filename+"/emma.jpg",1))
    emma1=face_detect(cv2.imread(filename+"/emma3.jpg",1))
    recognizer=cv2.createLBPHFaceRecognizer()
    trainingImages = [aurash0, aurash1, emma0, emma1]
    trainingLabels = numpy.array([1, 1, 2, 2])
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






   # while True:
	#	if testLabel==2:
	#	    #print "This is Emma"
	#	    cv2.imshow("RecResult",emma1)
	#	    if cv2.waitKey(1) & 0xFF == ord('q'):
	#	        video_capture.release()
	#	        cv2.destroyAllWindows()
	#	if testLabel==1:
	#	    #print "This is Aurash"
	#	    cv2.imshow("RecResult",aurash1)	
	#	    if cv2.waitKey(1) & 0xFF == ord('q'):
	#	        video_capture.release()
	#	        cv2.destroyAllWindows()











def face_detect(picture):
        filename = os.getcwd()+'/Data'

        faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_alt2.xml")

      
        found=False
        while found==False:
            # Capture frame-by-frame
            frame = picture

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            # Draw a rectangle around the faces
            faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
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


main()
