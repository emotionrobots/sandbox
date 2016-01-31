; Auto-generated. Do not edit!


(cl:in-package DLIB-msg)


;//! \htmlinclude DLIB.msg.html

(cl:defclass <DLIB> (roslisp-msg-protocol:ros-message)
  ((id
    :reader id
    :initarg :id
    :type cl:fixnum
    :initform 0)
   (data
    :reader data
    :initarg :data
    :type cl:string
    :initform ""))
)

(cl:defclass DLIB (<DLIB>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <DLIB>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'DLIB)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name DLIB-msg:<DLIB> is deprecated: use DLIB-msg:DLIB instead.")))

(cl:ensure-generic-function 'id-val :lambda-list '(m))
(cl:defmethod id-val ((m <DLIB>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:id-val is deprecated.  Use DLIB-msg:id instead.")
  (id m))

(cl:ensure-generic-function 'data-val :lambda-list '(m))
(cl:defmethod data-val ((m <DLIB>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:data-val is deprecated.  Use DLIB-msg:data instead.")
  (data m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <DLIB>) ostream)
  "Serializes a message object of type '<DLIB>"
  (cl:let* ((signed (cl:slot-value msg 'id)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 256) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    )
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'data))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'data))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <DLIB>) istream)
  "Deserializes a message object of type '<DLIB>"
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'id) (cl:if (cl:< unsigned 128) unsigned (cl:- unsigned 256))))
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'data) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'data) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<DLIB>)))
  "Returns string type for a message object of type '<DLIB>"
  "DLIB/DLIB")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'DLIB)))
  "Returns string type for a message object of type 'DLIB"
  "DLIB/DLIB")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<DLIB>)))
  "Returns md5sum for a message object of type '<DLIB>"
  "3398c77d30999b784ef30c252c7c0e98")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'DLIB)))
  "Returns md5sum for a message object of type 'DLIB"
  "3398c77d30999b784ef30c252c7c0e98")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<DLIB>)))
  "Returns full string definition for message of type '<DLIB>"
  (cl:format cl:nil "int8 id~%string data~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'DLIB)))
  "Returns full string definition for message of type 'DLIB"
  (cl:format cl:nil "int8 id~%string data~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <DLIB>))
  (cl:+ 0
     1
     4 (cl:length (cl:slot-value msg 'data))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <DLIB>))
  "Converts a ROS message object to a list"
  (cl:list 'DLIB
    (cl:cons ':id (id msg))
    (cl:cons ':data (data msg))
))
