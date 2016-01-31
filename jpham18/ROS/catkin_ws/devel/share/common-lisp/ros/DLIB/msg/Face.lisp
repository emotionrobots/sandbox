; Auto-generated. Do not edit!


(cl:in-package DLIB-msg)


;//! \htmlinclude Face.msg.html

(cl:defclass <Face> (roslisp-msg-protocol:ros-message)
  ((name
    :reader name
    :initarg :name
    :type cl:string
    :initform "")
   (image
    :reader image
    :initarg :image
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

(cl:defclass Face (<Face>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <Face>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'Face)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name DLIB-msg:<Face> is deprecated: use DLIB-msg:Face instead.")))

(cl:ensure-generic-function 'name-val :lambda-list '(m))
(cl:defmethod name-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:name-val is deprecated.  Use DLIB-msg:name instead.")
  (name m))

(cl:ensure-generic-function 'image-val :lambda-list '(m))
(cl:defmethod image-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:image-val is deprecated.  Use DLIB-msg:image instead.")
  (image m))

(cl:ensure-generic-function 'llx-val :lambda-list '(m))
(cl:defmethod llx-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:llx-val is deprecated.  Use DLIB-msg:llx instead.")
  (llx m))

(cl:ensure-generic-function 'lly-val :lambda-list '(m))
(cl:defmethod lly-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:lly-val is deprecated.  Use DLIB-msg:lly instead.")
  (lly m))

(cl:ensure-generic-function 'urx-val :lambda-list '(m))
(cl:defmethod urx-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:urx-val is deprecated.  Use DLIB-msg:urx instead.")
  (urx m))

(cl:ensure-generic-function 'ury-val :lambda-list '(m))
(cl:defmethod ury-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader DLIB-msg:ury-val is deprecated.  Use DLIB-msg:ury instead.")
  (ury m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <Face>) ostream)
  "Serializes a message object of type '<Face>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'name))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'name))
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'image))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'image))
  (cl:let* ((signed (cl:slot-value msg 'llx)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 256) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    )
  (cl:let* ((signed (cl:slot-value msg 'lly)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 256) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    )
  (cl:let* ((signed (cl:slot-value msg 'urx)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 256) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    )
  (cl:let* ((signed (cl:slot-value msg 'ury)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 256) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    )
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <Face>) istream)
  "Deserializes a message object of type '<Face>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'name) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'name) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'image) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'image) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'llx) (cl:if (cl:< unsigned 128) unsigned (cl:- unsigned 256))))
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'lly) (cl:if (cl:< unsigned 128) unsigned (cl:- unsigned 256))))
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'urx) (cl:if (cl:< unsigned 128) unsigned (cl:- unsigned 256))))
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'ury) (cl:if (cl:< unsigned 128) unsigned (cl:- unsigned 256))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<Face>)))
  "Returns string type for a message object of type '<Face>"
  "DLIB/Face")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'Face)))
  "Returns string type for a message object of type 'Face"
  "DLIB/Face")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<Face>)))
  "Returns md5sum for a message object of type '<Face>"
  "19103cb4c3b62ac0bec0b40d248c0f53")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'Face)))
  "Returns md5sum for a message object of type 'Face"
  "19103cb4c3b62ac0bec0b40d248c0f53")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<Face>)))
  "Returns full string definition for message of type '<Face>"
  (cl:format cl:nil "string name~%string image~%int8 llx~%int8 lly~%int8 urx~%int8 ury~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'Face)))
  "Returns full string definition for message of type 'Face"
  (cl:format cl:nil "string name~%string image~%int8 llx~%int8 lly~%int8 urx~%int8 ury~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <Face>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'name))
     4 (cl:length (cl:slot-value msg 'image))
     1
     1
     1
     1
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <Face>))
  "Converts a ROS message object to a list"
  (cl:list 'Face
    (cl:cons ':name (name msg))
    (cl:cons ':image (image msg))
    (cl:cons ':llx (llx msg))
    (cl:cons ':lly (lly msg))
    (cl:cons ':urx (urx msg))
    (cl:cons ':ury (ury msg))
))
