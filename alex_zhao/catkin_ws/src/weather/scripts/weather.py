#!/usr/bin/env python

import ast

def test(city):
    with open('citylist.txt') as inputfile:
        for line in inputfile:
            dictionary = ast.literal_eval(line)
	    print dictionary['name']
if __name__ == "__main__":
    test('SugarLand')
