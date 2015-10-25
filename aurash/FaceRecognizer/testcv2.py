import cv2
import sys
import dlib
import numpy as np



def main():
    

    predictor_path = sys.argv[1]
    detector = dlib.get_frontal_face_detector()
    predictor = dlib.shape_predictor(predictor_path)
    win = dlib.image_window()
    video_capture = cv2.VideoCapture(0)
    
    while True:
        # Capture frame-by-frame
        ret, frame = video_capture.read()
        list=[]
         
        
        dets = detector(frame, 1)
        win.set_image(frame)
    
        for i in dets:
            
            shape = predictor(frame, i)
            cv2.namedWindow("Video", cv2.WINDOW_NORMAL)
            cv2.imshow('Video', frame)
            win.add_overlay(shape)
            for a in xrange(68):
                b=shape.part(a)
                list.append((b.x,b.y))

        
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

        
        win.clear_overlay() 


main()
