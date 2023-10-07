using System.Linq;
using ReelWords;
using ReelWords.Controllers;
using Xunit;

namespace ReelWordsTests;

public class ReelsControllerTest
{
    private const string _resourcesDirectory = "Resources"; 
    private const string _reelsFile = "reels.txt";

    private const int LengthOfReel = 7;
    
    /// <summary>
    /// Reels should recycle, so after N - 7 iterations reels should return to initial state
    /// </summary>
    [Fact]
    public async void ReelCharactersRecycleTest()
    {
        var reelsController = new ReelsController(_reelsFile, _resourcesDirectory);
        await reelsController.Initialize();
 
        var initialReels = reelsController.GetCurrentReels();
        var currentReels = initialReels;
        
        for (int i = 0; i < LengthOfReel; i++)
        {
            reelsController.ProcessWord(new string(currentReels)); 
            currentReels = reelsController.GetCurrentReels(); 
        }
        
        Assert.True(initialReels.SequenceEqual(currentReels));
    } 
    
    /// <summary>
    /// Reels should start at random position, but since we can provide random seed to the Reels Controller,
    /// we should get same result for the same random seed
    /// </summary>
    [Fact]
    public async void ReeksRandomizationTestIfRandomReelPositionIsDeterministic()
    { 
            var reelsController1 = new ReelsController(_reelsFile, _resourcesDirectory, 1);
            await reelsController1.Initialize(); 
            var initialReels1 = reelsController1.GetCurrentReels();
        
            var reelsController2 = new ReelsController(_reelsFile, _resourcesDirectory, 1);
            await reelsController2.Initialize(); 
            var initialReels2 = reelsController1.GetCurrentReels(); 
             
            Assert.True(initialReels1.SequenceEqual(initialReels2)); 
    } 
    
    /// <summary>
    /// Reels should start at random position, 
    /// and we should get different result by providing different seeds
    /// </summary>
    [Fact]
    public async void ReeksRandomizationTestDifferentSeedGivesRandomInitialState()
    { 
        var reelsController1 = new ReelsController(_reelsFile, _resourcesDirectory, 1);
        await reelsController1.Initialize(); 
        var initialReels1 = reelsController1.GetCurrentReels();
        
        var reelsController2 = new ReelsController(_reelsFile, _resourcesDirectory, 2);
        await reelsController2.Initialize(); 
        var initialReels2 = reelsController2.GetCurrentReels(); 
        
        // also test default parameters
        var reelsController3 = new ReelsController(_reelsFile, _resourcesDirectory);
        await reelsController3.Initialize(); 
        var initialReels3 = reelsController3.GetCurrentReels(); 
             
        Assert.False(initialReels1.SequenceEqual(initialReels2)); 
        Assert.False(initialReels1.SequenceEqual(initialReels3)); 
    } 
}