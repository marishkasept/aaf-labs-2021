import regex as reg

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

def p_create(cmd):
    cmd = cmd.split()
    label = cmd[1]
    if(len(cmd) > 2):
        return label, -2
    if (reg.search("[a-zA-Z]", label[0]) == None):
        return label, -3
    if (reg.search("[a-zA-Z0-9_]*", label) == None):
        return label, -4
    return label, 5

def p_remove(cmd):
    cmd = cmd.split()
    sit = 0
    label = cmd[1]
    ind = 0
    if(len(cmd) == 3):
        sit = 2
        ind = cmd[2]
    elif (len(cmd) == 2):
        sit = 1
    else:
        return sit, label, ind, -2
    return sit, label, ind, 5

def p_insert(cmd):
    cmd = cmd.split()
    label = cmd[1]
    lines = ''
    if len(cmd)<3:
        return label, lines, -5
    ind = 0
    for i in range(len(cmd)):
        if (cmd[i][-1] == "\""):
            ind = i
    if (cmd[2][0] != "\""):
        return label, lines, -6
    if (cmd[-1][-1] != "\""):
        return label, lines, -6
    if (len(cmd)-1 != ind):
        return label, lines, -2
    lines = ' '.join([cmd[2:ind+1]])
    lines = lines.strip("\"")
    return label, lines, 5
