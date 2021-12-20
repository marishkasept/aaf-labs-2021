using System;
using System.Linq;

namespace ua.lab.oaa.Components{
    class Trie{
        public Node Root = new Node();
        public string name;
        public Trie(string[] words, string newName) {
            name = newName;
            if(words != null){
            for (int w = 0; w < words.Length; w++){
                var word = words[w];
                var node = Root;
                for (int len = 0; len < word.Length; len++){
                    var letter = word[len];
                    Node next;
                    if (!node.Edges.TryGetValue(letter.ToString(), out next)){
                        next = new Node();
                        node.Edges.Add(letter.ToString(), next);
                    }
                    node = next;
                }

            }
            }
        }
    }
}