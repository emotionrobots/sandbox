// Generated by gencpp from file music/musicgenre.msg
// DO NOT EDIT!


#ifndef MUSIC_MESSAGE_MUSICGENRE_H
#define MUSIC_MESSAGE_MUSICGENRE_H

#include <ros/service_traits.h>


#include <music/musicgenreRequest.h>
#include <music/musicgenreResponse.h>


namespace music
{

struct musicgenre
{

typedef musicgenreRequest Request;
typedef musicgenreResponse Response;
Request request;
Response response;

typedef Request RequestType;
typedef Response ResponseType;

}; // struct musicgenre
} // namespace music


namespace ros
{
namespace service_traits
{


template<>
struct MD5Sum< ::music::musicgenre > {
  static const char* value()
  {
    return "24f79890f3c7bcd70765f9b2c0864a5a";
  }

  static const char* value(const ::music::musicgenre&) { return value(); }
};

template<>
struct DataType< ::music::musicgenre > {
  static const char* value()
  {
    return "music/musicgenre";
  }

  static const char* value(const ::music::musicgenre&) { return value(); }
};


// service_traits::MD5Sum< ::music::musicgenreRequest> should match 
// service_traits::MD5Sum< ::music::musicgenre > 
template<>
struct MD5Sum< ::music::musicgenreRequest>
{
  static const char* value()
  {
    return MD5Sum< ::music::musicgenre >::value();
  }
  static const char* value(const ::music::musicgenreRequest&)
  {
    return value();
  }
};

// service_traits::DataType< ::music::musicgenreRequest> should match 
// service_traits::DataType< ::music::musicgenre > 
template<>
struct DataType< ::music::musicgenreRequest>
{
  static const char* value()
  {
    return DataType< ::music::musicgenre >::value();
  }
  static const char* value(const ::music::musicgenreRequest&)
  {
    return value();
  }
};

// service_traits::MD5Sum< ::music::musicgenreResponse> should match 
// service_traits::MD5Sum< ::music::musicgenre > 
template<>
struct MD5Sum< ::music::musicgenreResponse>
{
  static const char* value()
  {
    return MD5Sum< ::music::musicgenre >::value();
  }
  static const char* value(const ::music::musicgenreResponse&)
  {
    return value();
  }
};

// service_traits::DataType< ::music::musicgenreResponse> should match 
// service_traits::DataType< ::music::musicgenre > 
template<>
struct DataType< ::music::musicgenreResponse>
{
  static const char* value()
  {
    return DataType< ::music::musicgenre >::value();
  }
  static const char* value(const ::music::musicgenreResponse&)
  {
    return value();
  }
};

} // namespace service_traits
} // namespace ros

#endif // MUSIC_MESSAGE_MUSICGENRE_H
