

using WolfCurses.Utility;

namespace OregonTrailDotNet.Module.Scoring
{
    
    public sealed class Highscore
    {
      
        private readonly Performance _rating;

   
        public Highscore(string name, int points)
        {
            // PassengerLeader of party and total number of points.
            Name = name;
            Points = points;

            // Rank the players performance based on the number of points they have.
            if (points >= 7000)
                _rating = Performance.TrailGuide;
            else if ((points >= 3000) && (points < 7000))
                _rating = Performance.Adventurer;
            else if (points < 3000)
                _rating = Performance.Greenhorn;
        }

      
        public string Name { get; }

       
        public int Points { get; }

      
        public string Rating => _rating.ToDescriptionAttribute();
    }
}