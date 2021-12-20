import parser

cmd = ""
while True:
  x = input()
  if x:
    cmd += x + " "
  else:
    break

print(parser.remove_extra(cmd))