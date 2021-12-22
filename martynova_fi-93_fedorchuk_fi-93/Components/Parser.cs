using System;
using System.Text;
using ua.lab.oaa.Constants;
using System.Text.RegularExpressions;

namespace ua.lab.oaa.Components{
    static class Parser{
		private const string WHERE_COMMAND = "WHERE";
        static public int StartCommand(string userCommand, List<Trie> triesInCache)
        {
			if(!userCommand.Contains(';')){
				Console.WriteLine("Error! Need semicolon (;) for end of the command");
				return 0;
			}
            Commands choice = Commands.EXIT;
			Trie trie;
			userCommand = Regex.Replace(userCommand, @"\s+", " ");
			userCommand = Regex.Replace(userCommand, " ;", ";");
            var clearCommand = userCommand.Split(';')[0];
            var words = clearCommand.Split(new char[] {' ', '\t', '\r', '\n'});
            bool success = false;
            string userChoice = words[0].ToUpper();

            foreach (Commands command in (Commands[]) Enum.GetValues(typeof(Commands)))
            {
				string commandStr = command.ToString();
				if(command == Commands.EXIT)
				{
					commandStr = String.Concat(".", commandStr);
				}
                if (commandStr == userChoice )
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
					if(triesInCache.Exists(x => x.name == words[1])){
                        Console.WriteLine("Error! Trie with this name already exist!");
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
					switch(words.Length){
						case 2:
							if(triesInCache.Exists(x => x.name == words[1])){
                        		triesInCache.Find(x => x.name == words[1]).PrintAllWords(true);
							}
							break;
						case 3:
							bool successType = false;
							OrderType order = OrderType.ASC;
							words[2] = words[2].ToUpper();
							foreach (OrderType type in (OrderType[]) Enum.GetValues(typeof(OrderType))){
								string typeStr = type.ToString();
                				if (typeStr == words[2]){
                    				successType = true;
                    				order = type;
                				}
            				}
							if(!successType){
								Console.WriteLine("Incorrect orderType. Try ASC or DESC");
								break;
							}
							switch(order){
								case OrderType.ASC:
									if(triesInCache.Exists(x => x.name == words[1])){
                        				triesInCache.Find(x => x.name == words[1]).PrintAllWords(true);
									}
									break;
								case OrderType.DESC:
									if(triesInCache.Exists(x => x.name == words[1])){
                        				triesInCache.Find(x => x.name == words[1]).PrintAllWords(false);
									}
									break;
							}
							break;
						case 5:
							if(words[2].ToUpper() != WHERE_COMMAND){
								Console.WriteLine("Incorrect WHERE word. Try use WHERE in third word");
								break;
							}
							bool successFilter = false;
							words[3] = words[3].ToUpper();
                			if (FilterType.MATCH.ToString() == words[3]){
                    			successFilter = true;
                			}
							if(!successFilter){
								Console.WriteLine("Incorrect filterType. Try BETWEEN or MATCH");
								break;
							}
							if(triesInCache.Exists(x => x.name == words[1])){
                        		triesInCache.Find(x => x.name == words[1]).GetRegexWord(words[4].Trim('"'), true);
							} else{
								ErrorNoTrie();
							}
							break;
                    	case 6:
							if(words[2].ToUpper() != WHERE_COMMAND){
								Console.WriteLine("Incorrect WHERE word. Try use WHERE in third word");
								break;
							}
							bool successf = false;
							FilterType orderCom = FilterType.MATCH;
							words[3] = words[3].ToUpper();
							foreach (FilterType type in (FilterType[]) Enum.GetValues(typeof(FilterType))){
								string typeStr = type.ToString();
                				if (typeStr == words[3]){
                    				successf = true;
                    				orderCom = type;
                				}
            				}
							if(!successf){
								Console.WriteLine("Incorrect orderType. Try ASC or DESC");
								break;
							}
							switch(orderCom){
								case FilterType.BETWEEN:
									if(triesInCache.Exists(x => x.name == words[1])){
                        				triesInCache.Find(x => x.name == words[1]).GetBetweenWords(words[4].Trim(',').Trim('"'), words[5].Trim('"'), true);
									} else{
										ErrorNoTrie();
									}
									break;

								case FilterType.MATCH:
									bool successOrdering2 = false;
									OrderType ordering2 = OrderType.ASC;
									words[5] = words[5].ToUpper();
									foreach (OrderType type in (OrderType[]) Enum.GetValues(typeof(OrderType))){
										string typeStr = type.ToString();
                						if (typeStr == words[6]){
                    						successOrdering2 = true;
                    						ordering2 = type;
                						}
            						}
									if(!successOrdering2){
										Console.WriteLine("Incorrect orderType. Try ASC or DESC");
										break;
									}
									if(triesInCache.Exists(x => x.name == words[1])){
										switch(ordering2){
											case OrderType.ASC:
                        					triesInCache.Find(x => x.name == words[1]).GetRegexWord(words[4].Trim('"'), true);
											break;
										case OrderType.DESC:
                        					triesInCache.Find(x => x.name == words[1]).GetRegexWord(words[4].Trim('"'), false);
											break;
										}

									}else{
										ErrorNoTrie();
									}
									break;
							}
							break;
						case 7:
							if(words[2].ToUpper() != WHERE_COMMAND){
								Console.WriteLine("Incorrect WHERE word. Try use WHERE in third word");
								break;
							}
							bool successFilterv2 = false;
							words[3] = words[3].ToUpper();
                			if (FilterType.BETWEEN.ToString() == words[3]){
                    			successFilterv2 = true;
                			}
							if(!successFilterv2){
								Console.WriteLine("Incorrect filterType. Try BETWEEN or MATCH");
								break;
							}
							bool successOrdering = false;
							OrderType ordering = OrderType.ASC;
							words[6] = words[6].ToUpper();
							foreach (OrderType type in (OrderType[]) Enum.GetValues(typeof(OrderType))){
								string typeStr = type.ToString();
                				if (typeStr == words[6]){
                    				successOrdering = true;
                    				ordering = type;
                				}
            				}
							if(!successOrdering){
								Console.WriteLine("Incorrect orderType. Try ASC or DESC");
								break;
							}
							if(triesInCache.Exists(x => x.name == words[1])){
								switch(ordering){
								case OrderType.ASC:
									if(triesInCache.Exists(x => x.name == words[1])){
                        				triesInCache.Find(x => x.name == words[1]).GetBetweenWords(words[4].Trim(',').Trim('"'), words[5].Trim('"'), true);
									}
									break;
								case OrderType.DESC:
									if(triesInCache.Exists(x => x.name == words[1])){
                        				triesInCache.Find(x => x.name == words[1]).GetBetweenWords(words[4].Trim(',').Trim('"'), words[5].Trim('"'), false);
									}
									break;
								}
							} else{
								ErrorNoTrie();
							}
							break;
					}
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