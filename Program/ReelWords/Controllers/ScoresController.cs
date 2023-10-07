using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReelWords.Helpers;

namespace ReelWords.Controllers
{
    public class ScoresController
    {
        private readonly string _scoresFilePath;
        private readonly string _scoresDirectoryPath;
        private readonly Dictionary<char, int> _scoresForLetter;

        public int TotalScoresCollected { get; private set; }

        public ScoresController(string scoresFilePath, string scoresDirectoryPath)
        {
            _scoresFilePath = scoresFilePath;
            _scoresDirectoryPath = scoresDirectoryPath;
            _scoresForLetter = new();
        }

        public async Task Initialize()
        {
            try
            {
                await FileReaderHelper.ReadFileAsync(
                    FileReaderHelper.GetPathToTheFileToRead(_scoresDirectoryPath, _scoresFilePath),
                    ParseScores);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error initializing scores: {ex.Message}");
                throw;
            }
        }
  
        private void ParseScores(string line)
        {
            string[] parts = line.Split(' ');

            if (parts.Length == 2)
            {
                if (char.TryParse(parts[0], out var letter))
                {
                    if (int.TryParse(parts[1], out var number))
                    {
                        // Successfully parsed the letter and number
                        _scoresForLetter[letter] = number;
                    }
                    else
                    {
                        throw new FormatException("Invalid number format for first character.");
                    }
                }
                else
                {
                    throw new FormatException("Invalid letter format for second character.");
                }
            }
            else
            {
                throw new FormatException($"Invalid input format. Lenght for parts is {parts.Length} but expected 2");
            }
        }
 
        /// <summary>
        /// Collects scores for word completion. Scores value pre-populated from the config text file. 
        /// </summary>
        /// <param name="word"></param>
        /// <returns> Scores value for completion a word </returns>
        public int GrantRewardForWordCompletion(string word)
        { 
            int scores = 0;
            foreach (var letter in word)
            {
                if (_scoresForLetter.TryGetValue(letter, out var scoreValue))
                {
                    scores += scoreValue;
                }
            }
            TotalScoresCollected += scores;
            return scores;
        }
        
        public void LogState()
        {
            foreach (var score in _scoresForLetter)
            {
                Console.WriteLine($"{score.Key} - {score.Value}");
            }
        }
    }
}