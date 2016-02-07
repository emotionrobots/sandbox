
(cl:in-package :asdf)

(defsystem "skeleton-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils :sensor_msgs-msg
               :std_msgs-msg
)
  :components ((:file "_package")
    (:file "CustomString" :depends-on ("_package_CustomString"))
    (:file "_package_CustomString" :depends-on ("_package"))
    (:file "opencv" :depends-on ("_package_opencv"))
    (:file "_package_opencv" :depends-on ("_package"))
    (:file "Skeleton" :depends-on ("_package_Skeleton"))
    (:file "_package_Skeleton" :depends-on ("_package"))
    (:file "UnknownFace" :depends-on ("_package_UnknownFace"))
    (:file "_package_UnknownFace" :depends-on ("_package"))
    (:file "face_p" :depends-on ("_package_face_p"))
    (:file "_package_face_p" :depends-on ("_package"))
    (:file "namer" :depends-on ("_package_namer"))
    (:file "_package_namer" :depends-on ("_package"))
    (:file "Face" :depends-on ("_package_Face"))
    (:file "_package_Face" :depends-on ("_package"))
  ))