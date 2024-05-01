

using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Entity.Location;
using OregonTrailDotNet.Entity.Location.Point;
using OregonTrailDotNet.Window.Travel.Hunt;
using OregonTrailDotNet.Window.Travel.RiverCrossing;
using OregonTrailDotNet.Window.Travel.Store;
using OregonTrailDotNet.Window.Travel.Toll;
using WolfCurses.Utility;
using WolfCurses.Window;

namespace OregonTrailDotNet.Window.Travel
{
    
    public sealed class TravelInfo : WindowData
    {
      
        public TravelInfo()
        {
            // Store so player can buy food, clothes, ammo, etc.
            Store = new StoreGenerator();
        }

      
        public RiverGenerator River { get; private set; }

     
        public StoreGenerator Store { get; }

       
        public HuntManager Hunt { get; private set; }

        
        public TollGenerator Toll { get; private set; }

      
        public static string DriveStatus
        {
            get
            {
                // Grab instance of game simulation.
                var game = GameSimulationApp.Instance;

                // GetModule the current food item from vehicle inventory.
                var foodItem = game.Vehicle.Inventory[Entities.Food];

                // Set default food status text, update to actual food item total weight if it exists.
                var foodStatus = "0 pounds";
                if (foodItem != null)
                    foodStatus = $"{foodItem.TotalWeight} pounds";

                // Build up the status for the vehicle as it moves through the simulation.
                var driveStatus = new StringBuilder();
                driveStatus.AppendLine("--------------------------------");
                driveStatus.AppendLine($"Date: {game.Time.Date}");
                driveStatus.AppendLine(
                    $"Weather: {game.Trail.CurrentLocation.Weather.ToDescriptionAttribute()}");
                driveStatus.AppendLine($"Health: {game.Vehicle.PassengerHealthStatus.ToDescriptionAttribute()}");
                driveStatus.AppendLine($"Food: {foodStatus}");
                driveStatus.AppendLine($"Next landmark: {game.Trail.DistanceToNextLocation} miles");
                driveStatus.AppendLine($"Miles traveled: {game.Vehicle.Odometer} miles");
                driveStatus.AppendLine("--------------------------------");
                return driveStatus.ToString();
            }
        }

       
        public int DaysToRest { get; internal set; }

      
        public static string TravelStatus
        {
            get
            {
                // Grab instance of game simulation.
                var game = GameSimulationApp.Instance;

                var showLocationName = game.Trail.CurrentLocation.Status == LocationStatus.Arrived;
                var locationStatus = new StringBuilder();
                locationStatus.AppendLine("--------------------------------");

                // Only add the location name if we are on the next point, otherwise we should not show this.
                locationStatus.AppendLine(showLocationName
                    ? game.Trail.CurrentLocation.Name
                    : $"{game.Trail.DistanceToNextLocation:N0} miles to {game.Trail.NextLocation.Name}");

                locationStatus.AppendLine($"{game.Time.Date}");
                locationStatus.AppendLine("--------------------------------");
                locationStatus.AppendLine(
                    $"Weather: {game.Trail.CurrentLocation.Weather.ToDescriptionAttribute()}");
                locationStatus.AppendLine($"Health: {game.Vehicle.PassengerHealthStatus.ToDescriptionAttribute()}");
                locationStatus.AppendLine($"Pace: {game.Vehicle.Pace.ToDescriptionAttribute()}");
                locationStatus.AppendLine($"Rations: {game.Vehicle.Ration.ToDescriptionAttribute()}");
                locationStatus.AppendLine("--------------------------------");
                return locationStatus.ToString();
            }
        }

        
        public void GenerateHunt()
        {
            if (Hunt != null)
                return;

            Hunt = new HuntManager();
        }

       
        public void DestroyHunt()
        {
            if (Hunt == null)
                return;

            Hunt = null;
        }

        public void GenerateToll(TollRoad tollRoad)
        {
            if (Toll != null)
                return;

            Toll = new TollGenerator(tollRoad);
        }

     
        public void DestroyToll()
        {
            if (Toll == null)
                return;

            Toll = null;
        }

        public void GenerateRiver()
        {
            // Skip if river has already been created.
            if (River != null)
                return;

            // Creates a new river.
            River = new RiverGenerator();
        }

   
      
        public void DestroyRiver()
        {
            // Skip if the river is already null.
            if (River == null)
                return;

            // Destroy the river data.
            River = null;
        }
    }
}