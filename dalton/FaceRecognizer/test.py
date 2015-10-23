import cv2
import numpy
import os
import time



def main():
    filename = os.getcwd()+'/facepics'

    recognizer=cv2.createLBPHFaceRecognizer()

    settings = "data.txt"
    lastIDNum = "lastID.txt"

    if(os.path.isfile(lastIDNum)):
        readID = open(lastIDNum, "r")
        te = readID.read()
        readID.close()
        #print(te)
    else:
        writeID = open(lastIDNum, "w")
        print "writing"
        writeID.write('1')
        writeID.flush()
        writeID.close()

    if(os.path.isfile(settings)):
        readSettings = open(settings, "r")
        readSettings.close()
    else:
        writeSettings = open(settings, "w")
        writeSettings.write("")
        writeSettings.close()

    if(os.path.isfile("People.xml")):
        print "People.xml exists"
        recognizer.load("People.xml")
    else:
        print "People.xml doesn't exist"
        #aurash0=face_detect(cv2.imread(filename+"/aurian.jpg",1))
        #aurash1=face_detect(cv2.imread(filename+"/aurian.jpg",1))
        #emma0=face_detect(cv2.imread(filename+"/emma.jpg",1))
        #emma1=face_detect(cv2.imread(filename+"/emma3.jpg",1))
        #trainingImages = [aurash0, aurash1, emma0, emma1]
        #trainingLabels = numpy.array([1, 1, 2, 2])
        #recognizer.train(trainingImages, trainingLabels)
        recognizer.save("People.xml")

    tmp = str(raw_input('Type "t" to train and "y" to start detection:'))

    if(tmp == "t"):
        trainer(recognizer)
    elif(tmp == "y"):
        livedetect(recognizer)
    else:
        print "\n\nUnknown Command\n\n"


    #time.sleep(2)
    #trainer(recognizer)

    #cv2.destroyAllWindows()
    #time.sleep(5)
    #livedetect(recognizer)

    #video_capture.release()
    
    #livedetect(recognizer)


def trainer(recognizer):
    filename = os.getcwd()+'/Data'
    faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_alt2.xml")

    readID = open('lastID.txt', "r")
    
    ID = int(readID.read())
    readID.close()

    IDTemp = ID;

    trainingImages = []
    trainingLabels = numpy.array([], dtype=int)
    trainingNames = []

    video_capture = cv2.VideoCapture(0)
    numPics = 0

    name = str(raw_input('What is your name, human? \n name:'))

    print str(ID) + ", " + name 
    #print(''.join(str(ord(c)) for c in name) + ", " + name + "\n")

    flag = False

    with open('data.txt', 'r') as f:
        for line in f:
            print line
            allNamesInLine = line.split(";")
            allNamesInLine.pop()       #remove empty value
            print allNamesInLine
            for IDNameVal in allNamesInLine:
                idNames = IDNameVal.split(", ")

                if str(idNames[1]) == name:
                    print "name exists, don't write to file: " + str(idNames[1])
                    ID = int(idNames[0])
                    flag = True
                    break

        if flag == False:
            print "name doesnt exist, write to file"
            writeSettings = open("data.txt", "a")
            writeSettings.write(str(ID) + ", " + name + ";")
            writeSettings.close()

    print ID


    start = time.time()

    while True:

        ret, frame = video_capture.read()

        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        faces = faceCascade.detectMultiScale(gray,scaleFactor=1.2,minNeighbors=5,minSize=(30, 30),flags=cv2.cv.CV_HAAR_SCALE_IMAGE)
        for(x, y, w, h) in faces:
            cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
            roi_gray = gray[y:y+h, x:x+w]
            roi_color = frame[y:y+h, x:x+w]

        if cv2.waitKey(1) & 0xFF == ord('q'):
            trainingLabels = numpy.array(trainingNames)
            print len(trainingImages)
            print trainingLabels.shape
            recognizer.update(trainingImages, trainingLabels)
            recognizer.save("People.xml")
            if(flag):
                print "Using existing ID for name"
                #writeID = open("lastID.txt", "w")
                #writeID.write(str(ID))
                #writeID.close()
            else:
                print "Incrementing"
                ID += 1
                writeID = open("lastID.txt", "w")
                writeID.write(str(ID))
                writeID.close()
            #livedetect(recognizer)
            #cv2.destroyAllWindows()
            break

        cv2.putText(frame, "Press 'Q' to take finish training", (20,20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)

        count = 0;

        if len(faces) == 1:
            #cv2.imshow('Trainer', frame)
            cv2.putText(frame, "Press 'B' to take a picture", (x+20,y+20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
            #cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)

            #if int(time.time()-start) % 10 == 0 :
             #   count+=1

            #if count % 20 == 0:
            if cv2.waitKey(150) & 0xFF == ord('b'):
                print "Click!"
                #directly add to array
                trainingImages.append(face_detect(frame))
                trainingNames.append(ID)
                
                #numpy.append(trainingLabels, int(''.join(str(ord(c)) for c in name)))
                print len(trainingImages)
                print len(trainingNames)

                #start = time.time()
                #print  pathdir+name+str(time.time()-start)+'.jpg'
                #cv2.imwrite( pathdir+nome+'/'+str(time.time()-start)+'.jpg', resized_image );

        cv2.imshow('Trainer', frame)
        cv2.namedWindow("Trainer", cv2.WINDOW_NORMAL)
        



#add a face rotating method so the software can train on fabricated rotated image of person





def livedetect(recognizer):
    filename = os.getcwd()+'/Data'
    faceCascade = cv2.CascadeClassifier(filename+"/haarcascade_frontalface_alt2.xml")

    ids = []
    labels = []
 

    with open('data.txt', 'r') as f:
        for line in f:
            print line
            allNamesInLine = line.split(";")
            allNamesInLine.pop()       #remove empty value
            print allNamesInLine
            for IDNameVal in allNamesInLine:
                idNames = IDNameVal.split(", ")
                ids.append(int(idNames[0]))
                labels.append(str(idNames[1]))


    video_capture = cv2.VideoCapture(0)
    found=False

    prevText = ""

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
            text = ""

            cv2.namedWindow("Video", cv2.WINDOW_NORMAL)

            if(distance < 40):
            #video_capture.release()
            #cv2.destroyAllWindows()

            #for each name in file see whcih matches

                for idx, ID in enumerate(ids):
                    if(testLabel == ID):
                        text=labels[idx]
                        break

                

            #if testLabel==2:
            #    text="Emma"
            #if testLabel==1:
            #    text="Aurash"   
                cv2.putText(frame, "Person is:  " + text, (x+20,y-20), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)
                cv2.putText(frame, "My Confidence is:  " + str(distance), (20,50), cv2.FONT_HERSHEY_SIMPLEX, .5, 255)

                if text != prevText:
                    print text
                    prevText = text

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

        #video_capture = cv2.VideoCapture(0)
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
            #cv2.imshow('Video', frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

            if len(faces)>0:
                found=True
                #video_capture.release()
                #cv2.destroyAllWindows()
                return roi_gray    


main()