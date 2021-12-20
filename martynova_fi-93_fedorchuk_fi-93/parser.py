#import regex

def remove_extra(cmd):
	num_end = lambda n: len(n) if n.find(';') == -1 else n.find(';')
	cmd = cmd[:num_end(cmd)]
	cmd = cmd.split()
	cmd = ' '.join(cmd)
	return cmd
