# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 2.8

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list

# Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/bin/cmake

# The command to remove a file.
RM = /usr/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The program to use to edit the cache.
CMAKE_EDIT_COMMAND = /usr/bin/cmake-gui

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/emotion/emotion/sandbox/larry/rp2w_serial

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/emotion/emotion/sandbox/larry/rp2w_serial/build

# Include any dependencies generated for this target.
include CMakeFiles/testEncoder.dir/depend.make

# Include the progress variables for this target.
include CMakeFiles/testEncoder.dir/progress.make

# Include the compile flags for this target's objects.
include CMakeFiles/testEncoder.dir/flags.make

CMakeFiles/testEncoder.dir/testEncoder.cpp.o: CMakeFiles/testEncoder.dir/flags.make
CMakeFiles/testEncoder.dir/testEncoder.cpp.o: ../testEncoder.cpp
	$(CMAKE_COMMAND) -E cmake_progress_report /home/emotion/emotion/sandbox/larry/rp2w_serial/build/CMakeFiles $(CMAKE_PROGRESS_1)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Building CXX object CMakeFiles/testEncoder.dir/testEncoder.cpp.o"
	/usr/bin/c++   $(CXX_DEFINES) $(CXX_FLAGS) -o CMakeFiles/testEncoder.dir/testEncoder.cpp.o -c /home/emotion/emotion/sandbox/larry/rp2w_serial/testEncoder.cpp

CMakeFiles/testEncoder.dir/testEncoder.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/testEncoder.dir/testEncoder.cpp.i"
	/usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -E /home/emotion/emotion/sandbox/larry/rp2w_serial/testEncoder.cpp > CMakeFiles/testEncoder.dir/testEncoder.cpp.i

CMakeFiles/testEncoder.dir/testEncoder.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/testEncoder.dir/testEncoder.cpp.s"
	/usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -S /home/emotion/emotion/sandbox/larry/rp2w_serial/testEncoder.cpp -o CMakeFiles/testEncoder.dir/testEncoder.cpp.s

CMakeFiles/testEncoder.dir/testEncoder.cpp.o.requires:
.PHONY : CMakeFiles/testEncoder.dir/testEncoder.cpp.o.requires

CMakeFiles/testEncoder.dir/testEncoder.cpp.o.provides: CMakeFiles/testEncoder.dir/testEncoder.cpp.o.requires
	$(MAKE) -f CMakeFiles/testEncoder.dir/build.make CMakeFiles/testEncoder.dir/testEncoder.cpp.o.provides.build
.PHONY : CMakeFiles/testEncoder.dir/testEncoder.cpp.o.provides

CMakeFiles/testEncoder.dir/testEncoder.cpp.o.provides.build: CMakeFiles/testEncoder.dir/testEncoder.cpp.o

CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o: CMakeFiles/testEncoder.dir/flags.make
CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o: ../rp2w_serial.cpp
	$(CMAKE_COMMAND) -E cmake_progress_report /home/emotion/emotion/sandbox/larry/rp2w_serial/build/CMakeFiles $(CMAKE_PROGRESS_2)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Building CXX object CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o"
	/usr/bin/c++   $(CXX_DEFINES) $(CXX_FLAGS) -o CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o -c /home/emotion/emotion/sandbox/larry/rp2w_serial/rp2w_serial.cpp

CMakeFiles/testEncoder.dir/rp2w_serial.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/testEncoder.dir/rp2w_serial.cpp.i"
	/usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -E /home/emotion/emotion/sandbox/larry/rp2w_serial/rp2w_serial.cpp > CMakeFiles/testEncoder.dir/rp2w_serial.cpp.i

CMakeFiles/testEncoder.dir/rp2w_serial.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/testEncoder.dir/rp2w_serial.cpp.s"
	/usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -S /home/emotion/emotion/sandbox/larry/rp2w_serial/rp2w_serial.cpp -o CMakeFiles/testEncoder.dir/rp2w_serial.cpp.s

CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.requires:
.PHONY : CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.requires

CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.provides: CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.requires
	$(MAKE) -f CMakeFiles/testEncoder.dir/build.make CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.provides.build
.PHONY : CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.provides

CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.provides.build: CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o

# Object files for target testEncoder
testEncoder_OBJECTS = \
"CMakeFiles/testEncoder.dir/testEncoder.cpp.o" \
"CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o"

# External object files for target testEncoder
testEncoder_EXTERNAL_OBJECTS =

testEncoder: CMakeFiles/testEncoder.dir/testEncoder.cpp.o
testEncoder: CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o
testEncoder: CMakeFiles/testEncoder.dir/build.make
testEncoder: CMakeFiles/testEncoder.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --red --bold "Linking CXX executable testEncoder"
	$(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/testEncoder.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
CMakeFiles/testEncoder.dir/build: testEncoder
.PHONY : CMakeFiles/testEncoder.dir/build

CMakeFiles/testEncoder.dir/requires: CMakeFiles/testEncoder.dir/testEncoder.cpp.o.requires
CMakeFiles/testEncoder.dir/requires: CMakeFiles/testEncoder.dir/rp2w_serial.cpp.o.requires
.PHONY : CMakeFiles/testEncoder.dir/requires

CMakeFiles/testEncoder.dir/clean:
	$(CMAKE_COMMAND) -P CMakeFiles/testEncoder.dir/cmake_clean.cmake
.PHONY : CMakeFiles/testEncoder.dir/clean

CMakeFiles/testEncoder.dir/depend:
	cd /home/emotion/emotion/sandbox/larry/rp2w_serial/build && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/emotion/emotion/sandbox/larry/rp2w_serial /home/emotion/emotion/sandbox/larry/rp2w_serial /home/emotion/emotion/sandbox/larry/rp2w_serial/build /home/emotion/emotion/sandbox/larry/rp2w_serial/build /home/emotion/emotion/sandbox/larry/rp2w_serial/build/CMakeFiles/testEncoder.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : CMakeFiles/testEncoder.dir/depend

