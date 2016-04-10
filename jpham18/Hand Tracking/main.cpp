#include "Jive.h"
#include "ros/ros.h"
#include "std_msgs/String.h"

int getkey() {
    int character;
    struct termios orig_term_attr;
    struct termios new_term_attr;

    /* set the terminal to raw mode */
    tcgetattr(fileno(stdin), &orig_term_attr);
    memcpy(&new_term_attr, &orig_term_attr, sizeof(struct termios));
    new_term_attr.c_lflag &= ~(ECHO|ICANON);
    new_term_attr.c_cc[VTIME] = 0;
    new_term_attr.c_cc[VMIN] = 0;
    tcsetattr(fileno(stdin), TCSANOW, &new_term_attr);

    /* read a character from the stdin stream without blocking */
    /*   returns EOF (-1) if no character is available */
    character = fgetc(stdin);

    /* restore the original terminal attributes */
    tcsetattr(fileno(stdin), TCSANOW, &orig_term_attr);

    return character;
}


void callback(const std_msgs::String::ConstPtr& msg)
{
  ROS_INFO("I heard: [%s]", msg->data.c_str());
}

int main(int argc, char *argv[])
{
  int key;
  bool done = false;
  Mat bImg;
  Jive jive(80, 60);
  ros::Subscriber sub = n.subscribe("depth", 1000, callback);
  jive.start();
  namedWindow( "Binary", WINDOW_NORMAL );
  while (!done) {
     if (getkey() == 'q') 
        done = true;
     usleep(100000);
  }

err_exit:
   jive.stop();
}
