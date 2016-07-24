
(cl:in-package :asdf)

(defsystem "music-srv"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "musicgenre" :depends-on ("_package_musicgenre"))
    (:file "_package_musicgenre" :depends-on ("_package"))
  ))