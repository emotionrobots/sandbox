cmake_minimum_required(VERSION 2.8 FATAL_ERROR)

project(iterative_closest_point_capture)

find_package(PCL 1.7 REQUIRED)

include_directories(${PCL_INCLUDE_DIRS})
link_directories(${PCL_LIBRARY_DIRS})
add_definitions(${PCL_DEFINITIONS})

set(PCL_BUILD_TYPE Release)

add_executable (capture capture.cpp)
target_link_libraries (capture ${PCL_LIBRARIES})
