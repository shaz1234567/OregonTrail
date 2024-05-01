

using System.Collections.Generic;
using System.Linq;

namespace OregonTrailDotNet.Module.Scoring
{
   
    public sealed class ScoringModule : WolfCurses.Module.Module
    {
        
        private List<Highscore> _highScores;

       
        public ScoringModule()
        {
            Reset();
        }

    
        public IEnumerable<Highscore> TopTen
        {
            get { return _highScores.OrderByDescending(x => x.Points).Take(10); }
        }

        public static IEnumerable<Highscore> DefaultTopTen => new List<Highscore>
        {
            new Highscore("Stephen Meek", 7650),
            new Highscore("Celinda Hines", 5694),
            new Highscore("Andrew Sublette", 4138),
            new Highscore("David Hastings", 2945),
            new Highscore("Ezra Meeker", 2052),
            new Highscore("Willian Vaughn", 1401),
            new Highscore("Mary Bartlett", 937),
            new Highscore("Willian Wiggins", 615),
            new Highscore("Charles Hopper", 396),
            new Highscore("Elijah White", 250)
        };

        public void Add(Highscore score)
        {
            _highScores.Add(score);
        }

      
        public override void Destroy()
        {
            base.Destroy();

            // TODO: Save the high score list as JSON before it is destroyed.

            // Destroyed the high score list.
            _highScores = null;
        }

        /// <summary>
        ///     Makes the top ten list reset to the original top ten hard-coded defaults.
        /// </summary>
        public void Reset()
        {
            _highScores = new List<Highscore>(DefaultTopTen);

            // TODO: Load custom list from JSON with user high scores altered from defaults.
        }
    }
}