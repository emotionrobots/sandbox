
// #include "CustomCloud.h"
#include "OpenNICapturer.h"

int main (int argc, char ** argv)
{
  std::string file_name;
  if (argc > 1)
  {
    file_name = argv[1];
  }
  else
  {
    file_name = "ICP";
  }

  OpenNICapturer capturer;
  capturer.setFileName(file_name);
  capturer.setInputCloudParameters (1.5, 0.005, 0.02, 20, 0.1, 0);
  capturer.setRegistrationParameters (5, 0.25, 3, 0.9, 0.005, 1000);
  capturer.run();
  
  return 0;
}