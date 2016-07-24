; Auto-generated. Do not edit!


(cl:in-package music-srv)


;//! \htmlinclude musicgenre-request.msg.html

(cl:defclass <musicgenre-request> (roslisp-msg-protocol:ros-message)
  ((genre
    :reader genre
    :initarg :genre
    :type cl:string
    :initform ""))
)

(cl:defclass musicgenre-request (<musicgenre-request>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <musicgenre-request>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'musicgenre-request)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name music-srv:<musicgenre-request> is deprecated: use music-srv:musicgenre-request instead.")))

(cl:ensure-generic-function 'genre-val :lambda-list '(m))
(cl:defmethod genre-val ((m <musicgenre-request>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader music-srv:genre-val is deprecated.  Use music-srv:genre instead.")
  (genre m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <musicgenre-request>) ostream)
  "Serializes a message object of type '<musicgenre-request>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'genre))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'genre))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <musicgenre-request>) istream)
  "Deserializes a message object of type '<musicgenre-request>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'genre) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'genre) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<musicgenre-request>)))
  "Returns string type for a service object of type '<musicgenre-request>"
  "music/musicgenreRequest")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'musicgenre-request)))
  "Returns string type for a service object of type 'musicgenre-request"
  "music/musicgenreRequest")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<musicgenre-request>)))
  "Returns md5sum for a message object of type '<musicgenre-request>"
  "24f79890f3c7bcd70765f9b2c0864a5a")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'musicgenre-request)))
  "Returns md5sum for a message object of type 'musicgenre-request"
  "24f79890f3c7bcd70765f9b2c0864a5a")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<musicgenre-request>)))
  "Returns full string definition for message of type '<musicgenre-request>"
  (cl:format cl:nil "string genre~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'musicgenre-request)))
  "Returns full string definition for message of type 'musicgenre-request"
  (cl:format cl:nil "string genre~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <musicgenre-request>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'genre))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <musicgenre-request>))
  "Converts a ROS message object to a list"
  (cl:list 'musicgenre-request
    (cl:cons ':genre (genre msg))
))
;//! \htmlinclude musicgenre-response.msg.html

(cl:defclass <musicgenre-response> (roslisp-msg-protocol:ros-message)
  ((song
    :reader song
    :initarg :song
    :type cl:string
    :initform ""))
)

(cl:defclass musicgenre-response (<musicgenre-response>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <musicgenre-response>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'musicgenre-response)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name music-srv:<musicgenre-response> is deprecated: use music-srv:musicgenre-response instead.")))

(cl:ensure-generic-function 'song-val :lambda-list '(m))
(cl:defmethod song-val ((m <musicgenre-response>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader music-srv:song-val is deprecated.  Use music-srv:song instead.")
  (song m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <musicgenre-response>) ostream)
  "Serializes a message object of type '<musicgenre-response>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'song))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'song))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <musicgenre-response>) istream)
  "Deserializes a message object of type '<musicgenre-response>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'song) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'song) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<musicgenre-response>)))
  "Returns string type for a service object of type '<musicgenre-response>"
  "music/musicgenreResponse")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'musicgenre-response)))
  "Returns string type for a service object of type 'musicgenre-response"
  "music/musicgenreResponse")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<musicgenre-response>)))
  "Returns md5sum for a message object of type '<musicgenre-response>"
  "24f79890f3c7bcd70765f9b2c0864a5a")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'musicgenre-response)))
  "Returns md5sum for a message object of type 'musicgenre-response"
  "24f79890f3c7bcd70765f9b2c0864a5a")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<musicgenre-response>)))
  "Returns full string definition for message of type '<musicgenre-response>"
  (cl:format cl:nil "string song~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'musicgenre-response)))
  "Returns full string definition for message of type 'musicgenre-response"
  (cl:format cl:nil "string song~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <musicgenre-response>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'song))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <musicgenre-response>))
  "Converts a ROS message object to a list"
  (cl:list 'musicgenre-response
    (cl:cons ':song (song msg))
))
(cl:defmethod roslisp-msg-protocol:service-request-type ((msg (cl:eql 'musicgenre)))
  'musicgenre-request)
(cl:defmethod roslisp-msg-protocol:service-response-type ((msg (cl:eql 'musicgenre)))
  'musicgenre-response)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'musicgenre)))
  "Returns string type for a service object of type '<musicgenre>"
  "music/musicgenre")