import regex as reg
cmd = ""
while True:
    x = input()
    if x:
        cmd += x + " "
    else:
        break

def remove_extra(cmd):
    cmd = cmd.lower()
    cmd = cmd.split()
    cmd = ' '.join(cmd)
    num_end = lambda n: len(n) if n.find(';') == -1 else n.find(';')
    cmd = cmd[:num_end(cmd)]
    return cmd

def define_cmd(cmd):
    keywords = ["create", "insert", "contains", "search", "print_tree"]
    cmd = cmd.split()
    if cmd[0] in keywords:
        return cmd[0], 1
    else:
        return ' ', -1
