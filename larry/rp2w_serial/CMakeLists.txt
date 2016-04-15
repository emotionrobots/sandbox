cmake_minimum_required(VERSION 2.6)
project (testapp)

# Set version number
set (testapp_VERSION_MAJOR 1)
set (testapp_VERSION_MINOR 0)

# Configure a header file to pass some of the cmake settings
# to the source code
configure_file (
   "${PROJECT_SOURCE_DIR}/testappConfig.h.in"
   "${PROJECT_SOURCE_DIR}/testappConfig.h"
   )

# Add the binary tree to the search path for include file
# so that we will find testappConfig.h
include_directories("${PROJECT_BINARY_DIR}")

# Add the executable
add_executable(testapp testapp.cpp joystick.cpp rp2w_serial.cpp)
add_executable(testjoystick testjoystick.cpp joystick.cpp)
target_link_libraries(testapp serial)
target_link_libraries(testjoystick serial)

add_executable(testEncoder testEncoder.cpp rp2w_serial.cpp)
target_link_libraries(testEncoder serial)
