
(cl:in-package :asdf)

(defsystem "music-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "Genre" :depends-on ("_package_Genre"))
    (:file "_package_Genre" :depends-on ("_package"))
    (:file "Genre" :depends-on ("_package_Genre"))
    (:file "_package_Genre" :depends-on ("_package"))
  ))