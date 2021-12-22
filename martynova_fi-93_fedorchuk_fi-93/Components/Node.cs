using System;
using System.Text;

namespace ua.lab.oaa.Components{
    class Node{
        public SortedDictionary<String, Node> Edges = new SortedDictionary<String, Node>();
		public List<bool> IsWordList = new List<bool>();

        override public String ToString() {
            StringBuilder buffer = new StringBuilder(50);
            print(buffer, "", "", "[root]");
            return buffer.ToString();
        }

        private void print(StringBuilder buffer, string prefix, string childrenPrefix,  string name) {
            buffer.Append(prefix);
            buffer.Append(name);
            buffer.Append('\n');
            var nodes = Edges.Values.ToList();
            for( int i = 0; i < nodes.Count; i++){
                Node next = nodes[i];
                if (i != nodes.Count - 1) {
                    next.print(buffer, childrenPrefix + "├──", childrenPrefix + "│  ", Edges.FirstOrDefault(x => x.Value == next).Key);
                } else {
                    next.print(buffer, childrenPrefix + "└──", childrenPrefix + "   ", Edges.FirstOrDefault(x => x.Value == next).Key);
                }
            }
        }


    }
}