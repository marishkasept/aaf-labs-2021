using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ua.lab.oaa.Components{
    class Trie{
        public Node Root = new Node();
        public string name;
        public Trie(string newName) {
            name = newName;
			Root.IsWordList.Add(false);
        }

        public void PrintTrie(){
            Console.WriteLine(Root.ToString());
        }



        public void CompressNodes(Node currentNode){
            List<string> keysToRemove = new List<string>();
            Dictionary<string, Node> itemsToAdd = new Dictionary<string, Node>();
            foreach(var key in currentNode.Edges.Keys){
                Node nextNode;
                currentNode.Edges.TryGetValue(key, out nextNode);
                CompressNodes(nextNode);
                if(nextNode != null){
                    if(nextNode.Edges.Count == 1){
                        foreach(var nextKey in nextNode.Edges.Keys){
                            string newKey = string.Concat(key, nextKey);
                            Node newNode;
                            nextNode.Edges.TryGetValue(nextKey, out newNode);
							newNode.IsWordList.Insert(0, nextNode.IsWordList[0]);
                            itemsToAdd.Add(newKey, newNode);
                            keysToRemove.Add(key);
                        }

                    }
                }
            }

            foreach(var key in keysToRemove){
                currentNode.Edges.Remove(key);
            }

            itemsToAdd.ToList().ForEach(x => currentNode.Edges.Add(x.Key, x.Value));
        }

        public void UncompressNodes(Node currentNode){
            List<string> keysToRemove = new List<string>();
            Dictionary<string, Node> itemsToAdd = new Dictionary<string, Node>();
            foreach(var key in currentNode.Edges.Keys){
                Node nextNode;
                if(currentNode.Edges.TryGetValue(key, out nextNode)){
                    UncompressNodes(nextNode);
                }
                if(key.Length <= 1){
                    continue;
                }
                keysToRemove.Add(key);
                for(int i = 0; i < key.Length; i++) {
                    if (i == 0)
                    {
						var tempNode = new Node();
						tempNode.IsWordList.Add(nextNode.IsWordList[0]);
                        itemsToAdd.Add(key[0].ToString(), tempNode);
                        continue;
                    }
                    Node newNode = new Node();
                    string currentLetter = key[i].ToString();
                    int index = 0;
                    string currentKey = key[index].ToString();
                    Node newNextNode;
                    itemsToAdd.TryGetValue(currentKey, out newNode);
                    index++;
                    currentKey = key[index].ToString();
                    while (newNode.Edges.TryGetValue(currentKey, out newNextNode))
                    {
                        newNode = newNextNode;
                        index++;
                        currentKey = key[index].ToString();
                    }


                    if (i == key.Length - 1)
                    {
                        if (nextNode != null) {
							var lastIsWord = nextNode.IsWordList[nextNode.IsWordList.Count - 1];
							nextNode.IsWordList.Clear();
							nextNode.IsWordList.Add(lastIsWord);
                            newNode.Edges.Add(currentKey, nextNode);
                        } else {
							var node = new Node();
							node.IsWordList.Add(nextNode.IsWordList[i]);
                            newNode.Edges.Add(currentKey, node);
                        }
                    } else {
						var nodeNew = new Node();
						nodeNew.IsWordList.Add(nextNode.IsWordList[i]);
                        newNode.Edges.Add(currentKey, nodeNew);
                    }

                }
            }

            foreach(var key in keysToRemove){
                currentNode.Edges.Remove(key);
            }

            itemsToAdd.ToList().ForEach(x => currentNode.Edges.Add(x.Key, x.Value));
        }

        public bool ContainsWord(string word) {
            UncompressNodes(Root);
            Node node = Root;
            for(int i = 0; i < word.Length; i++){
                var letter = word[i];
                Node next;
                if (node.Edges.TryGetValue(letter.ToString(), out next)) {
                    node = next;
                } else {
                    CompressNodes(Root);
                    return false;
                }
				if(i == word.Length - 1){
					bool result = node.IsWordList[0];
					CompressNodes(Root);
            		return result;
				}
            }
            return true;
        }


        public void AddWord(string word){
            UncompressNodes(Root);
            var node = Root;
            for (int len = 0; len < word.Length; len++){
                var letter = word[len];
                Node next;
                if (!node.Edges.TryGetValue(letter.ToString(), out next))
                {
                    next = new Node();
                    node.Edges.Add(letter.ToString(), next);
                }
                node = next;
				next.IsWordList.Clear();
				if(len == word.Length - 1){
					next.IsWordList.Add(true);
				} else{
					next.IsWordList.Add(false);
				}
            }
            CompressNodes(Root);
        }

		public void PrintAllWords(bool isAsc){
			UncompressNodes(Root);
			List<string> resultWords = new List<string>();
			DFSPrint(Root, "", resultWords);
			if(isAsc){
				for(int i = 0; i < resultWords.Count; i++){
					Console.WriteLine(resultWords[i]);
				}
			} else{
				for(int i = resultWords.Count - 1; i >= 0; i--){
					Console.WriteLine(resultWords[i]);
				}
			}
			CompressNodes(Root);
		}

		private void DFSPrint(Node currentNode, string word, List<string> results){
        	Node next;
			if(currentNode.IsWordList[0]){
				results.Add(word);
			}
            foreach(var key in currentNode.Edges.Keys){
                Node nextNode;
                currentNode.Edges.TryGetValue(key, out nextNode);
				word = String.Concat(word, key);
				DFSPrint(nextNode, word, results);
				word = word.Remove(word.Length-1);
            }
       }

		public void GetRegexWord(string regex, bool isAsc){
			UncompressNodes(Root);
			List<string> answer = new List<string>();
			RegexFind(Root, regex, "", answer);
			List<string> result = new List<string>();
			foreach(var word in answer){
				if(!result.Contains(word)){
					result.Add(word);
				}
			}
			if(isAsc){
				for(int i = 0; i < result.Count; i++){
					Console.WriteLine(result[i]);
				}
			} else{
				for(int i = result.Count - 1; i >= 0; i--){
					Console.WriteLine(result[i]);
				}
			}
			CompressNodes(Root);
		}

		private void RegexFind(Node currentNode, string regexStr, string word, List<string> results){
			Node current = currentNode;
			Node next = currentNode;
			for(int i = 0; i < regexStr.Length; i++){
				if(next != null){
					current = next;
				}

				switch(regexStr[i]){
					case '?':
						foreach(var key in current.Edges.Keys){
							Node temp;
							if(current.Edges.TryGetValue(key, out temp)){
								word = String.Concat(word, key);
								RegexFind(temp, regexStr, word, results);
								word = word.Remove(word.Length-1);
							}
						}
						break;
					case '*':
						DFSPrint(current, word, results);
						break;
					default:
						if(current.Edges.TryGetValue(regexStr[i].ToString(), out next)){
							word = String.Concat(word, regexStr[i]);
						}
						break;
				}
				if(i == regexStr.Length - 1){
					if(next != null && next.IsWordList[0]){
						results.Add(word);
					}
				}
			}

		}

		public void GetBetweenWords(string from, string to, bool isAsc){
			UncompressNodes(Root);
			List<string> resultWords = new List<string>();
			DFSPrint(Root, "", resultWords);
			List<string> answer = new List<string>();
			foreach(var word in resultWords){
				if(String.Compare(word, from) != -1 && String.Compare(word, to) != 1){
					answer.Add(word);
				}
			}
			if(isAsc){
				for(int i = 0; i < answer.Count; i++){
					Console.WriteLine(answer[i]);
				}
			} else{
				for(int i = answer.Count - 1; i >= 0; i--){
					Console.WriteLine(answer[i]);
				}
			}
			CompressNodes(Root);
		}



    }
}