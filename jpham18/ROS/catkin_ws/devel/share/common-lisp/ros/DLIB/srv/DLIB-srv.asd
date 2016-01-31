
(cl:in-package :asdf)

(defsystem "DLIB-srv"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "festTTS" :depends-on ("_package_festTTS"))
    (:file "_package_festTTS" :depends-on ("_package"))
  ))