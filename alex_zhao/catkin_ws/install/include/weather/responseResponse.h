// Generated by gencpp from file weather/responseResponse.msg
// DO NOT EDIT!


#ifndef WEATHER_MESSAGE_RESPONSERESPONSE_H
#define WEATHER_MESSAGE_RESPONSERESPONSE_H


#include <string>
#include <vector>
#include <map>

#include <ros/types.h>
#include <ros/serialization.h>
#include <ros/builtin_message_traits.h>
#include <ros/message_operations.h>


namespace weather
{
template <class ContainerAllocator>
struct responseResponse_
{
  typedef responseResponse_<ContainerAllocator> Type;

  responseResponse_()
    : temperature(0.0)  {
    }
  responseResponse_(const ContainerAllocator& _alloc)
    : temperature(0.0)  {
  (void)_alloc;
    }



   typedef float _temperature_type;
  _temperature_type temperature;




  typedef boost::shared_ptr< ::weather::responseResponse_<ContainerAllocator> > Ptr;
  typedef boost::shared_ptr< ::weather::responseResponse_<ContainerAllocator> const> ConstPtr;

}; // struct responseResponse_

typedef ::weather::responseResponse_<std::allocator<void> > responseResponse;

typedef boost::shared_ptr< ::weather::responseResponse > responseResponsePtr;
typedef boost::shared_ptr< ::weather::responseResponse const> responseResponseConstPtr;

// constants requiring out of line definition



template<typename ContainerAllocator>
std::ostream& operator<<(std::ostream& s, const ::weather::responseResponse_<ContainerAllocator> & v)
{
ros::message_operations::Printer< ::weather::responseResponse_<ContainerAllocator> >::stream(s, "", v);
return s;
}

} // namespace weather

namespace ros
{
namespace message_traits
{



// BOOLTRAITS {'IsFixedSize': True, 'IsMessage': True, 'HasHeader': False}
// {'weather': ['/home/alex/catkin_ws/src/weather/msg', '/home/alex/catkin_ws/src/weather/msg'], 'std_msgs': ['/opt/ros/indigo/share/std_msgs/cmake/../msg']}

// !!!!!!!!!!! ['__class__', '__delattr__', '__dict__', '__doc__', '__eq__', '__format__', '__getattribute__', '__hash__', '__init__', '__module__', '__ne__', '__new__', '__reduce__', '__reduce_ex__', '__repr__', '__setattr__', '__sizeof__', '__str__', '__subclasshook__', '__weakref__', '_parsed_fields', 'constants', 'fields', 'full_name', 'has_header', 'header_present', 'names', 'package', 'parsed_fields', 'short_name', 'text', 'types']




template <class ContainerAllocator>
struct IsFixedSize< ::weather::responseResponse_<ContainerAllocator> >
  : TrueType
  { };

template <class ContainerAllocator>
struct IsFixedSize< ::weather::responseResponse_<ContainerAllocator> const>
  : TrueType
  { };

template <class ContainerAllocator>
struct IsMessage< ::weather::responseResponse_<ContainerAllocator> >
  : TrueType
  { };

template <class ContainerAllocator>
struct IsMessage< ::weather::responseResponse_<ContainerAllocator> const>
  : TrueType
  { };

template <class ContainerAllocator>
struct HasHeader< ::weather::responseResponse_<ContainerAllocator> >
  : FalseType
  { };

template <class ContainerAllocator>
struct HasHeader< ::weather::responseResponse_<ContainerAllocator> const>
  : FalseType
  { };


template<class ContainerAllocator>
struct MD5Sum< ::weather::responseResponse_<ContainerAllocator> >
{
  static const char* value()
  {
    return "694ab1a51fd38693b5cadd94c1ae252d";
  }

  static const char* value(const ::weather::responseResponse_<ContainerAllocator>&) { return value(); }
  static const uint64_t static_value1 = 0x694ab1a51fd38693ULL;
  static const uint64_t static_value2 = 0xb5cadd94c1ae252dULL;
};

template<class ContainerAllocator>
struct DataType< ::weather::responseResponse_<ContainerAllocator> >
{
  static const char* value()
  {
    return "weather/responseResponse";
  }

  static const char* value(const ::weather::responseResponse_<ContainerAllocator>&) { return value(); }
};

template<class ContainerAllocator>
struct Definition< ::weather::responseResponse_<ContainerAllocator> >
{
  static const char* value()
  {
    return "float32 temperature\n\
\n\
";
  }

  static const char* value(const ::weather::responseResponse_<ContainerAllocator>&) { return value(); }
};

} // namespace message_traits
} // namespace ros

namespace ros
{
namespace serialization
{

  template<class ContainerAllocator> struct Serializer< ::weather::responseResponse_<ContainerAllocator> >
  {
    template<typename Stream, typename T> inline static void allInOne(Stream& stream, T m)
    {
      stream.next(m.temperature);
    }

    ROS_DECLARE_ALLINONE_SERIALIZER
  }; // struct responseResponse_

} // namespace serialization
} // namespace ros

namespace ros
{
namespace message_operations
{

template<class ContainerAllocator>
struct Printer< ::weather::responseResponse_<ContainerAllocator> >
{
  template<typename Stream> static void stream(Stream& s, const std::string& indent, const ::weather::responseResponse_<ContainerAllocator>& v)
  {
    s << indent << "temperature: ";
    Printer<float>::stream(s, indent + "  ", v.temperature);
  }
};

} // namespace message_operations
} // namespace ros

#endif // WEATHER_MESSAGE_RESPONSERESPONSE_H
