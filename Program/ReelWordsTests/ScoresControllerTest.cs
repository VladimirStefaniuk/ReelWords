using ReelWords;
using ReelWords.Controllers;
using Xunit;

namespace ReelWordsTests;

public class ScoresControllerTest
{
    private const string ResourcesDirectory = "Resources"; 
    private const string ScoresFileName = "scores.txt";
    
    [Fact]
    public async void AwardingScoresForWordAndTotalScoresTest()
    {
        var scoresController = new ScoresController(ScoresFileName, ResourcesDirectory);
        await scoresController.Initialize();

        Assert.True(scoresController.GrantRewardForWordCompletion("a") == 1);
        Assert.True(scoresController.TotalScoresCollected == 1);
        
        Assert.True(scoresController.GrantRewardForWordCompletion("b") == 3);
        Assert.True(scoresController.TotalScoresCollected == (1 + 3));
        
        Assert.True(scoresController.GrantRewardForWordCompletion("b") == 3);
        Assert.True(scoresController.TotalScoresCollected == (1 + 3 + 3));

        int scoresForWordScopely = (1 + 3 + 1 + 3 + 1 + 1 + 4);
        Assert.True(scoresController.GrantRewardForWordCompletion("scopely") == scoresForWordScopely);
        Assert.True(scoresController.TotalScoresCollected == (1 + 3 + 3) + scoresForWordScopely);
    }
}