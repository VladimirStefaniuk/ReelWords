using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReelWords.Views
{
    public class GameConsoleView : IGameView
    {
        private readonly short _contentSeparatorLength = 64;
        private readonly string _contentSeparator;
        public GameConsoleView()
        {
            _contentSeparator = new string('=', _contentSeparatorLength);
        }
        public async Task ShowLoadingText(CancellationToken cancellationToken)
        {
            Console.Write("Loading");
            int dots = 0;

            while (true)
            {
                await Task.Delay(100, cancellationToken);
                Console.Write(".");
                dots++;
                if (dots == 3)
                {
                    Console.Write("\b\b\b   \b\b\b"); // Clear the three dots
                    dots = 0;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }   
    
        public void DrawHeader()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine()
                .AppendLine(_contentSeparator)
                .AppendLine("\tWelcome to ReelWords Game!")
                .AppendLine("Type `:exit` to stop the game")
                .AppendLine(_contentSeparator); 
            Console.Write(stringBuilder.ToString());
        }

        public void DrawReelsUI(char[] reels, int scores)
        { 
            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine()
                .AppendLine()
                .AppendLine($"Total Scores: {scores}")
                .AppendLine("Use letters from Reel to create a word: ")
                .AppendLine($"\t{string.Join(' ', reels)}")
                .AppendLine($"Please enter a word:");
            Console.Write(stringBuilder.ToString());
            Console.Write("> ");
        }

        public void PrintSuccessText(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green; 
            Console.WriteLine(message);  
            Console.ResetColor();
        }
    
        public void PrintFailText(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; 
            Console.WriteLine(message);  
            Console.ResetColor();
        }
    }
}