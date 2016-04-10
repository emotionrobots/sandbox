#define __kinectDetection_CPP__

// #include "kinectDetection.h"
#include <iostream>

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;
using namespace std;

int main() {
	cout << "hello" ;
	namedWindow("test", WINDOW_NORMAL);
	VideoCapture capture(0);
	// capture.open(CV_CAP_OPENNI);
	for(;;)
	{
		Mat img;
		capture.grab();
		capture.retrieve(img, CV_CAP_OPENNI_BGR_IMAGE);
		cout << img;	
		imshow("test", img);
		// if(waitKey(30) >= 0);
			// break;
		waitKey();
	}
}