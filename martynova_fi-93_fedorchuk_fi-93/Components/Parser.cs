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

                    trie = new Trie(words[1]);
					triesInCache.Add(trie);
                    Console.WriteLine("Success!");
                    break;

                case Commands.INSERT:
                    if (!IsCorrectWordsCount(words, 3)){
                        return 0;
                    }

                    if(triesInCache.Exists(x => x.name == words[1])){
                        triesInCache.Find(x => x.name == words[1]).AddWord(words[2].Trim('"'));
                        Console.WriteLine("Success!");
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
						Console.WriteLine("Success!");
                    }else{
                        ErrorNoTrie();
                        return 0;
                    }
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
			return 0;
		}

		static private bool IsCorrectWordsCount(string[] words, int neededWords){
			if(words.Length > neededWords){
				Console.WriteLine($"Error! You need use only {neededWords - 1} word after command. Try again");
				return false;
			}
			if(words.Length < neededWords){
				Console.WriteLine($"Error! Maybe you forgot to add parametres? Try again");
				return false;
			}
			return true;
		}

		static private void ErrorNoTrie(){
			Console.WriteLine("This trie doesnt exists. Try create it with command CREATE");
		}


    }
}