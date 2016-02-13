; Auto-generated. Do not edit!


(cl:in-package skeleton-msg)


;//! \htmlinclude face_p.msg.html

(cl:defclass <face_p> (roslisp-msg-protocol:ros-message)
  ((header
    :reader header
    :initarg :header
    :type std_msgs-msg:Header
    :initform (cl:make-instance 'std_msgs-msg:Header))
   (arr
    :reader arr
    :initarg :arr
    :type std_msgs-msg:UInt32MultiArray
    :initform (cl:make-instance 'std_msgs-msg:UInt32MultiArray))
   (image
    :reader image
    :initarg :image
    :type sensor_msgs-msg:Image
    :initform (cl:make-instance 'sensor_msgs-msg:Image)))
)

(cl:defclass face_p (<face_p>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <face_p>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'face_p)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name skeleton-msg:<face_p> is deprecated: use skeleton-msg:face_p instead.")))

(cl:ensure-generic-function 'header-val :lambda-list '(m))
(cl:defmethod header-val ((m <face_p>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:header-val is deprecated.  Use skeleton-msg:header instead.")
  (header m))

(cl:ensure-generic-function 'arr-val :lambda-list '(m))
(cl:defmethod arr-val ((m <face_p>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:arr-val is deprecated.  Use skeleton-msg:arr instead.")
  (arr m))

(cl:ensure-generic-function 'image-val :lambda-list '(m))
(cl:defmethod image-val ((m <face_p>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader skeleton-msg:image-val is deprecated.  Use skeleton-msg:image instead.")
  (image m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <face_p>) ostream)
  "Serializes a message object of type '<face_p>"
  (roslisp-msg-protocol:serialize (cl:slot-value msg 'header) ostream)
  (roslisp-msg-protocol:serialize (cl:slot-value msg 'arr) ostream)
  (roslisp-msg-protocol:serialize (cl:slot-value msg 'image) ostream)
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <face_p>) istream)
  "Deserializes a message object of type '<face_p>"
  (roslisp-msg-protocol:deserialize (cl:slot-value msg 'header) istream)
  (roslisp-msg-protocol:deserialize (cl:slot-value msg 'arr) istream)
  (roslisp-msg-protocol:deserialize (cl:slot-value msg 'image) istream)
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<face_p>)))
  "Returns string type for a message object of type '<face_p>"
  "skeleton/face_p")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'face_p)))
  "Returns string type for a message object of type 'face_p"
  "skeleton/face_p")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<face_p>)))
  "Returns md5sum for a message object of type '<face_p>"
  "ef577c59b680f9e7134aea7b455ed9f7")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'face_p)))
  "Returns md5sum for a message object of type 'face_p"
  "ef577c59b680f9e7134aea7b455ed9f7")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<face_p>)))
  "Returns full string definition for message of type '<face_p>"
  (cl:format cl:nil "Header header~%std_msgs/UInt32MultiArray arr~%sensor_msgs/Image image~%~%================================================================================~%MSG: std_msgs/Header~%# Standard metadata for higher-level stamped data types.~%# This is generally used to communicate timestamped data ~%# in a particular coordinate frame.~%# ~%# sequence ID: consecutively increasing ID ~%uint32 seq~%#Two-integer timestamp that is expressed as:~%# * stamp.sec: seconds (stamp_secs) since epoch (in Python the variable is called 'secs')~%# * stamp.nsec: nanoseconds since stamp_secs (in Python the variable is called 'nsecs')~%# time-handling sugar is provided by the client library~%time stamp~%#Frame this data is associated with~%# 0: no frame~%# 1: global frame~%string frame_id~%~%================================================================================~%MSG: std_msgs/UInt32MultiArray~%# Please look at the MultiArrayLayout message definition for~%# documentation on all multiarrays.~%~%MultiArrayLayout  layout        # specification of data layout~%uint32[]          data          # array of data~%~%~%================================================================================~%MSG: std_msgs/MultiArrayLayout~%# The multiarray declares a generic multi-dimensional array of a~%# particular data type.  Dimensions are ordered from outer most~%# to inner most.~%~%MultiArrayDimension[] dim # Array of dimension properties~%uint32 data_offset        # padding bytes at front of data~%~%# Accessors should ALWAYS be written in terms of dimension stride~%# and specified outer-most dimension first.~%# ~%# multiarray(i,j,k) = data[data_offset + dim_stride[1]*i + dim_stride[2]*j + k]~%#~%# A standard, 3-channel 640x480 image with interleaved color channels~%# would be specified as:~%#~%# dim[0].label  = \"height\"~%# dim[0].size   = 480~%# dim[0].stride = 3*640*480 = 921600  (note dim[0] stride is just size of image)~%# dim[1].label  = \"width\"~%# dim[1].size   = 640~%# dim[1].stride = 3*640 = 1920~%# dim[2].label  = \"channel\"~%# dim[2].size   = 3~%# dim[2].stride = 3~%#~%# multiarray(i,j,k) refers to the ith row, jth column, and kth channel.~%================================================================================~%MSG: std_msgs/MultiArrayDimension~%string label   # label of given dimension~%uint32 size    # size of given dimension (in type units)~%uint32 stride  # stride of given dimension~%================================================================================~%MSG: sensor_msgs/Image~%# This message contains an uncompressed image~%# (0, 0) is at top-left corner of image~%#~%~%Header header        # Header timestamp should be acquisition time of image~%                     # Header frame_id should be optical frame of camera~%                     # origin of frame should be optical center of cameara~%                     # +x should point to the right in the image~%                     # +y should point down in the image~%                     # +z should point into to plane of the image~%                     # If the frame_id here and the frame_id of the CameraInfo~%                     # message associated with the image conflict~%                     # the behavior is undefined~%~%uint32 height         # image height, that is, number of rows~%uint32 width          # image width, that is, number of columns~%~%# The legal values for encoding are in file src/image_encodings.cpp~%# If you want to standardize a new string format, join~%# ros-users@lists.sourceforge.net and send an email proposing a new encoding.~%~%string encoding       # Encoding of pixels -- channel meaning, ordering, size~%                      # taken from the list of strings in include/sensor_msgs/image_encodings.h~%~%uint8 is_bigendian    # is this data bigendian?~%uint32 step           # Full row length in bytes~%uint8[] data          # actual matrix data, size is (step * rows)~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'face_p)))
  "Returns full string definition for message of type 'face_p"
  (cl:format cl:nil "Header header~%std_msgs/UInt32MultiArray arr~%sensor_msgs/Image image~%~%================================================================================~%MSG: std_msgs/Header~%# Standard metadata for higher-level stamped data types.~%# This is generally used to communicate timestamped data ~%# in a particular coordinate frame.~%# ~%# sequence ID: consecutively increasing ID ~%uint32 seq~%#Two-integer timestamp that is expressed as:~%# * stamp.sec: seconds (stamp_secs) since epoch (in Python the variable is called 'secs')~%# * stamp.nsec: nanoseconds since stamp_secs (in Python the variable is called 'nsecs')~%# time-handling sugar is provided by the client library~%time stamp~%#Frame this data is associated with~%# 0: no frame~%# 1: global frame~%string frame_id~%~%================================================================================~%MSG: std_msgs/UInt32MultiArray~%# Please look at the MultiArrayLayout message definition for~%# documentation on all multiarrays.~%~%MultiArrayLayout  layout        # specification of data layout~%uint32[]          data          # array of data~%~%~%================================================================================~%MSG: std_msgs/MultiArrayLayout~%# The multiarray declares a generic multi-dimensional array of a~%# particular data type.  Dimensions are ordered from outer most~%# to inner most.~%~%MultiArrayDimension[] dim # Array of dimension properties~%uint32 data_offset        # padding bytes at front of data~%~%# Accessors should ALWAYS be written in terms of dimension stride~%# and specified outer-most dimension first.~%# ~%# multiarray(i,j,k) = data[data_offset + dim_stride[1]*i + dim_stride[2]*j + k]~%#~%# A standard, 3-channel 640x480 image with interleaved color channels~%# would be specified as:~%#~%# dim[0].label  = \"height\"~%# dim[0].size   = 480~%# dim[0].stride = 3*640*480 = 921600  (note dim[0] stride is just size of image)~%# dim[1].label  = \"width\"~%# dim[1].size   = 640~%# dim[1].stride = 3*640 = 1920~%# dim[2].label  = \"channel\"~%# dim[2].size   = 3~%# dim[2].stride = 3~%#~%# multiarray(i,j,k) refers to the ith row, jth column, and kth channel.~%================================================================================~%MSG: std_msgs/MultiArrayDimension~%string label   # label of given dimension~%uint32 size    # size of given dimension (in type units)~%uint32 stride  # stride of given dimension~%================================================================================~%MSG: sensor_msgs/Image~%# This message contains an uncompressed image~%# (0, 0) is at top-left corner of image~%#~%~%Header header        # Header timestamp should be acquisition time of image~%                     # Header frame_id should be optical frame of camera~%                     # origin of frame should be optical center of cameara~%                     # +x should point to the right in the image~%                     # +y should point down in the image~%                     # +z should point into to plane of the image~%                     # If the frame_id here and the frame_id of the CameraInfo~%                     # message associated with the image conflict~%                     # the behavior is undefined~%~%uint32 height         # image height, that is, number of rows~%uint32 width          # image width, that is, number of columns~%~%# The legal values for encoding are in file src/image_encodings.cpp~%# If you want to standardize a new string format, join~%# ros-users@lists.sourceforge.net and send an email proposing a new encoding.~%~%string encoding       # Encoding of pixels -- channel meaning, ordering, size~%                      # taken from the list of strings in include/sensor_msgs/image_encodings.h~%~%uint8 is_bigendian    # is this data bigendian?~%uint32 step           # Full row length in bytes~%uint8[] data          # actual matrix data, size is (step * rows)~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <face_p>))
  (cl:+ 0
     (roslisp-msg-protocol:serialization-length (cl:slot-value msg 'header))
     (roslisp-msg-protocol:serialization-length (cl:slot-value msg 'arr))
     (roslisp-msg-protocol:serialization-length (cl:slot-value msg 'image))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <face_p>))
  "Converts a ROS message object to a list"
  (cl:list 'face_p
    (cl:cons ':header (header msg))
    (cl:cons ':arr (arr msg))
    (cl:cons ':image (image msg))
))
