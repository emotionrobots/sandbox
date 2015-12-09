/* !
 * @{
 *=====================================================================
 *
 *  @file	ut_Bam.cpp
 *
 *  @brief	Bidirectional Associative Memory Unit Test 
 *
 *  Copyright	E-Motion Inc.  2014-2015.  All rights reserved
 *	
 *=====================================================================
 */
#define __UT_BAM_CPP__ 

#include "Bam.h" 

using namespace std;

int main (int argc, char **argv)
{
   float pi = 3.1415926;
   Bam b(2, 2); 
   vector<float> xvec;
   vector<float> yvec;

   if (argc != 4) {
      cout << "Usage: ./Bam <sigma2> <samples> <iter> " << endl;
      return 1;
   }
   float sigma2 = atof(argv[1]); 
   int samples  = atoi(argv[2]); 
   int iter     = atoi(argv[3]); 

   b.setSigma2(sigma2);

   // Create scratchpad vectors
   for (int i=0; i< b.getDimX(); i++)
      xvec.push_back(0);
   for (int i=0; i< b.getDimY(); i++)
      yvec.push_back(0);

   // Learning
   for (float x=0; x <= 1; x+= 1.0/samples) {
      for (float y=0; y <= 1; y += 1.0/samples) {
          xvec[0] = x;
          xvec[1] = y;
          yvec[0] = cos(x*2*pi);
          yvec[1] = 1-sin(y*2*pi); 
          b.learn(xvec, yvec);           
      }
   }
 
   float xerr = 0;
   float yerr = 0;
   float xerr_sum = 0;
   float yerr_sum = 0;
   int count = 0;

   // Recall
   float x = 0;
   float y = 0;
   for (x = 0; x <= 1.0; x+=2.1/samples) { 
      for (y = 0; y <= 1.0; y+=1.8/samples) {   

         b.getInX()[0] = xvec[0] = x;
         b.getInX()[1] = xvec[1] = y;

         float r1 = cos(x*2*pi);
         float r2 = 1-sin(y*2*pi); 

         for (int i=0; i<iter; i++) {
            b.updateForward(b.getInY());
            b.updateBackward(b.getInX());
         }

         yerr = (r1-b.getInY()[0])*(r1-b.getInY()[0])
              + (r2-b.getInY()[1])*(r2-b.getInY()[1]);
         xerr = (x-b.getInX()[0])*(x-b.getInX()[0])
              + (y-b.getInX()[1])*(y-b.getInX()[1]);
         yerr_sum += yerr;
         xerr_sum += xerr;
         count++;
      }
   }
   cout << "xerr=" << sqrt(xerr_sum)/count << " " 
        << "yerr=" << sqrt(yerr_sum)/count << " "
        << "data points = " << b.getMemory().size() << " "
        << "tested points = " << count << endl; 
}

#undef __UT_BAM_CPP__ 
/*! @} */
