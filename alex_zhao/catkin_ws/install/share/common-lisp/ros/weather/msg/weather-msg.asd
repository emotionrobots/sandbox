
(cl:in-package :asdf)

(defsystem "weather-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "city" :depends-on ("_package_city"))
    (:file "_package_city" :depends-on ("_package"))
    (:file "city" :depends-on ("_package_city"))
    (:file "_package_city" :depends-on ("_package"))
  ))