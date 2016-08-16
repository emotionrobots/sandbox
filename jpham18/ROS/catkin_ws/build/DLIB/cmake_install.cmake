# Install script for directory: /home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "/home/julian/sandbox/jpham18/ROS/catkin_ws/install")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Install shared libraries without execute permission?
if(NOT DEFINED CMAKE_INSTALL_SO_NO_EXE)
  set(CMAKE_INSTALL_SO_NO_EXE "1")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/DLIB/msg" TYPE FILE FILES
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg"
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg"
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg"
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg"
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg"
    )
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/DLIB/srv" TYPE FILE FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/DLIB/cmake" TYPE FILE FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/build/DLIB/catkin_generated/installspace/DLIB-msg-paths.cmake")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include" TYPE DIRECTORY FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/devel/include/DLIB")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/common-lisp/ros" TYPE DIRECTORY FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/devel/share/common-lisp/ros/DLIB")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  execute_process(COMMAND "/usr/bin/python" -m compileall "/home/julian/sandbox/jpham18/ROS/catkin_ws/devel/lib/python2.7/dist-packages/DLIB")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/python2.7/dist-packages" TYPE DIRECTORY FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/devel/lib/python2.7/dist-packages/DLIB")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/pkgconfig" TYPE FILE FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/build/DLIB/catkin_generated/installspace/DLIB.pc")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/DLIB/cmake" TYPE FILE FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/build/DLIB/catkin_generated/installspace/DLIB-msg-extras.cmake")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/DLIB/cmake" TYPE FILE FILES
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/build/DLIB/catkin_generated/installspace/DLIBConfig.cmake"
    "/home/julian/sandbox/jpham18/ROS/catkin_ws/build/DLIB/catkin_generated/installspace/DLIBConfig-version.cmake"
    )
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/DLIB" TYPE FILE FILES "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/package.xml")
endif()

if(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  include("/home/julian/sandbox/jpham18/ROS/catkin_ws/build/DLIB/dlib_build/cmake_install.cmake")

endif()

