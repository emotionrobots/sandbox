
(cl:in-package :asdf)

(defsystem "weather-srv"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "response" :depends-on ("_package_response"))
    (:file "_package_response" :depends-on ("_package"))
  ))