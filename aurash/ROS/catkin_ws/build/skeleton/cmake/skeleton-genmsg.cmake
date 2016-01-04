# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "skeleton: 4 messages, 1 services")

set(MSG_I_FLAGS "-Iskeleton:/home/aurash/catkin_ws/src/skeleton/msg;-Istd_msgs:/opt/ros/indigo/share/std_msgs/cmake/../msg;-Isensor_msgs:/opt/ros/indigo/share/sensor_msgs/cmake/../msg;-Igeometry_msgs:/opt/ros/indigo/share/geometry_msgs/cmake/../msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(genlisp REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(skeleton_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv" NAME_WE)
add_custom_target(_skeleton_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "skeleton" "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv" ""
)

get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg" NAME_WE)
add_custom_target(_skeleton_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "skeleton" "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg" "std_msgs/UInt32MultiArray:std_msgs/MultiArrayDimension:sensor_msgs/Image:std_msgs/Header:std_msgs/MultiArrayLayout"
)

get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg" NAME_WE)
add_custom_target(_skeleton_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "skeleton" "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg" ""
)

get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg" NAME_WE)
add_custom_target(_skeleton_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "skeleton" "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg" ""
)

get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg" NAME_WE)
add_custom_target(_skeleton_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "skeleton" "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg" ""
)

#
#  langs = gencpp;genlisp;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
)
_generate_msg_cpp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
)
_generate_msg_cpp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
)
_generate_msg_cpp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/UInt32MultiArray.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayDimension.msg;/opt/ros/indigo/share/sensor_msgs/cmake/../msg/Image.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayLayout.msg"
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
)

### Generating Services
_generate_srv_cpp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
)

### Generating Module File
_generate_module_cpp(skeleton
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(skeleton_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(skeleton_generate_messages skeleton_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv" NAME_WE)
add_dependencies(skeleton_generate_messages_cpp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_cpp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_cpp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_cpp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_cpp _skeleton_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(skeleton_gencpp)
add_dependencies(skeleton_gencpp skeleton_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS skeleton_generate_messages_cpp)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
)
_generate_msg_lisp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
)
_generate_msg_lisp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
)
_generate_msg_lisp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/UInt32MultiArray.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayDimension.msg;/opt/ros/indigo/share/sensor_msgs/cmake/../msg/Image.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayLayout.msg"
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
)

### Generating Services
_generate_srv_lisp(skeleton
  "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
)

### Generating Module File
_generate_module_lisp(skeleton
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(skeleton_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(skeleton_generate_messages skeleton_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv" NAME_WE)
add_dependencies(skeleton_generate_messages_lisp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_lisp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_lisp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_lisp _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_lisp _skeleton_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(skeleton_genlisp)
add_dependencies(skeleton_genlisp skeleton_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS skeleton_generate_messages_lisp)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
)
_generate_msg_py(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
)
_generate_msg_py(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
)
_generate_msg_py(skeleton
  "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg"
  "${MSG_I_FLAGS}"
  "/opt/ros/indigo/share/std_msgs/cmake/../msg/UInt32MultiArray.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayDimension.msg;/opt/ros/indigo/share/sensor_msgs/cmake/../msg/Image.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/Header.msg;/opt/ros/indigo/share/std_msgs/cmake/../msg/MultiArrayLayout.msg"
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
)

### Generating Services
_generate_srv_py(skeleton
  "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
)

### Generating Module File
_generate_module_py(skeleton
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(skeleton_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(skeleton_generate_messages skeleton_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/srv/festTTS.srv" NAME_WE)
add_dependencies(skeleton_generate_messages_py _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/face_p.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_py _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Skeleton.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_py _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/Face.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_py _skeleton_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/aurash/catkin_ws/src/skeleton/msg/UnknownFace.msg" NAME_WE)
add_dependencies(skeleton_generate_messages_py _skeleton_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(skeleton_genpy)
add_dependencies(skeleton_genpy skeleton_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS skeleton_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/skeleton
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
add_dependencies(skeleton_generate_messages_cpp std_msgs_generate_messages_cpp)
add_dependencies(skeleton_generate_messages_cpp sensor_msgs_generate_messages_cpp)

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/skeleton
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
add_dependencies(skeleton_generate_messages_lisp std_msgs_generate_messages_lisp)
add_dependencies(skeleton_generate_messages_lisp sensor_msgs_generate_messages_lisp)

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton)
  install(CODE "execute_process(COMMAND \"/usr/bin/python\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/skeleton
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
add_dependencies(skeleton_generate_messages_py std_msgs_generate_messages_py)
add_dependencies(skeleton_generate_messages_py sensor_msgs_generate_messages_py)
