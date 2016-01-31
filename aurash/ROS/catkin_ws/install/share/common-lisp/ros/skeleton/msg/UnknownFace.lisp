; Auto-generated. Do not edit!


(cl:in-package skeleton-msg)


;//! \htmlinclude UnknownFace.msg.html

(cl:defclass <UnknownFace> (roslisp-msg-protocol:ros-message)
  ((name
    :reader name
    :initarg :name
    :type cl:string
    :initform "")
   (llx
    :reader llx
    :initarg :llx
    :type cl:fixnum
    :initform 0)
   (lly
    :reader lly
    :initarg :lly
    :type cl:fixnum
    :initform 0)
   (urx
    :reader urx
    :initarg :urx
    :type cl:fixnum
    :initform 0)
   (ury
    :reader ury
    :initarg :ury
    :type cl:fixnum
    :initform 0))
)

(cl:defclass UnknownFace (<UnknownFace>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <UnknownFace>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'UnknownFace)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name skeleton-msg:<UnknownFace> is deprecated: use skeleton-msg:UnknownFace instead.")))

(cl:ensure-generic-function 'name-val :lambda-list '(m))
(cl:defmethod name-val ((m <UnknownFace>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:name-val is deprecated.  Use skeleton-msg:name instead.")
  (name m))

(cl:ensure-generic-function 'llx-val :lambda-list '(m))
(cl:defmethod llx-val ((m <UnknownFace>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:llx-val is deprecated.  Use skeleton-msg:llx instead.")
  (llx m))

(cl:ensure-generic-function 'lly-val :lambda-list '(m))
(cl:defmethod lly-val ((m <UnknownFace>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:lly-val is deprecated.  Use skeleton-msg:lly instead.")
  (lly m))

(cl:ensure-generic-function 'urx-val :lambda-list '(m))
(cl:defmethod urx-val ((m <UnknownFace>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:urx-val is deprecated.  Use skeleton-msg:urx instead.")
  (urx m))

(cl:ensure-generic-function 'ury-val :lambda-list '(m))
(cl:defmethod ury-val ((m <UnknownFace>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:ury-val is deprecated.  Use skeleton-msg:ury instead.")
  (ury m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <UnknownFace>) ostream)
  "Serializes a message object of type '<UnknownFace>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'name))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'name))
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'llx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'lly)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'urx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'ury)) ostream)
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <UnknownFace>) istream)
  "Deserializes a message object of type '<UnknownFace>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'name) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'name) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'llx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'lly)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'urx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'ury)) (cl:read-byte istream))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<UnknownFace>)))
  "Returns string type for a message object of type '<UnknownFace>"
  "skeleton/UnknownFace")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'UnknownFace)))
  "Returns string type for a message object of type 'UnknownFace"
  "skeleton/UnknownFace")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<UnknownFace>)))
  "Returns md5sum for a message object of type '<UnknownFace>"
  "6cad868307b5c8d4a7455516e9791b3a")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'UnknownFace)))
  "Returns md5sum for a message object of type 'UnknownFace"
  "6cad868307b5c8d4a7455516e9791b3a")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<UnknownFace>)))
  "Returns full string definition for message of type '<UnknownFace>"
  (cl:format cl:nil "string name~%uint8 llx~%uint8 lly~%uint8 urx~%uint8 ury~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'UnknownFace)))
  "Returns full string definition for message of type 'UnknownFace"
  (cl:format cl:nil "string name~%uint8 llx~%uint8 lly~%uint8 urx~%uint8 ury~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <UnknownFace>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'name))
     1
     1
     1
     1
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <UnknownFace>))
  "Converts a ROS message object to a list"
  (cl:list 'UnknownFace
    (cl:cons ':name (name msg))
    (cl:cons ':llx (llx msg))
    (cl:cons ':lly (lly msg))
    (cl:cons ':urx (urx msg))
    (cl:cons ':ury (ury msg))
))
