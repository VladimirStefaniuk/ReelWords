using System;
using System.Collections.Generic;  
using System.Threading;
using System.Threading.Tasks;
using ReelWords.Controllers;
using ReelWords.Models;
using ReelWords.Views;

namespace ReelWords
{
    public static class Program
    {
        private static IGameView _gameView;
        private static ScoresController _scoresController;
        private static ReelsController _reelsController;
        private static WordSearchController _wordSearchController;

        private const string ResourcesDirectory = "Resources";
        private const string ScoresFileName = "scores.txt";
        private const string ReelsFileName = "reels.txt";
        private const string WordsFileName = "american-english-large.txt";
 
        static async Task Main(string[] args)
        {
            InitializeComponents();

            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken loadingTextCancellationToken = cancellationTokenSource.Token;
                var loadingTextTask = _gameView.ShowLoadingText(loadingTextCancellationToken);
 
                await InitializeControllersAsync();
 
                cancellationTokenSource.Cancel();
                await loadingTextTask;
            }
            catch (TaskCanceledException)
            { 
            }
            catch (Exception e)
            {
                HandleInitializationError(e);
                return;
            }

            _gameView.DrawHeader(); 
     
            bool playing = true; 
            while (playing)
            {
                PlayGame(ref playing);
            }
            Console.ReadLine();
        } 
        
        private static void InitializeComponents()
        {
            _gameView = new GameConsoleView();
            _scoresController = new ScoresController(ScoresFileName, ResourcesDirectory);
            _reelsController = new ReelsController(ReelsFileName, ResourcesDirectory);
            _wordSearchController = new WordSearchController(WordsFileName, ResourcesDirectory, new Trie());
        } 
        
        private static async Task InitializeControllersAsync()
        {
            await Task.WhenAll(
                _scoresController.Initialize(),
                _reelsController.Initialize(),
                _wordSearchController.Initialize()
            );
        }
        
        private static void HandleInitializationError(Exception exception)
        {
            _gameView.PrintFailText($"Critical error: {exception}");
        }

        private static void PlayGame(ref bool playing)
        { 
            var currentReels = _reelsController.GetCurrentReels();
            _gameView.DrawReelsUI(currentReels, _scoresController.TotalScoresCollected);
                
            string input = Console.ReadLine();
            if (!ValidateInput(input))
            {
                _gameView.PrintFailText("Invalid input. Please try again.");
                return;
            }
            
            if (input.ToLower() == ":exit")
            {
                playing = false; 
                _gameView.PrintSuccessText($"Thanks for playing. Your score is {_scoresController.TotalScoresCollected}");
                return;
            }

            if (_reelsController.CanFormWordFromReels(input))
            {
                if (_wordSearchController.HasWord(input))
                {
                    var scores = _scoresController.GrantRewardForWordCompletion(input);
                    _reelsController.ProcessWord(input); 
                    _gameView.PrintSuccessText($"\tGreat, you entered correct word!" +
                                               $"\n + {scores} Scores!");
                }
                else
                {
                    _gameView.PrintFailText($"\t{input} is not correct word, try again");
                }
            }
            else
            {
                _gameView.PrintFailText($"\tThe word '{input}' cannot be formed with the letters in Reel " +
                                        $": '{string.Join(' ', currentReels)}'"); 
            }
        }

        private static bool ValidateInput(string input)
        { 
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}