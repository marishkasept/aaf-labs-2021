using System;
using System.Collections.Generic;
using ua.lab.oaa.Components;

namespace ua.lab.oaa{
    class Program{
        static void Main(string[] args){
			List<Trie> tries = new List<Trie>();
			Console.WriteLine("Welcome! Try to create a new trie with CREATE command");
            while(true){
                var command = Console.ReadLine();
				if(Parser.StartCommand(command, tries) == -1){
					return;
				}
            }
        }

    }
}
