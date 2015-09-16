cmake_minimum_required(VERSION 2.8 FATAL_ERROR)

project(ICP_Capture)

find_package(PCL 1.6 REQUIRED)

include_directories(${PCL_INCLUDE_DIRS})
link_directories(${PCL_LIBRARY_DIRS})
add_definitions(${PCL_DEFINITIONS})

set(SRC_FILES "capture.cpp" "OpenNICapturer.cpp" "OpenNICapturer.h" "CustomCloud.cpp" "CustomCloud.h" "Registration.cpp" "Registration.h" "PCDWriter.cpp" "PCDWriter.h")
add_executable (capture ${SRC_FILES})
target_link_libraries (capture ${PCL_LIBRARIES})
