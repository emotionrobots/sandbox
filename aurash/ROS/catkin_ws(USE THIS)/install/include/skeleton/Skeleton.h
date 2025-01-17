// Generated by gencpp from file skeleton/Skeleton.msg
// DO NOT EDIT!


#ifndef SKELETON_MESSAGE_SKELETON_H
#define SKELETON_MESSAGE_SKELETON_H


#include <string>
#include <vector>
#include <map>

#include <ros/types.h>
#include <ros/serialization.h>
#include <ros/builtin_message_traits.h>
#include <ros/message_operations.h>


namespace skeleton
{
template <class ContainerAllocator>
struct Skeleton_
{
  typedef Skeleton_<ContainerAllocator> Type;

  Skeleton_()
    : id(0)
    , data()  {
    }
  Skeleton_(const ContainerAllocator& _alloc)
    : id(0)
    , data(_alloc)  {
    }



   typedef int8_t _id_type;
  _id_type id;

   typedef std::basic_string<char, std::char_traits<char>, typename ContainerAllocator::template rebind<char>::other >  _data_type;
  _data_type data;




  typedef boost::shared_ptr< ::skeleton::Skeleton_<ContainerAllocator> > Ptr;
  typedef boost::shared_ptr< ::skeleton::Skeleton_<ContainerAllocator> const> ConstPtr;

}; // struct Skeleton_

typedef ::skeleton::Skeleton_<std::allocator<void> > Skeleton;

typedef boost::shared_ptr< ::skeleton::Skeleton > SkeletonPtr;
typedef boost::shared_ptr< ::skeleton::Skeleton const> SkeletonConstPtr;

// constants requiring out of line definition



template<typename ContainerAllocator>
std::ostream& operator<<(std::ostream& s, const ::skeleton::Skeleton_<ContainerAllocator> & v)
{
ros::message_operations::Printer< ::skeleton::Skeleton_<ContainerAllocator> >::stream(s, "", v);
return s;
}

} // namespace skeleton

namespace ros
{
namespace message_traits
{



// BOOLTRAITS {'IsFixedSize': False, 'IsMessage': True, 'HasHeader': False}
// {'sensor_msgs': ['/opt/ros/indigo/share/sensor_msgs/cmake/../msg'], 'geometry_msgs': ['/opt/ros/indigo/share/geometry_msgs/cmake/../msg'], 'std_msgs': ['/opt/ros/indigo/share/std_msgs/cmake/../msg'], 'skeleton': ['/home/aurash/catkin_ws/src/skeleton/msg']}

// !!!!!!!!!!! ['__class__', '__delattr__', '__dict__', '__doc__', '__eq__', '__format__', '__getattribute__', '__hash__', '__init__', '__module__', '__ne__', '__new__', '__reduce__', '__reduce_ex__', '__repr__', '__setattr__', '__sizeof__', '__str__', '__subclasshook__', '__weakref__', '_parsed_fields', 'constants', 'fields', 'full_name', 'has_header', 'header_present', 'names', 'package', 'parsed_fields', 'short_name', 'text', 'types']




template <class ContainerAllocator>
struct IsFixedSize< ::skeleton::Skeleton_<ContainerAllocator> >
  : FalseType
  { };

template <class ContainerAllocator>
struct IsFixedSize< ::skeleton::Skeleton_<ContainerAllocator> const>
  : FalseType
  { };

template <class ContainerAllocator>
struct IsMessage< ::skeleton::Skeleton_<ContainerAllocator> >
  : TrueType
  { };

template <class ContainerAllocator>
struct IsMessage< ::skeleton::Skeleton_<ContainerAllocator> const>
  : TrueType
  { };

template <class ContainerAllocator>
struct HasHeader< ::skeleton::Skeleton_<ContainerAllocator> >
  : FalseType
  { };

template <class ContainerAllocator>
struct HasHeader< ::skeleton::Skeleton_<ContainerAllocator> const>
  : FalseType
  { };


template<class ContainerAllocator>
struct MD5Sum< ::skeleton::Skeleton_<ContainerAllocator> >
{
  static const char* value()
  {
    return "3398c77d30999b784ef30c252c7c0e98";
  }

  static const char* value(const ::skeleton::Skeleton_<ContainerAllocator>&) { return value(); }
  static const uint64_t static_value1 = 0x3398c77d30999b78ULL;
  static const uint64_t static_value2 = 0x4ef30c252c7c0e98ULL;
};

template<class ContainerAllocator>
struct DataType< ::skeleton::Skeleton_<ContainerAllocator> >
{
  static const char* value()
  {
    return "skeleton/Skeleton";
  }

  static const char* value(const ::skeleton::Skeleton_<ContainerAllocator>&) { return value(); }
};

template<class ContainerAllocator>
struct Definition< ::skeleton::Skeleton_<ContainerAllocator> >
{
  static const char* value()
  {
    return "int8 id\n\
string data\n\
";
  }

  static const char* value(const ::skeleton::Skeleton_<ContainerAllocator>&) { return value(); }
};

} // namespace message_traits
} // namespace ros

namespace ros
{
namespace serialization
{

  template<class ContainerAllocator> struct Serializer< ::skeleton::Skeleton_<ContainerAllocator> >
  {
    template<typename Stream, typename T> inline static void allInOne(Stream& stream, T m)
    {
      stream.next(m.id);
      stream.next(m.data);
    }

    ROS_DECLARE_ALLINONE_SERIALIZER;
  }; // struct Skeleton_

} // namespace serialization
} // namespace ros

namespace ros
{
namespace message_operations
{

template<class ContainerAllocator>
struct Printer< ::skeleton::Skeleton_<ContainerAllocator> >
{
  template<typename Stream> static void stream(Stream& s, const std::string& indent, const ::skeleton::Skeleton_<ContainerAllocator>& v)
  {
    s << indent << "id: ";
    Printer<int8_t>::stream(s, indent + "  ", v.id);
    s << indent << "data: ";
    Printer<std::basic_string<char, std::char_traits<char>, typename ContainerAllocator::template rebind<char>::other > >::stream(s, indent + "  ", v.data);
  }
};

} // namespace message_operations
} // namespace ros

#endif // SKELETON_MESSAGE_SKELETON_H
