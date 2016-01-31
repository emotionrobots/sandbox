# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "DLIB: 5 messages, 1 services")

set(MSG_I_FLAGS "-IDLIB:/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg;-Istd_msgs:/opt/ros/indigo/share/std_msgs/cmake/../msg;-Isensor_msgs:/opt/ros/indigo/share/sensor_msgs/cmake/../msg;-Igeometry_msgs:/opt/ros/indigo/share/geometry_msgs/cmake/../msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(genlisp REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(DLIB_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg" NAME_WE)
add_custom_target(_DLIB_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "DLIB" "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg" ""
)

get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg" NAME_WE)
add_custom_target(_DLIB_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "DLIB" "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg" "std_msgs/Header"
)

get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv" NAME_WE)
add_custom_target(_DLIB_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "DLIB" "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv" ""
)

get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg" NAME_WE)
add_custom_target(_DLIB_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "DLIB" "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg" "std_msgs/UInt32MultiArray:std_msgs/MultiArrayDimension:sensor_msgs/Image:std_msgs/Header:std_msgs/MultiArrayLayout"
)

get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg" NAME_WE)
add_custom_target(_DLIB_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "DLIB" "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg" ""
)

get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg" NAME_WE)
add_custom_target(_DLIB_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "DLIB" "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg" ""
)

#
#  langs = gencpp;genlisp;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
)
_generate_msg_cpp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/UInt32MultiArray.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayDimension.msg;/opt/ros/indigo/share/sensor_msgs/cmake/../msg/Image.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayLayout.msg"
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
)
_generate_msg_cpp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg"
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
)
_generate_msg_cpp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
)
_generate_msg_cpp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
)

### Generating Services
_generate_srv_cpp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
)

### Generating Module File
_generate_module_cpp(DLIB
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(DLIB_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(DLIB_generate_messages DLIB_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_cpp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_cpp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv" NAME_WE)
add_dependencies(DLIB_generate_messages_cpp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_cpp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_cpp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_cpp _DLIB_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(DLIB_gencpp)
add_dependencies(DLIB_gencpp DLIB_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS DLIB_generate_messages_cpp)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
)
_generate_msg_lisp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/UInt32MultiArray.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayDimension.msg;/opt/ros/indigo/share/sensor_msgs/cmake/../msg/Image.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayLayout.msg"
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
)
_generate_msg_lisp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg"
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
)
_generate_msg_lisp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
)
_generate_msg_lisp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
)

### Generating Services
_generate_srv_lisp(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
)

### Generating Module File
_generate_module_lisp(DLIB
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(DLIB_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(DLIB_generate_messages DLIB_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_lisp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_lisp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv" NAME_WE)
add_dependencies(DLIB_generate_messages_lisp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_lisp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_lisp _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_lisp _DLIB_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(DLIB_genlisp)
add_dependencies(DLIB_genlisp DLIB_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS DLIB_generate_messages_lisp)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
)
_generate_msg_py(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/UInt32MultiArray.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayDimension.msg;/opt/ros/indigo/share/sensor_msgs/cmake/../msg/Image.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayLayout.msg"
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
)
_generate_msg_py(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg"
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
)
_generate_msg_py(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
)
_generate_msg_py(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
)

### Generating Services
_generate_srv_py(DLIB
  "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
)

### Generating Module File
_generate_module_py(DLIB
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(DLIB_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(DLIB_generate_messages DLIB_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Face.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_py _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/CustomString.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_py _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/srv/festTTS.srv" NAME_WE)
add_dependencies(DLIB_generate_messages_py _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/face_p.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_py _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/Skeleton.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_py _DLIB_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/julian/sandbox/jpham18/ROS/catkin_ws/src/DLIB/msg/UnknownFace.msg" NAME_WE)
add_dependencies(DLIB_generate_messages_py _DLIB_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(DLIB_genpy)
add_dependencies(DLIB_genpy DLIB_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS DLIB_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/DLIB
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
add_dependencies(DLIB_generate_messages_cpp std_msgs_generate_messages_cpp)
add_dependencies(DLIB_generate_messages_cpp sensor_msgs_generate_messages_cpp)

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/DLIB
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
add_dependencies(DLIB_generate_messages_lisp std_msgs_generate_messages_lisp)
add_dependencies(DLIB_generate_messages_lisp sensor_msgs_generate_messages_lisp)

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB)
  install(CODE "execute_process(COMMAND \"/usr/bin/python\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/DLIB
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
add_dependencies(DLIB_generate_messages_py std_msgs_generate_messages_py)
add_dependencies(DLIB_generate_messages_py sensor_msgs_generate_messages_py)
