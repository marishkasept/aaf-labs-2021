using System;
using System.Text;
using ua.lab.oaa.Constants;

namespace ua.lab.oaa.Components{
    static class Parser{
        static public int StartCommand(string userCommand, List<Trie> triesInCache)
        {
            Commands choice = Commands.DEFAULT;
			Trie trie;
            var clearCommand = userCommand.Split(';')[0];
            var words = clearCommand.Split(' ', '\t', '\r', '\n');
            bool success = false;
            string userChoice = words[0].ToUpper();
        }
    }
}