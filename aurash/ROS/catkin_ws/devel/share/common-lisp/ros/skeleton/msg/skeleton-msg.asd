
(cl:in-package :asdf)

(defsystem "skeleton-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "opencv" :depends-on ("_package_opencv"))
    (:file "_package_opencv" :depends-on ("_package"))
    (:file "Skeleton" :depends-on ("_package_Skeleton"))
    (:file "_package_Skeleton" :depends-on ("_package"))
    (:file "namer" :depends-on ("_package_namer"))
    (:file "_package_namer" :depends-on ("_package"))
  ))