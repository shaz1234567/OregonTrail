

using System;

namespace OregonTrailDotNet.Window.Travel.RiverCrossing
{
    
    public sealed class RiverGenerator
    {
       
        public RiverGenerator()
        {
            // Grab instance of the game simulation.
            var game = GameSimulationApp.Instance;

            // Cast the current location as river crossing.
            var riverLocation = game.Trail.CurrentLocation as Entity.Location.Point.RiverCrossing;
            if (riverLocation == null)
                throw new InvalidCastException(
                    "Unable to cast location as river crossing even though it returns as one!");

            // Randomly generates statistics about the river each time you cross it.
            RiverDepth = game.Random.Next(1, 20);

            // Determines how long the player will spend crossing river.
            RiverWidth = game.Random.Next(100, 1500);

            // Determines how the player will want to cross the river.
            CrossingType = RiverCrossChoice.None;

            // Only setup ferry cost and delay if this is that type of crossing.
            switch (riverLocation.RiverCrossOption)
            {
                case RiverOption.FerryOperator:
                    IndianCost = 0;
                    FerryCost = game.Random.Next(3, 8);
                    FerryDelayInDays = game.Random.Next(1, 10);
                    break;
                case RiverOption.FloatAndFord:
                    IndianCost = 0;
                    FerryCost = 0;
                    FerryDelayInDays = 0;
                    break;
                case RiverOption.IndianGuide:
                    IndianCost = game.Random.Next(3, 8);
                    FerryCost = 0;
                    FerryDelayInDays = 0;
                    break;
                case RiverOption.None:
                    throw new ArgumentException(
                        "Unable to generate river without having options configured to some value other than NONE!");
                default:
                    throw new ArgumentException(
                        "Unable to figure out what the river option value should be! Check value being sent to river generator class!");
            }
        }

       
        public bool DisasterHappened { get; set; }

      
        public RiverCrossChoice CrossingType { get; set; }

        
        public int RiverDepth { get; }

      
        public float FerryCost { get; set; }

        
     
        public int FerryDelayInDays { get; set; }

       
        public int RiverWidth { get; }

      
        public int IndianCost { get; set; }
    }
}