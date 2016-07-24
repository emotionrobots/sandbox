# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "music: 1 messages, 1 services")

set(MSG_I_FLAGS "-Imusic:/home/alex/catkin_ws/src/music/msg;-Istd_msgs:/opt/ros/indigo/share/std_msgs/cmake/../msg;-Imusic:/home/alex/catkin_ws/src/music/msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(genlisp REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(music_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/home/alex/catkin_ws/src/music/srv/musicgenre.srv" NAME_WE)
add_custom_target(_music_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "music" "/home/alex/catkin_ws/src/music/srv/musicgenre.srv" ""
)

get_filename_component(_filename "/home/alex/catkin_ws/src/music/msg/Genre.msg" NAME_WE)
add_custom_target(_music_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "music" "/home/alex/catkin_ws/src/music/msg/Genre.msg" ""
)

#
#  langs = gencpp;genlisp;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(music
  "/home/alex/catkin_ws/src/music/msg/Genre.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/music
)

### Generating Services
_generate_srv_cpp(music
  "/home/alex/catkin_ws/src/music/srv/musicgenre.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/music
)

### Generating Module File
_generate_module_cpp(music
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/music
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(music_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(music_generate_messages music_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/alex/catkin_ws/src/music/srv/musicgenre.srv" NAME_WE)
add_dependencies(music_generate_messages_cpp _music_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/alex/catkin_ws/src/music/msg/Genre.msg" NAME_WE)
add_dependencies(music_generate_messages_cpp _music_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(music_gencpp)
add_dependencies(music_gencpp music_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS music_generate_messages_cpp)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(music
  "/home/alex/catkin_ws/src/music/msg/Genre.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/music
)

### Generating Services
_generate_srv_lisp(music
  "/home/alex/catkin_ws/src/music/srv/musicgenre.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/music
)

### Generating Module File
_generate_module_lisp(music
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/music
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(music_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(music_generate_messages music_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/alex/catkin_ws/src/music/srv/musicgenre.srv" NAME_WE)
add_dependencies(music_generate_messages_lisp _music_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/alex/catkin_ws/src/music/msg/Genre.msg" NAME_WE)
add_dependencies(music_generate_messages_lisp _music_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(music_genlisp)
add_dependencies(music_genlisp music_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS music_generate_messages_lisp)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(music
  "/home/alex/catkin_ws/src/music/msg/Genre.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/music
)

### Generating Services
_generate_srv_py(music
  "/home/alex/catkin_ws/src/music/srv/musicgenre.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/music
)

### Generating Module File
_generate_module_py(music
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/music
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(music_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(music_generate_messages music_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/alex/catkin_ws/src/music/srv/musicgenre.srv" NAME_WE)
add_dependencies(music_generate_messages_py _music_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/alex/catkin_ws/src/music/msg/Genre.msg" NAME_WE)
add_dependencies(music_generate_messages_py _music_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(music_genpy)
add_dependencies(music_genpy music_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS music_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/music)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/music
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
add_dependencies(music_generate_messages_cpp std_msgs_generate_messages_cpp)
add_dependencies(music_generate_messages_cpp music_generate_messages_cpp)

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/music)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/music
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
add_dependencies(music_generate_messages_lisp std_msgs_generate_messages_lisp)
add_dependencies(music_generate_messages_lisp music_generate_messages_lisp)

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/music)
  install(CODE "execute_process(COMMAND \"/usr/bin/python\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/music\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/music
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
add_dependencies(music_generate_messages_py std_msgs_generate_messages_py)
add_dependencies(music_generate_messages_py music_generate_messages_py)
