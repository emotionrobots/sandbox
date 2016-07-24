; Auto-generated. Do not edit!


(cl:in-package weather-msg)


;//! \htmlinclude city.msg.html

(cl:defclass <city> (roslisp-msg-protocol:ros-message)
  ((city
    :reader city
    :initarg :city
    :type cl:string
    :initform ""))
)

(cl:defclass city (<city>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <city>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'city)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name weather-msg:<city> is deprecated: use weather-msg:city instead.")))

(cl:ensure-generic-function 'city-val :lambda-list '(m))
(cl:defmethod city-val ((m <city>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader weather-msg:city-val is deprecated.  Use weather-msg:city instead.")
  (city m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <city>) ostream)
  "Serializes a message object of type '<city>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'city))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'city))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <city>) istream)
  "Deserializes a message object of type '<city>"
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
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<city>)))
  "Returns string type for a message object of type '<city>"
  "weather/city")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'city)))
  "Returns string type for a message object of type 'city"
  "weather/city")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<city>)))
  "Returns md5sum for a message object of type '<city>"
  "6ca7c9808e023a3417491b7104889ca1")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'city)))
  "Returns md5sum for a message object of type 'city"
  "6ca7c9808e023a3417491b7104889ca1")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<city>)))
  "Returns full string definition for message of type '<city>"
  (cl:format cl:nil "string city~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'city)))
  "Returns full string definition for message of type 'city"
  (cl:format cl:nil "string city~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <city>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'city))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <city>))
  "Converts a ROS message object to a list"
  (cl:list 'city
    (cl:cons ':city (city msg))
))
