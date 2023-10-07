using System;
using System.Threading.Tasks;
using ReelWords.Helpers;
using ReelWords.Models;

namespace ReelWords.Controllers
{
    public class WordSearchController
    { 
        private readonly string _wordsFilePath;
        private readonly string _parentDirectoryPath;
        private readonly ITrie _trie;

        public WordSearchController(string wordsFilePath, string parentDirectoryPath, ITrie trie)
        {
            _wordsFilePath = wordsFilePath;
            _parentDirectoryPath = parentDirectoryPath;
            _trie = trie;
        }
 
        public async Task Initialize()
        {
            try
            {
                await FileReaderHelper.ReadFileAsync(FileReaderHelper.GetPathToTheFileToRead(_parentDirectoryPath, _wordsFilePath), ParseReels); 
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error initializing words: {ex.Message}");
                throw;
            } 
        }

        private void ParseReels(string line)
        { 
            _trie.Insert(line.Trim());
        }
  
        public bool HasWord(string reel)
        {
            return _trie.Search(reel);
        }
    }
}