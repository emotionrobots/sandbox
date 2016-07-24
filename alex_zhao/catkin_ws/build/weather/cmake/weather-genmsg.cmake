# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "weather: 1 messages, 1 services")

set(MSG_I_FLAGS "-Iweather:/home/alex/catkin_ws/src/weather/msg;-Istd_msgs:/opt/ros/indigo/share/std_msgs/cmake/../msg;-Iweather:/home/alex/catkin_ws/src/weather/msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(genlisp REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(weather_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/home/alex/catkin_ws/src/weather/msg/city.msg" NAME_WE)
add_custom_target(_weather_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "weather" "/home/alex/catkin_ws/src/weather/msg/city.msg" ""
)

get_filename_component(_filename "/home/alex/catkin_ws/src/weather/srv/response.srv" NAME_WE)
add_custom_target(_weather_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "weather" "/home/alex/catkin_ws/src/weather/srv/response.srv" ""
)

#
#  langs = gencpp;genlisp;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(weather
  "/home/alex/catkin_ws/src/weather/msg/city.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/weather
)

### Generating Services
_generate_srv_cpp(weather
  "/home/alex/catkin_ws/src/weather/srv/response.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/weather
)

### Generating Module File
_generate_module_cpp(weather
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/weather
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(weather_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(weather_generate_messages weather_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/alex/catkin_ws/src/weather/msg/city.msg" NAME_WE)
add_dependencies(weather_generate_messages_cpp _weather_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/alex/catkin_ws/src/weather/srv/response.srv" NAME_WE)
add_dependencies(weather_generate_messages_cpp _weather_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(weather_gencpp)
add_dependencies(weather_gencpp weather_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS weather_generate_messages_cpp)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(weather
  "/home/alex/catkin_ws/src/weather/msg/city.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/weather
)

### Generating Services
_generate_srv_lisp(weather
  "/home/alex/catkin_ws/src/weather/srv/response.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/weather
)

### Generating Module File
_generate_module_lisp(weather
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/weather
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(weather_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(weather_generate_messages weather_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/alex/catkin_ws/src/weather/msg/city.msg" NAME_WE)
add_dependencies(weather_generate_messages_lisp _weather_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/alex/catkin_ws/src/weather/srv/response.srv" NAME_WE)
add_dependencies(weather_generate_messages_lisp _weather_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(weather_genlisp)
add_dependencies(weather_genlisp weather_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS weather_generate_messages_lisp)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(weather
  "/home/alex/catkin_ws/src/weather/msg/city.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/weather
)

### Generating Services
_generate_srv_py(weather
  "/home/alex/catkin_ws/src/weather/srv/response.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/weather
)

### Generating Module File
_generate_module_py(weather
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/weather
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(weather_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(weather_generate_messages weather_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/alex/catkin_ws/src/weather/msg/city.msg" NAME_WE)
add_dependencies(weather_generate_messages_py _weather_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/alex/catkin_ws/src/weather/srv/response.srv" NAME_WE)
add_dependencies(weather_generate_messages_py _weather_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(weather_genpy)
add_dependencies(weather_genpy weather_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS weather_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/weather)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/weather
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
add_dependencies(weather_generate_messages_cpp std_msgs_generate_messages_cpp)
add_dependencies(weather_generate_messages_cpp weather_generate_messages_cpp)

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/weather)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/weather
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
add_dependencies(weather_generate_messages_lisp std_msgs_generate_messages_lisp)
add_dependencies(weather_generate_messages_lisp weather_generate_messages_lisp)

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/weather)
  install(CODE "execute_process(COMMAND \"/usr/bin/python\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/weather\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/weather
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
add_dependencies(weather_generate_messages_py std_msgs_generate_messages_py)
add_dependencies(weather_generate_messages_py weather_generate_messages_py)
