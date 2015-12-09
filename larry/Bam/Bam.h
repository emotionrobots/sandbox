/* !
 * @{
 *=====================================================================
 *
 *  @file	Bam.h
 *
 *  @brief	Bidirectional Associative Memory
 *
 *  Copyright	E-Motion Inc.  2014-2015.  All rights reserved
 *	
 *=====================================================================
 */
#ifndef __BAM_H__ 
#define __BAM_H__ 
#include <iostream>
#include <cstdlib>
#include <vector>
#include <math.h>

using namespace std;

class Bam 
{
public:
   Bam();
   Bam(int xdim, int ydim, float s2=1);
   ~Bam(); 
   void reset();
   void init(int xdim, int ydim, float xv, float yv, float s2=1);
   bool connectX(Bam *b);
   bool connectY(Bam *b);
   Bam *disconnectX();
   Bam *disconnectY();
   void updateForward(vector<float> &out);
   void updateForward();
   void updateBackward(vector<float> &out);
   void updateBackward();
   bool learn(vector<float> &x, vector<float> &y);
   inline void setSigma2(float v) {sigma2 = v;}
   inline int getDimX() {return xdim;}
   inline int getDimY() {return ydim;}
   inline vector<float> &getInX() {return xIn;}
   inline vector<float> &getInY() {return yIn;}
   inline vector< vector<float> > &getMemory() {return memory;}
   inline void setCapacity(int cap) {capacity = cap;}
 
private:
   bool bInit;
   int xdim, ydim;
   int capacity;
   int overwrite;
   vector<float> xIn;
   vector<float> yIn;
   vector< vector<float> > memory; 
   Bam *next, *prev;
   float sigma2;
};

#endif // __BAM_H__ 
/*! @} */
