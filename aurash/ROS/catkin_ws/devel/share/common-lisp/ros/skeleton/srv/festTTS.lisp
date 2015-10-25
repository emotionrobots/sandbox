; Auto-generated. Do not edit!


(cl:in-package skeleton-srv)


;//! \htmlinclude festTTS-request.msg.html

(cl:defclass <festTTS-request> (roslisp-msg-protocol:ros-message)
  ((str
    :reader str
    :initarg :str
    :type cl:string
    :initform ""))
)

(cl:defclass festTTS-request (<festTTS-request>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <festTTS-request>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'festTTS-request)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name skeleton-srv:<festTTS-request> is deprecated: use skeleton-srv:festTTS-request instead.")))

(cl:ensure-generic-function 'str-val :lambda-list '(m))
(cl:defmethod str-val ((m <festTTS-request>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-srv:str-val is deprecated.  Use skeleton-srv:str instead.")
  (str m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <festTTS-request>) ostream)
  "Serializes a message object of type '<festTTS-request>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'str))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'str))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <festTTS-request>) istream)
  "Deserializes a message object of type '<festTTS-request>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'str) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'str) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<festTTS-request>)))
  "Returns string type for a service object of type '<festTTS-request>"
  "skeleton/festTTSRequest")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'festTTS-request)))
  "Returns string type for a service object of type 'festTTS-request"
  "skeleton/festTTSRequest")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<festTTS-request>)))
  "Returns md5sum for a message object of type '<festTTS-request>"
  "671f8e4998eaec79f1c47e339dfd527b")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'festTTS-request)))
  "Returns md5sum for a message object of type 'festTTS-request"
  "671f8e4998eaec79f1c47e339dfd527b")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<festTTS-request>)))
  "Returns full string definition for message of type '<festTTS-request>"
  (cl:format cl:nil "string str~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'festTTS-request)))
  "Returns full string definition for message of type 'festTTS-request"
  (cl:format cl:nil "string str~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <festTTS-request>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'str))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <festTTS-request>))
  "Converts a ROS message object to a list"
  (cl:list 'festTTS-request
    (cl:cons ':str (str msg))
))
;//! \htmlinclude festTTS-response.msg.html

(cl:defclass <festTTS-response> (roslisp-msg-protocol:ros-message)
  ((str
    :reader str
    :initarg :str
    :type cl:string
    :initform ""))
)

(cl:defclass festTTS-response (<festTTS-response>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <festTTS-response>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'festTTS-response)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name skeleton-srv:<festTTS-response> is deprecated: use skeleton-srv:festTTS-response instead.")))

(cl:ensure-generic-function 'str-val :lambda-list '(m))
(cl:defmethod str-val ((m <festTTS-response>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-srv:str-val is deprecated.  Use skeleton-srv:str instead.")
  (str m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <festTTS-response>) ostream)
  "Serializes a message object of type '<festTTS-response>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'str))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'str))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <festTTS-response>) istream)
  "Deserializes a message object of type '<festTTS-response>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'str) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'str) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<festTTS-response>)))
  "Returns string type for a service object of type '<festTTS-response>"
  "skeleton/festTTSResponse")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'festTTS-response)))
  "Returns string type for a service object of type 'festTTS-response"
  "skeleton/festTTSResponse")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<festTTS-response>)))
  "Returns md5sum for a message object of type '<festTTS-response>"
  "671f8e4998eaec79f1c47e339dfd527b")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'festTTS-response)))
  "Returns md5sum for a message object of type 'festTTS-response"
  "671f8e4998eaec79f1c47e339dfd527b")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<festTTS-response>)))
  "Returns full string definition for message of type '<festTTS-response>"
  (cl:format cl:nil "string str~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'festTTS-response)))
  "Returns full string definition for message of type 'festTTS-response"
  (cl:format cl:nil "string str~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <festTTS-response>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'str))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <festTTS-response>))
  "Converts a ROS message object to a list"
  (cl:list 'festTTS-response
    (cl:cons ':str (str msg))
))
(cl:defmethod roslisp-msg-protocol:service-request-type ((msg (cl:eql 'festTTS)))
  'festTTS-request)
(cl:defmethod roslisp-msg-protocol:service-response-type ((msg (cl:eql 'festTTS)))
  'festTTS-response)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'festTTS)))
  "Returns string type for a service object of type '<festTTS>"
  "skeleton/festTTS")