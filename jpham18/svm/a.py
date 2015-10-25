#!/usr/bin/env python2

import os

print os.listdir("./")
if "emotionData.bin" in os.listdir("./"):
	print True