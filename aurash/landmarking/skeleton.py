#!/usr/bin/python
# coding: utf-8

# In[ ]:

import openni as opi
import numpy as np
import Image
#import matplotlib.pyplot as plt
import pygame
from pygame.locals import KEYDOWN, K_ESCAPE
#from speak import speak


POSE2USE = 'Psi'
name_joints = ['SKEL_HEAD', 'SKEL_LEFT_FOOT', 'SKEL_RIGHT_SHOULDER',
                       'SKEL_LEFT_HAND', 'SKEL_NECK',
                       'SKEL_RIGHT_FOOT', 'SKEL_LEFT_HIP', 'SKEL_RIGHT_HAND',
                       'SKEL_TORSO', 'SKEL_LEFT_ELBOW', 'SKEL_LEFT_KNEE',
                       'SKEL_RIGHT_HIP', 'SKEL_LEFT_SHOULDER',
                       'SKEL_RIGHT_ELBOW', 'SKEL_RIGHT_KNEE']


# #### Create de Context and Generators

# In[ ]:

ctx = opi.Context()
ctx.init()


# In[ ]:

depth_generator = opi.DepthGenerator()
depth_generator.create(ctx)
depth_generator.set_resolution_preset(opi.RES_VGA)
depth_generator.fps = 30

image_generator = opi.ImageGenerator()
image_generator.create(ctx)

user = opi.UserGenerator()
user.create(ctx)


# #### Get the skeleton and pose capabilites from the user generator

# In[ ]:

skel_cap = user.skeleton_cap
pose_cap = user.pose_detection_cap


# #### Create and register some callbacks
def new_user(src, id):
    s = "Hi User " +  str(id) + "."
    #speak.say(s)
    s = "Make a secret pose."
    #speak.say(s)
    pose_cap.start_detection(POSE2USE, id)

def lost_user(src, id):
    s = "Bye Bye User " +  str(id) + "."
    #speak.say(s)

def pose_detected(src, pose, id):
    print "The User %s is doing the secret pose %s, now do the calibration" %(id, pose)
    pose_cap.stop_detection(id)
    skel_cap.request_calibration(id, True)

def calibration_complete(src, id, status):
    if status == opi.CALIBRATION_STATUS_OK:
        print "Congrats User %s! You're Calibrated" %(id)
        skel_cap.start_tracking(id)
    else:
        print "Something went wrong User %s :(" %(id)
        new_user(user, id)


user.register_user_cb(new_user, lost_user)
pose_cap.register_pose_detected_cb(pose_detected)
skel_cap.register_c_complete_cb(calibration_complete)
skel_cap.set_profile(opi.SKEL_PROFILE_ALL)


# #### Now, lets get the x,y position of our joints ...
def get_joints():
    for id in user.users:
        if skel_cap.is_tracking(id) and skel_cap.is_calibrated(id):
            joints = [skel_cap.get_joint_position(id, j)
                  for j in map(lambda a: getattr(opi, a), name_joints)]
            return depth_generator.to_projective([j.point for j in joints])


# #### And a camera to see the output
def capture_rgb():
    rgb_frame = np.fromstring(image_generator.get_raw_image_map_bgr(),
                              dtype=np.uint8).reshape(480, 640, 3)
    im = Image.fromarray(rgb_frame)
    b, g, r = im.split()
    im = Image.merge("RGB", (r, g, b))
    return pygame.image.frombuffer(im.tostring(), im.size, 'RGB')


#name_joints = ['SKEL_HEAD', 'SKEL_LEFT_FOOT', 'SKEL_RIGHT_SHOULDER',
#                       'SKEL_LEFT_HAND', 'SKEL_NECK',
#                       'SKEL_RIGHT_FOOT', 'SKEL_LEFT_HIP', 'SKEL_RIGHT_HAND',
#                       'SKEL_TORSO', 'SKEL_LEFT_ELBOW', 'SKEL_LEFT_KNEE',
#                       'SKEL_RIGHT_HIP', 'SKEL_LEFT_SHOULDER',
#                       'SKEL_RIGHT_ELBOW', 'SKEL_RIGHT_KNEE']

sk_head = 0
sk_left_foot = 1
sk_right_shoulder= 2
sk_left_hand= 3
sk_neck	= 4
sk_right_foot= 5
sk_left_hip = 6
sk_right_hand	= 7
sk_torso = 8
sk_left_elbow 	= 9
sk_left_knee 	= 10 
sk_right_hip 	= 11 
sk_left_shoulder= 12 
sk_right_elbow 	= 13 
sk_right_knee 	= 14 


def draw_skeleton(screen, pos):
   if (pos):
      # Upper extremeties
      startpos = pos[sk_right_shoulder] 
      endpos =   pos[sk_left_shoulder] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      pygame.draw.circle(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), 10, 4) 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      startpos = pos[sk_left_shoulder] 
      endpos =   pos[sk_torso] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_torso] 
      endpos =   pos[sk_right_shoulder] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_right_shoulder] 
      endpos =   pos[sk_neck] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_left_shoulder] 
      endpos =   pos[sk_neck] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_neck] 
      endpos =   pos[sk_head] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_right_shoulder] 
      endpos =   pos[sk_right_elbow] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_right_elbow] 
      endpos =   pos[sk_right_hand] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_left_shoulder] 
      endpos =   pos[sk_left_elbow] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_left_elbow] 
      endpos =   pos[sk_left_hand] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 

      # Lower extremeties
      startpos = pos[sk_torso] 
      endpos =   pos[sk_right_hip] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_torso] 
      endpos =   pos[sk_left_hip] 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_right_hip] 
      endpos =   pos[sk_left_hip] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      startpos = pos[sk_right_hip] 
      endpos =   pos[sk_right_knee] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      startpos = pos[sk_right_knee] 
      endpos =   pos[sk_right_foot] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      startpos = pos[sk_left_hip] 
      endpos =   pos[sk_left_knee] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
      startpos = pos[sk_left_knee] 
      endpos =   pos[sk_left_foot] 
      pygame.draw.line(screen, (255, 5, 0), (int(startpos[0]), int(startpos[1])), (int(endpos[0]), int(endpos[1])), 4) 
      pygame.draw.circle(screen, (255, 5, 0), (int(endpos[0]), int(endpos[1])), 10, 4) 
   return
 

# #### Run sensor run

def main():
   ctx.start_generating_all()
   pygame.init()
   running = True
   screen = pygame.display.set_mode((640, 480), pygame.HWSURFACE | pygame.DOUBLEBUF)
   pygame.display.set_caption('Skeleton View')

   while running:
      for event in pygame.event.get():
         if event.type == KEYDOWN and event.key == K_ESCAPE: 
            running = False
      ctx.wait_any_update_all()
      screen.blit(capture_rgb(), (0, 0))
      newpos_skeleton = get_joints()
      draw_skeleton(screen, newpos_skeleton)
      pygame.display.update()
      pygame.display.flip()
    
   pygame.display.quit()
   pygame.quit()

   ctx.stop_generating_all()
   ctx.shutdown()


if __name__ == "__main__":
   main()
