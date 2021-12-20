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

            foreach (Commands command in (Commands[]) Enum.GetValues(typeof(Commands)))
            {
                if (command.ToString() == userChoice )
                {
                    success = true;
                    choice = command;
                }

                if (!success)
                {
                    Console.WriteLine("Error! There is no command like this. Try again");
                    return 0;
                }

                switch (choice)
                {
                    case Commands.EXIT:
					return -1;
                    break;

                    case Commands.CREATE:
                        if (!IsCorrectWordsCount(words, 2)){
                            return 0;
                        }

                        trie = new Trie(null, words[1]);
					    triesInCache.Add(trie);
                        ErrorNoTrie();
                        break;

                    case Commands.INSERT:
                        if (!IsCorrectWordsCount(words, 3)){
                            return 0;
                        }

                        if(triesInCache.Exists(x => x.name == words[1])){
                            triesInCache.Find(x => x.name == words[1]).AddWord(words[2].Trim('"'));
                        }else{
                            ErrorNoTrie();
                        }
                        break;

                    case Commands.PRINT_TREE:
                        if (!IsCorrectWordsCount(words, 2)){
                            return 0;
                        }

                        if(triesInCache.Exists(x => x.name == words[1])){
                            triesInCache.Find(x => x.name == words[1]).PrintTrie();
                        }else{
                            ErrorNoTrie();
                            return 0;
                        }
                        Console.WriteLine("Success!");
                        break;

                    case Commands.CONTAINS:
					    if (!IsCorrectWordsCount(words, 3)){
                            return 0;
                        }

                        if(triesInCache.Exists(x => x.name == words[1])){
                    	    trie = triesInCache.Find(x => x.name == words[1]);
                    	    var result = trie.ContainsWord(words[2].Trim('"'));
						    Console.WriteLine($"The result is {result}");
					    }else{
						    ErrorNoTrie();
					    }
                        break;

                    case Commands.SEARCH:
                        break;

                    default:
                        Console.WriteLine("Unhandled Error! Try again");
                        break;

                }
            }
        }
    }
}