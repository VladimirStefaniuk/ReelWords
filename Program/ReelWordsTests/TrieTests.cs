using ReelWords;
using ReelWords.Models;
using Xunit;

namespace ReelWordsTests
{
    public class TrieTests
    {
        private const string TEST_WORD = "parallel";

        [Fact]
        public void TrieInsertTest()
        {
            ITrie trie = new Trie();
            trie.Insert(TEST_WORD);
            Assert.True(trie.Search(TEST_WORD));
        }

        [Fact]
        public void TrieDeleteTest()
        {
            ITrie trie = new Trie();
            trie.Insert(TEST_WORD);
            Assert.True(trie.Search(TEST_WORD));
            trie.Delete(TEST_WORD);
            Assert.False(trie.Search(TEST_WORD));
        }
        
        [Fact]
        public void DeleteWordWithCommonPrefix()
        {
            string shortWord = "Account";
            string longWord1 = "Accountable"; 
        
            ITrie trie = new Trie();
            trie.Insert(longWord1);
            Assert.True(trie.Search(longWord1));
        
            trie.Insert(shortWord);
            Assert.True(trie.Search(shortWord));
        
            trie.Delete(longWord1);
            Assert.False(trie.Search(longWord1));
            Assert.True(trie.Search(shortWord));
        }
    }
}