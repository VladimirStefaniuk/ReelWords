using ReelWords.Controllers;
using ReelWords.Models;
using Xunit;

namespace ReelWordsTests;

public class WordsSearchControllerTest
{
    private const string ResourcesDirectory = "Resources";  
    private const string WordsFileName = "american-english-large.txt";
    
    [Fact]
    public async void WordSearchCanFindWordsTest()
    {
        var wordSearchController = new WordSearchController(WordsFileName, ResourcesDirectory, new Trie());
        await wordSearchController.Initialize();

        Assert.True(wordSearchController.HasWord("volleys"));
        Assert.True(wordSearchController.HasWord("a"));
        Assert.True(wordSearchController.HasWord("crypt's"));
        Assert.True(wordSearchController.HasWord("dah")); 
    }
    
    [Fact]
    public async void WordSearchReturnFalseForWrongWordsTest()
    {
        var wordSearchController = new WordSearchController(WordsFileName, ResourcesDirectory, new Trie());
        await wordSearchController.Initialize();

        Assert.False(wordSearchController.HasWord("impossibleword")); 
        Assert.False(wordSearchController.HasWord("catdog")); 
        Assert.False(wordSearchController.HasWord("apple and pie")); 
        Assert.False(wordSearchController.HasWord(" ")); 
    }
}