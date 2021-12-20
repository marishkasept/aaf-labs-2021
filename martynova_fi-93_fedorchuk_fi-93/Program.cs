using System;
using System.Collections.Generic;
using ua.lab.oaa.Components;

namespace ua.lab.oaa{
    class Program{
        static void Main(string[] args){
			List<Trie> tries = new List<Trie>();
			Trie trie;
			var choice = "-1";
            while(true){
                var command = Console.ReadLine();
				if(Parser.StartCommand(command, tries) == -1){
					return;
				}
            }
        }

    }
}