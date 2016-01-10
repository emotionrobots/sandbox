// Generated by gencpp from file DLIB/festTTS.msg
// DO NOT EDIT!


#ifndef DLIB_MESSAGE_FESTTTS_H
#define DLIB_MESSAGE_FESTTTS_H

#include <ros/service_traits.h>


#include <DLIB/festTTSRequest.h>
#include <DLIB/festTTSResponse.h>


namespace DLIB
{

struct festTTS
{

typedef festTTSRequest Request;
typedef festTTSResponse Response;
Request request;
Response response;

typedef Request RequestType;
typedef Response ResponseType;

}; // struct festTTS
} // namespace DLIB


namespace ros
{
namespace service_traits
{


template<>
struct MD5Sum< ::DLIB::festTTS > {
  static const char* value()
  {
    return "671f8e4998eaec79f1c47e339dfd527b";
  }

  static const char* value(const ::DLIB::festTTS&) { return value(); }
};

template<>
struct DataType< ::DLIB::festTTS > {
  static const char* value()
  {
    return "DLIB/festTTS";
  }

  static const char* value(const ::DLIB::festTTS&) { return value(); }
};


// service_traits::MD5Sum< ::DLIB::festTTSRequest> should match 
// service_traits::MD5Sum< ::DLIB::festTTS > 
template<>
struct MD5Sum< ::DLIB::festTTSRequest>
{
  static const char* value()
  {
    return MD5Sum< ::DLIB::festTTS >::value();
  }
  static const char* value(const ::DLIB::festTTSRequest&)
  {
    return value();
  }
};

// service_traits::DataType< ::DLIB::festTTSRequest> should match 
// service_traits::DataType< ::DLIB::festTTS > 
template<>
struct DataType< ::DLIB::festTTSRequest>
{
  static const char* value()
  {
    return DataType< ::DLIB::festTTS >::value();
  }
  static const char* value(const ::DLIB::festTTSRequest&)
  {
    return value();
  }
};

// service_traits::MD5Sum< ::DLIB::festTTSResponse> should match 
// service_traits::MD5Sum< ::DLIB::festTTS > 
template<>
struct MD5Sum< ::DLIB::festTTSResponse>
{
  static const char* value()
  {
    return MD5Sum< ::DLIB::festTTS >::value();
  }
  static const char* value(const ::DLIB::festTTSResponse&)
  {
    return value();
  }
};

// service_traits::DataType< ::DLIB::festTTSResponse> should match 
// service_traits::DataType< ::DLIB::festTTS > 
template<>
struct DataType< ::DLIB::festTTSResponse>
{
  static const char* value()
  {
    return DataType< ::DLIB::festTTS >::value();
  }
  static const char* value(const ::DLIB::festTTSResponse&)
  {
    return value();
  }
};

} // namespace service_traits
} // namespace ros

#endif // DLIB_MESSAGE_FESTTTS_H
