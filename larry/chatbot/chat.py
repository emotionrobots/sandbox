#!/usr/bin/python
from chatterbot import ChatBot

chatbot = ChatBot("Ron Obvious")

conversation = [
    "Hello",
    "Hi there!",
    "How are you doing?",
    "I'm doing great.",
    "That is good to hear",
    "Thank you.",
    "You're welcome."
]

chatbot.train(conversation)
#chatbot.get_response("Hi there!")
chatbot.get_response("Hello")
