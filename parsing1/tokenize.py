import sys

path = sys.argv[1]
content = open(path).read()
for char in content:
    print(char.rjust(10))
