
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
  capturer.setInputCloudParameters (1.5, 0.004, 0.02, 12, 0.1, 0);
  capturer.setRegistrationParameters (0.02, 0.01, 800, 0.05, 0.001*0.001);
  capturer.run();
  
  return 0;
}