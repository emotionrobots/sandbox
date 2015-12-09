/* !
 * @{
 *=====================================================================
 *
 *  @file	Bam.cpp
 *
 *  @brief	Bidirectional Associative Memory
 *
 *  Copyright	E-Motion Inc.  2014-2015.  All rights reserved
 *	
 *=====================================================================
 */
#define __BAM_CPP__
 
#include "Bam.h" 

using namespace std;

static float gaussian2(float x2, float s2)
{
   return expf(-x2/s2);
}

static float lambda2(float x2, float s2)
{
   float v = 1 - x2/s2;  
   return (v > 0) ? v : 0;  
}

Bam::Bam()
{
}


Bam::Bam(int xd, int yd, float s2)
{
   bInit = false;
   next = prev = NULL;
   init(xd, yd, 0, 0, s2); 
}


void Bam::init(int xd, int yd, float xv, float yv, float s2)
{
   xdim = xd;
   ydim = yd;
   sigma2 = s2;
   for (int i=0; i < xdim; i++) 
      xIn.push_back(xv);
   for (int i=0; i < ydim; i++)
      yIn.push_back(yv);
   bInit = true;
   overwrite = 0;
   capacity = -1;
}


void Bam::reset()
{
   xIn.clear();
   yIn.clear();
   bInit = false;
   next = prev = NULL;
   init(xdim, ydim, 0, 0, sigma2);
}


Bam::~Bam()
{
}


bool Bam::connectX(Bam *b)
{
   bool rc = false;
   if (b->bInit && bInit && b->ydim == xdim) { 
      b->next = this;
      prev = b;
      rc = true;
   }
   return rc;
}

Bam *Bam::disconnectX()
{
   Bam *p = NULL;   
   if (prev != NULL) {
      p = prev;
      prev->next = NULL;
      prev = NULL; 
   }
   return p;
}


bool Bam::connectY(Bam *b)
{
   bool rc = false;
   if (b->bInit && bInit && b->xdim == ydim) { 
      b->prev = this;
      next = b;
      rc = true;
   }
   return rc;
}


Bam *Bam::disconnectY()
{
   Bam *p = NULL;   
   if (next != NULL) {
      p = next;   
      next->prev = NULL;
      next = NULL; 
   }
   return p;
}

void Bam::updateForward(vector<float> &out)
{
   if (bInit && out.size() == ydim) {
      for (int j = 0; j < ydim; j++) { 
         float num = 0, den = 0, sumx = 0, sumy=0;
         for (int k = 0; k < memory.size(); k++) {
            sumx = sumy = 0;
            for (int i=0; i < xdim; i++) { 
               float e = xIn[i] - memory[k][i]; 
               sumx += e*e;
            }
            float g = gaussian2(sumx, sigma2);
            num += g*memory[k][xdim+j]; 
            den += g; 
         }
         float v = (den > 0) ? num/den : 0;
         out[j] = v;
      } // for (j)
   } // if (bInit)
}

void Bam::updateForward()
{
   if (next != NULL && next->xdim == ydim) {
      updateForward(next->xIn);
   }
}


void Bam::updateBackward(vector<float> &out)
{
   if (bInit) {
      out.clear();
      for (int j = 0; j < xdim; j++) { 
         float num = 0, den = 0, sumx = 0, sumy=0;
         for (int k = 0; k < memory.size(); k++) {
            sumx = 0;
            for (int i=0; i < xdim; i++) { 
               float e = xIn[i] - memory[k][i]; 
               sumx += e*e;
            }
            float g = gaussian2(sumx, sigma2);
            num += g * memory[k][j]; 
            den += g; 
         }
         out.push_back((den > 0) ? num/den : 0);
      } // for (j)
   } // if (bInit)
}


void Bam::updateBackward()
{
   if (prev != NULL && prev->ydim == xdim) {
      updateBackward(prev->yIn);
   }
}


bool Bam::learn(vector<float> &x, vector<float> &y)
{
   bool rc = false;
   vector<float> a;

   if (x.size() == xdim && y.size() == ydim) {
      for (int i=0; i < xdim; i++) a.push_back(x[i]);
      for (int i=0; i < ydim; i++) a.push_back(y[i]);
      if (memory.size() < capacity) {
         memory.push_back(a);
      }
      else {
         memory[overwrite++] = a; 
         if (overwrite >= memory.size()) 
            overwrite = 0;
      }
      rc = true;
   }
   return rc;
}

#undef __BAM_CPP__ 
/*! @} */
