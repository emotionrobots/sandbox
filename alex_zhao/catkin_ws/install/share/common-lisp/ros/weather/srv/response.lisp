; Auto-generated. Do not edit!


(cl:in-package weather-srv)


;//! \htmlinclude response-request.msg.html

(cl:defclass <response-request> (roslisp-msg-protocol:ros-message)
  ((city
    :reader city
    :initarg :city
    :type cl:string
    :initform ""))
)

(cl:defclass response-request (<response-request>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <response-request>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'response-request)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name weather-srv:<response-request> is deprecated: use weather-srv:response-request instead.")))

(cl:ensure-generic-function 'city-val :lambda-list '(m))
(cl:defmethod city-val ((m <response-request>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader weather-srv:city-val is deprecated.  Use weather-srv:city instead.")
  (city m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <response-request>) ostream)
  "Serializes a message object of type '<response-request>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'city))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'city))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <response-request>) istream)
  "Deserializes a message object of type '<response-request>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'city) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'city) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<response-request>)))
  "Returns string type for a service object of type '<response-request>"
  "weather/responseRequest")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'response-request)))
  "Returns string type for a service object of type 'response-request"
  "weather/responseRequest")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<response-request>)))
  "Returns md5sum for a message object of type '<response-request>"
  "5712ed08c2d5465e5c8c268201c42b92")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'response-request)))
  "Returns md5sum for a message object of type 'response-request"
  "5712ed08c2d5465e5c8c268201c42b92")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<response-request>)))
  "Returns full string definition for message of type '<response-request>"
  (cl:format cl:nil "string city~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'response-request)))
  "Returns full string definition for message of type 'response-request"
  (cl:format cl:nil "string city~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <response-request>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'city))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <response-request>))
  "Converts a ROS message object to a list"
  (cl:list 'response-request
    (cl:cons ':city (city msg))
))
;//! \htmlinclude response-response.msg.html

(cl:defclass <response-response> (roslisp-msg-protocol:ros-message)
  ((temperature
    :reader temperature
    :initarg :temperature
    :type cl:float
    :initform 0.0))
)

(cl:defclass response-response (<response-response>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <response-response>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'response-response)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name weather-srv:<response-response> is deprecated: use weather-srv:response-response instead.")))

(cl:ensure-generic-function 'temperature-val :lambda-list '(m))
(cl:defmethod temperature-val ((m <response-response>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader weather-srv:temperature-val is deprecated.  Use weather-srv:temperature instead.")
  (temperature m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <response-response>) ostream)
  "Serializes a message object of type '<response-response>"
  (cl:let ((bits (roslisp-utils:encode-single-float-bits (cl:slot-value msg 'temperature))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) bits) ostream))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <response-response>) istream)
  "Deserializes a message object of type '<response-response>"
    (cl:let ((bits 0))
      (cl:setf (cl:ldb (cl:byte 8 0) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) bits) (cl:read-byte istream))
    (cl:setf (cl:slot-value msg 'temperature) (roslisp-utils:decode-single-float-bits bits)))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<response-response>)))
  "Returns string type for a service object of type '<response-response>"
  "weather/responseResponse")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'response-response)))
  "Returns string type for a service object of type 'response-response"
  "weather/responseResponse")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<response-response>)))
  "Returns md5sum for a message object of type '<response-response>"
  "5712ed08c2d5465e5c8c268201c42b92")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'response-response)))
  "Returns md5sum for a message object of type 'response-response"
  "5712ed08c2d5465e5c8c268201c42b92")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<response-response>)))
  "Returns full string definition for message of type '<response-response>"
  (cl:format cl:nil "float32 temperature~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'response-response)))
  "Returns full string definition for message of type 'response-response"
  (cl:format cl:nil "float32 temperature~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <response-response>))
  (cl:+ 0
     4
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <response-response>))
  "Converts a ROS message object to a list"
  (cl:list 'response-response
    (cl:cons ':temperature (temperature msg))
))
(cl:defmethod roslisp-msg-protocol:service-request-type ((msg (cl:eql 'response)))
  'response-request)
(cl:defmethod roslisp-msg-protocol:service-response-type ((msg (cl:eql 'response)))
  'response-response)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'response)))
  "Returns string type for a service object of type '<response>"
  "weather/response")