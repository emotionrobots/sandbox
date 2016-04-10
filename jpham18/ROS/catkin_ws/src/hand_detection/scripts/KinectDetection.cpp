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
	namedWindow("capture", WINDOW_AUTOSIZE	)
	VideoCapture capture;
	capture.open(CV_CAP_OPENNI);
	for(;;)
	{
		Mat depthMap;
		if(capture.retrieve(depthMap, CV_CAP_OPENNI_DEPTH_MAP))
		{
			imshow("capture", depthMap);
		}
		if(waitKey(30) >= 0)
			break;
	}
}

