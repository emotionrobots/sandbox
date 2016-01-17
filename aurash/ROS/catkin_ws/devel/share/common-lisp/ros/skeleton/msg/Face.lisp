; Auto-generated. Do not edit!


(cl:in-package skeleton-msg)


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
    :type cl:integer
    :initform 0)
   (lly
    :reader lly
    :initarg :lly
    :type cl:integer
    :initform 0)
   (urx
    :reader urx
    :initarg :urx
    :type cl:integer
    :initform 0)
   (ury
    :reader ury
    :initarg :ury
    :type cl:integer
    :initform 0))
)

(cl:defclass Face (<Face>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <Face>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'Face)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name skeleton-msg:<Face> is deprecated: use skeleton-msg:Face instead.")))

(cl:ensure-generic-function 'name-val :lambda-list '(m))
(cl:defmethod name-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:name-val is deprecated.  Use skeleton-msg:name instead.")
  (name m))

(cl:ensure-generic-function 'image-val :lambda-list '(m))
(cl:defmethod image-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:image-val is deprecated.  Use skeleton-msg:image instead.")
  (image m))

(cl:ensure-generic-function 'llx-val :lambda-list '(m))
(cl:defmethod llx-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:llx-val is deprecated.  Use skeleton-msg:llx instead.")
  (llx m))

(cl:ensure-generic-function 'lly-val :lambda-list '(m))
(cl:defmethod lly-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:lly-val is deprecated.  Use skeleton-msg:lly instead.")
  (lly m))

(cl:ensure-generic-function 'urx-val :lambda-list '(m))
(cl:defmethod urx-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:urx-val is deprecated.  Use skeleton-msg:urx instead.")
  (urx m))

(cl:ensure-generic-function 'ury-val :lambda-list '(m))
(cl:defmethod ury-val ((m <Face>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:ury-val is deprecated.  Use skeleton-msg:ury instead.")
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
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'llx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'llx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'llx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'llx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'lly)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'lly)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'lly)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'lly)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'urx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'urx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'urx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'urx)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'ury)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'ury)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'ury)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'ury)) ostream)
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
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'llx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'llx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'llx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'llx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'lly)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'lly)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'lly)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'lly)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'urx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'urx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'urx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'urx)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'ury)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'ury)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 16) (cl:slot-value msg 'ury)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 24) (cl:slot-value msg 'ury)) (cl:read-byte istream))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<Face>)))
  "Returns string type for a message object of type '<Face>"
  "skeleton/Face")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'Face)))
  "Returns string type for a message object of type 'Face"
  "skeleton/Face")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<Face>)))
  "Returns md5sum for a message object of type '<Face>"
  "0d177734d9fba9e1fbead9ccf8016ee6")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'Face)))
  "Returns md5sum for a message object of type 'Face"
  "0d177734d9fba9e1fbead9ccf8016ee6")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<Face>)))
  "Returns full string definition for message of type '<Face>"
  (cl:format cl:nil "string name~%string image~%uint32 llx~%uint32 lly~%uint32 urx~%uint32 ury~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'Face)))
  "Returns full string definition for message of type 'Face"
  (cl:format cl:nil "string name~%string image~%uint32 llx~%uint32 lly~%uint32 urx~%uint32 ury~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <Face>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'name))
     4 (cl:length (cl:slot-value msg 'image))
     4
     4
     4
     4
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
