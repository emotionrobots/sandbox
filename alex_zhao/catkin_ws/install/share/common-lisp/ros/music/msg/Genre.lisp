; Auto-generated. Do not edit!


(cl:in-package music-msg)


;//! \htmlinclude Genre.msg.html

(cl:defclass <Genre> (roslisp-msg-protocol:ros-message)
  ((genre
    :reader genre
    :initarg :genre
    :type cl:string
    :initform ""))
)

(cl:defclass Genre (<Genre>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <Genre>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'Genre)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name music-msg:<Genre> is deprecated: use music-msg:Genre instead.")))

(cl:ensure-generic-function 'genre-val :lambda-list '(m))
(cl:defmethod genre-val ((m <Genre>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader music-msg:genre-val is deprecated.  Use music-msg:genre instead.")
  (genre m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <Genre>) ostream)
  "Serializes a message object of type '<Genre>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'genre))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'genre))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <Genre>) istream)
  "Deserializes a message object of type '<Genre>"
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
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<Genre>)))
  "Returns string type for a message object of type '<Genre>"
  "music/Genre")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'Genre)))
  "Returns string type for a message object of type 'Genre"
  "music/Genre")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<Genre>)))
  "Returns md5sum for a message object of type '<Genre>"
  "9ccf1c8ab8cb09eedf68bf876568f66e")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'Genre)))
  "Returns md5sum for a message object of type 'Genre"
  "9ccf1c8ab8cb09eedf68bf876568f66e")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<Genre>)))
  "Returns full string definition for message of type '<Genre>"
  (cl:format cl:nil "string genre~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'Genre)))
  "Returns full string definition for message of type 'Genre"
  (cl:format cl:nil "string genre~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <Genre>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'genre))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <Genre>))
  "Converts a ROS message object to a list"
  (cl:list 'Genre
    (cl:cons ':genre (genre msg))
))
