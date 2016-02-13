; Auto-generated. Do not edit!


(cl:in-package skeleton-msg)


;//! \htmlinclude CustomString.msg.html

(cl:defclass <CustomString> (roslisp-msg-protocol:ros-message)
  ((header
    :reader header
    :initarg :header
    :type std_msgs-msg:Header
    :initform (cl:make-instance 'std_msgs-msg:Header))
   (data
    :reader data
    :initarg :data
    :type cl:string
    :initform ""))
)

(cl:defclass CustomString (<CustomString>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <CustomString>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'CustomString)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name skeleton-msg:<CustomString> is deprecated: use skeleton-msg:CustomString instead.")))

(cl:ensure-generic-function 'header-val :lambda-list '(m))
(cl:defmethod header-val ((m <CustomString>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:header-val is deprecated.  Use skeleton-msg:header instead.")
  (header m))

(cl:ensure-generic-function 'data-val :lambda-list '(m))
(cl:defmethod data-val ((m <CustomString>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:data-val is deprecated.  Use skeleton-msg:data instead.")
  (data m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <CustomString>) ostream)
  "Serializes a message object of type '<CustomString>"
  (roslisp-msg-protocol:serialize (cl:slot-value msg 'header) ostream)
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'data))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'data))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <CustomString>) istream)
  "Deserializes a message object of type '<CustomString>"
  (roslisp-msg-protocol:deserialize (cl:slot-value msg 'header) istream)
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
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<CustomString>)))
  "Returns string type for a message object of type '<CustomString>"
  "skeleton/CustomString")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'CustomString)))
  "Returns string type for a message object of type 'CustomString"
  "skeleton/CustomString")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<CustomString>)))
  "Returns md5sum for a message object of type '<CustomString>"
  "c99a9440709e4d4a9716d55b8270d5e7")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'CustomString)))
  "Returns md5sum for a message object of type 'CustomString"
  "c99a9440709e4d4a9716d55b8270d5e7")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<CustomString>)))
  "Returns full string definition for message of type '<CustomString>"
  (cl:format cl:nil "Header header~%string data~%~%================================================================================~%MSG: std_msgs/Header~%# Standard metadata for higher-level stamped data types.~%# This is generally used to communicate timestamped data ~%# in a particular coordinate frame.~%# ~%# sequence ID: consecutively increasing ID ~%uint32 seq~%#Two-integer timestamp that is expressed as:~%# * stamp.sec: seconds (stamp_secs) since epoch (in Python the variable is called 'secs')~%# * stamp.nsec: nanoseconds since stamp_secs (in Python the variable is called 'nsecs')~%# time-handling sugar is provided by the client library~%time stamp~%#Frame this data is associated with~%# 0: no frame~%# 1: global frame~%string frame_id~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'CustomString)))
  "Returns full string definition for message of type 'CustomString"
  (cl:format cl:nil "Header header~%string data~%~%================================================================================~%MSG: std_msgs/Header~%# Standard metadata for higher-level stamped data types.~%# This is generally used to communicate timestamped data ~%# in a particular coordinate frame.~%# ~%# sequence ID: consecutively increasing ID ~%uint32 seq~%#Two-integer timestamp that is expressed as:~%# * stamp.sec: seconds (stamp_secs) since epoch (in Python the variable is called 'secs')~%# * stamp.nsec: nanoseconds since stamp_secs (in Python the variable is called 'nsecs')~%# time-handling sugar is provided by the client library~%time stamp~%#Frame this data is associated with~%# 0: no frame~%# 1: global frame~%string frame_id~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <CustomString>))
  (cl:+ 0
     (roslisp-msg-protocol:serialization-length (cl:slot-value msg 'header))
     4 (cl:length (cl:slot-value msg 'data))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <CustomString>))
  "Converts a ROS message object to a list"
  (cl:list 'CustomString
    (cl:cons ':header (header msg))
    (cl:cons ':data (data msg))
))
