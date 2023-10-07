using System;
using System.IO;
using System.Threading.Tasks;

namespace ReelWords.Helpers
{
    public static class FileReaderHelper
    {
        private const string RootDirectory = "Program";
 
        public static string GetPathToTheFileToRead(string subDirectory, string fileName)
        {
            string executableDirectory = AppDomain.CurrentDomain.BaseDirectory; 
            var pathToTheRootDirectoryOfCodeChallenge = executableDirectory.Split(RootDirectory)[0];
            return Path.Combine(pathToTheRootDirectoryOfCodeChallenge, subDirectory, fileName);
        }

        public static async Task ReadFileAsync(string filePath, Action<string> onLineRead)
        { 
            if (!File.Exists(filePath))
            {
                var errorMessage = $"The file '{filePath}' was not found.";
                await Console.Error.WriteLineAsync(errorMessage);
                throw new FileNotFoundException(errorMessage);
            }
            
            await using FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            await using BufferedStream bs = new BufferedStream(fs);
            using StreamReader reader = new StreamReader(bs);
            
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                onLineRead.Invoke(line);
            }  
        }
    }
}