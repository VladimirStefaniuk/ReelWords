using System.Collections.Generic;
using System.Linq;

namespace ReelWords.Models
{
    public class Reel
    {
        private readonly List<char> _letters;
        private int _currentIndex;

        public char CurrentLetter => _letters[_currentIndex];
        public Reel(List<char> letters, int initialIndex)
        {
            _currentIndex = initialIndex;
            _letters = letters;
        }

        public void Increment()
        {
            if (!_letters.Any())
                return;
            _currentIndex = (++_currentIndex) % _letters.Count;
        }
    }
}