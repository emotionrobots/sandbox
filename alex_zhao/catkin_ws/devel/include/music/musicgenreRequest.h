// Generated by gencpp from file music/musicgenreRequest.msg
// DO NOT EDIT!


#ifndef MUSIC_MESSAGE_MUSICGENREREQUEST_H
#define MUSIC_MESSAGE_MUSICGENREREQUEST_H


#include <string>
#include <vector>
#include <map>

#include <ros/types.h>
#include <ros/serialization.h>
#include <ros/builtin_message_traits.h>
#include <ros/message_operations.h>


namespace music
{
template <class ContainerAllocator>
struct musicgenreRequest_
{
  typedef musicgenreRequest_<ContainerAllocator> Type;

  musicgenreRequest_()
    : genre()  {
    }
  musicgenreRequest_(const ContainerAllocator& _alloc)
    : genre(_alloc)  {
  (void)_alloc;
    }



   typedef std::basic_string<char, std::char_traits<char>, typename ContainerAllocator::template rebind<char>::other >  _genre_type;
  _genre_type genre;




  typedef boost::shared_ptr< ::music::musicgenreRequest_<ContainerAllocator> > Ptr;
  typedef boost::shared_ptr< ::music::musicgenreRequest_<ContainerAllocator> const> ConstPtr;

}; // struct musicgenreRequest_

typedef ::music::musicgenreRequest_<std::allocator<void> > musicgenreRequest;

typedef boost::shared_ptr< ::music::musicgenreRequest > musicgenreRequestPtr;
typedef boost::shared_ptr< ::music::musicgenreRequest const> musicgenreRequestConstPtr;

// constants requiring out of line definition



template<typename ContainerAllocator>
std::ostream& operator<<(std::ostream& s, const ::music::musicgenreRequest_<ContainerAllocator> & v)
{
ros::message_operations::Printer< ::music::musicgenreRequest_<ContainerAllocator> >::stream(s, "", v);
return s;
}

} // namespace music

namespace ros
{
namespace message_traits
{



// BOOLTRAITS {'IsFixedSize': False, 'IsMessage': True, 'HasHeader': False}
// {'std_msgs': ['/opt/ros/indigo/share/std_msgs/cmake/../msg'], 'music': ['/home/alex/catkin_ws/src/music/msg', '/home/alex/catkin_ws/src/music/msg']}

// !!!!!!!!!!! ['__class__', '__delattr__', '__dict__', '__doc__', '__eq__', '__format__', '__getattribute__', '__hash__', '__init__', '__module__', '__ne__', '__new__', '__reduce__', '__reduce_ex__', '__repr__', '__setattr__', '__sizeof__', '__str__', '__subclasshook__', '__weakref__', '_parsed_fields', 'constants', 'fields', 'full_name', 'has_header', 'header_present', 'names', 'package', 'parsed_fields', 'short_name', 'text', 'types']




template <class ContainerAllocator>
struct IsFixedSize< ::music::musicgenreRequest_<ContainerAllocator> >
  : FalseType
  { };

template <class ContainerAllocator>
struct IsFixedSize< ::music::musicgenreRequest_<ContainerAllocator> const>
  : FalseType
  { };

template <class ContainerAllocator>
struct IsMessage< ::music::musicgenreRequest_<ContainerAllocator> >
  : TrueType
  { };

template <class ContainerAllocator>
struct IsMessage< ::music::musicgenreRequest_<ContainerAllocator> const>
  : TrueType
  { };

template <class ContainerAllocator>
struct HasHeader< ::music::musicgenreRequest_<ContainerAllocator> >
  : FalseType
  { };

template <class ContainerAllocator>
struct HasHeader< ::music::musicgenreRequest_<ContainerAllocator> const>
  : FalseType
  { };


template<class ContainerAllocator>
struct MD5Sum< ::music::musicgenreRequest_<ContainerAllocator> >
{
  static const char* value()
  {
    return "9ccf1c8ab8cb09eedf68bf876568f66e";
  }

  static const char* value(const ::music::musicgenreRequest_<ContainerAllocator>&) { return value(); }
  static const uint64_t static_value1 = 0x9ccf1c8ab8cb09eeULL;
  static const uint64_t static_value2 = 0xdf68bf876568f66eULL;
};

template<class ContainerAllocator>
struct DataType< ::music::musicgenreRequest_<ContainerAllocator> >
{
  static const char* value()
  {
    return "music/musicgenreRequest";
  }

  static const char* value(const ::music::musicgenreRequest_<ContainerAllocator>&) { return value(); }
};

template<class ContainerAllocator>
struct Definition< ::music::musicgenreRequest_<ContainerAllocator> >
{
  static const char* value()
  {
    return "string genre\n\
";
  }

  static const char* value(const ::music::musicgenreRequest_<ContainerAllocator>&) { return value(); }
};

} // namespace message_traits
} // namespace ros

namespace ros
{
namespace serialization
{

  template<class ContainerAllocator> struct Serializer< ::music::musicgenreRequest_<ContainerAllocator> >
  {
    template<typename Stream, typename T> inline static void allInOne(Stream& stream, T m)
    {
      stream.next(m.genre);
    }

    ROS_DECLARE_ALLINONE_SERIALIZER;
  }; // struct musicgenreRequest_

} // namespace serialization
} // namespace ros

namespace ros
{
namespace message_operations
{

template<class ContainerAllocator>
struct Printer< ::music::musicgenreRequest_<ContainerAllocator> >
{
  template<typename Stream> static void stream(Stream& s, const std::string& indent, const ::music::musicgenreRequest_<ContainerAllocator>& v)
  {
    s << indent << "genre: ";
    Printer<std::basic_string<char, std::char_traits<char>, typename ContainerAllocator::template rebind<char>::other > >::stream(s, indent + "  ", v.genre);
  }
};

} // namespace message_operations
} // namespace ros

#endif // MUSIC_MESSAGE_MUSICGENREREQUEST_H
