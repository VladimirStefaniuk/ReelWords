using System.Threading;
using System.Threading.Tasks;

namespace ReelWords.Views;

public interface IGameView
{
    Task ShowLoadingText(CancellationToken cancellationToken);
    void DrawHeader();
    void DrawReelsUI(char[] reels, int scores);
    void PrintSuccessText(string message);
    void PrintFailText(string message);
}
