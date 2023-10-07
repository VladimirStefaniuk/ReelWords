using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReelWords.Helpers;
using ReelWords.Models;

namespace ReelWords.Controllers
{ 
    public class ReelsController
    {
        private readonly string _reelsFile;
        private readonly string _reelsParentDirectory;
        private readonly List<Reel> _reels;
        private readonly Random _random;

        public ReelsController(string reelsFile, string reelsParentDirectory, int randomReelsSeed = -1)
        {
            _reelsFile = reelsFile;
            _reelsParentDirectory = reelsParentDirectory;
            _reels = new List<Reel>(); 
            _random = new Random(randomReelsSeed == -1 ? Environment.TickCount : randomReelsSeed);
        }
 
        public async Task Initialize()
        {
            try
            {
                var reelsFilePath = FileReaderHelper.GetPathToTheFileToRead(_reelsParentDirectory, _reelsFile);
                await FileReaderHelper.ReadFileAsync(reelsFilePath, ParseReels);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error initializing reels: {ex.Message}");
                throw;
            }
        }
 
        private void ParseReels(string line)
        {
            string[] parts = line.Split(' ');
            List<char> letters = new();

            foreach (var part in parts)
            {
                if (char.TryParse(part, out var letter))
                {
                    letters.Add(letter);
                }
                else
                {
                    var error = "Invalid letter format.";
                    Console.Error.WriteLine(error);
                    throw new FormatException(error);
                }
            }
  
            int randomIndex = _random.Next(0, letters.Count); 
            _reels.Add(new Reel(letters, randomIndex));
        }
  
        public char[] GetCurrentReels()
        {
            return _reels.Select(reel => reel.CurrentLetter).ToArray();
        }

        public bool CanFormWordFromReels(string word)
        {
            if (string.IsNullOrEmpty(word) || _reels.Count == 0)
                return false;
            
            var reels = GetCurrentReels();
            
            // words needs more letters than in reals 
            if(word.Length > reels.Length)
                return false;

            var letterCountForReels = _reels.GroupBy(r => r.CurrentLetter)
                .ToDictionary(g => g.Key, g => g.Count());
 
            foreach (var wordLetter in word)
            {
                if(!letterCountForReels.ContainsKey(wordLetter))
                    // no letter found in the count for reels, can't create a word
                    return false;
                letterCountForReels[wordLetter]--;
            }

            return letterCountForReels.All(l => l.Value >= 0);
        }

        public void ProcessWord(string word)
        {
            var reelsToUpdate = new List<Reel>(_reels);
            foreach (var letter in word)
            {
                foreach (var reel in reelsToUpdate)
                {
                    if (reel.CurrentLetter == letter)
                    {
                        reel.Increment();
                        reelsToUpdate.Remove(reel);
                        break;
                    }
                }
            }
        }
    }
}