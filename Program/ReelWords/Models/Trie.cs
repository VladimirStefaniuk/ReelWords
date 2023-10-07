using System;
using System.Collections.Generic;

namespace ReelWords.Models
{ 
    public class Trie : ITrie
    {
        private class TrieNode 
        { 
            public readonly Dictionary<char, TrieNode> ChildrenMap = new ();
            public bool IsWord { get; set; }
        }
        
        private readonly TrieNode _root;
        
        public Trie()
        {
            _root = new TrieNode();
        }
        public bool Search(string word)
        {
            var current = _root;
            foreach(var letter in word)
            {
                if(current.ChildrenMap.TryGetValue(letter, out var value))
                {
                    current = value;
                }
                else
                {
                    return false;
                }
            }
            return current.IsWord; 
        }

        public void Insert(string word)
        {
            var current = _root;
            foreach(var letter in word)
            {
                if(!current.ChildrenMap.ContainsKey(letter))
                {
                    current.ChildrenMap[letter] = new TrieNode();
                } 
                current = current.ChildrenMap[letter];
            }
            current.IsWord = true;
        }

        public void Delete(string word)
        {
            DeleteRecursively(_root, word, 0);
        }

        private bool DeleteRecursively(TrieNode node, string word, int depth)
        {
            if (node == null)
            {
                return false;
            }

            // Base case: reached the end of the word
            if (depth == word.Length)
            {
                if (!node.IsWord)
                {
                    Console.Error.WriteLine($"Can't delete {word} because it's not in the Trie");
                    return false;
                }
                node.IsWord = false;

                // Check if this node has no other children, and if so, it can be removed.
                return node.ChildrenMap.Count == 0;
            }

            char currentChar = word[depth];

            // Recursive case: traverse down the trie
            if (DeleteRecursively(node.ChildrenMap[currentChar], word, depth + 1))
            {
                node.ChildrenMap.Remove(currentChar);

                // Check if this node has no other children and is not a word, then it can be removed.
                return !node.IsWord && node.ChildrenMap.Count == 0;
            }

            return false;
        } 
    }
}