

using System;
using OregonTrailDotNet.Entity.Location.Weather;

namespace OregonTrailDotNet.Entity.Location
{

    public abstract class Location : IEntity
    {
       
        private readonly LocationWeather _weather;

       
        protected Location(string name, Climate climateType)
        {
            // Default warning message for the location is based on fresh water status.
            Warning = GameSimulationApp.Instance.Random.NextBool() ? LocationWarning.None : LocationWarning.BadWater;

            // Creates a new system to deal with the management of the weather for this given location.
            _weather = new LocationWeather(climateType);

            // Name of the point as it should be known to the player.
            Name = name;

            // Default location status is not visited by the player or vehicle.
            Status = LocationStatus.Unreached;
        }

        public int Depth { get; set; }

     
        public LocationWarning Warning { get; }

       
        public Weather.Weather Weather => _weather.Condition;

      
        public abstract bool ChattingAllowed { get; }

       
      
        public abstract bool ShoppingAllowed { get; }

       
        public LocationStatus Status { get; set; }

        
        public bool ArrivalFlag { get; set; }

        
        public bool LastLocation { get; set; }


        public int TotalDistance { get; set; }

       
        public string Name { get; }

      
        
        public int Compare(IEntity x, IEntity y)
        {
            var result = string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
            if (result != 0) return result;

            return result;
        }

        
        public int CompareTo(IEntity other)
        {
            var result = string.Compare(other.Name, Name, StringComparison.Ordinal);
            if (result != 0) return result;

            return result;
        }

       
        public bool Equals(IEntity other)
        {
            // Reference equality check
            if (this == other)
                return true;

            if (other == null)
                return false;

            if (other.GetType() != GetType())
                return false;

            if (Name.Equals(other.Name))
                return true;

            return false;
        }

        public bool Equals(IEntity x, IEntity y)
        {
            return x.Equals(y);
        }

     
        public int GetHashCode(IEntity obj)
        {
            var hash = 23;
            hash = hash*31 + Name.GetHashCode();
            return hash;
        }

     
        public void OnTick(bool systemTick, bool skipDay)
        {
            // Skip system ticks.
            if (systemTick)
                return;

            // Weather will only be ticked when not skipping a day.
            if (!skipDay)
                _weather.Tick();
        }
    }
}