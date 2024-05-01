

using System;
using System.Collections.Generic;
using System.Text;
using OregonTrailDotNet.Entity.Person;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Module.Scoring;
using OregonTrailDotNet.Module.Time;
using OregonTrailDotNet.Module.Tombstone;
using OregonTrailDotNet.Module.Trail;
using OregonTrailDotNet.Window.GameOver;
using OregonTrailDotNet.Window.Graveyard;
using OregonTrailDotNet.Window.MainMenu;
using OregonTrailDotNet.Window.RandomEvent;
using OregonTrailDotNet.Window.Travel;
using WolfCurses;

namespace OregonTrailDotNet
{
    
    public class GameSimulationApp : SimulationApp
    {
    
        public const int MAXPLAYERS = 4;

       
        public TrailModule Trail { get; private set; }

      
        public static GameSimulationApp Instance { get; private set; }

      
        public TimeModule Time { get; private set; }

        public EventDirectorModule EventDirector { get; private set; }

      
        public Vehicle Vehicle { get; private set; }

        public int TotalTurns { get; private set; }

      
        public ScoringModule Scoring { get; private set; }

        public TombstoneModule Tombstone { get; private set; }

        
        public override IEnumerable<Type> AllowedWindows
        {
            get
            {
                var windowList = new List<Type>
                {
                    typeof(Travel),
                    typeof(MainMenu),
                    typeof(RandomEvent),
                    typeof(Graveyard),
                    typeof(GameOver)
                };

                return windowList;
            }
        }

      
        public void TakeTurn(bool skipDay)
        {
            // Advance the turn counter if we are not skipping days.
            if (!skipDay)
                TotalTurns++;

            // Let the modules of the game simulation decide how they want to deal with skip time turn.
            Time.TickTime(skipDay);
        }

        
        internal void SetStartInfo(NewGameInfo startingInfo)
        {
            // Clear out any data amount items, monies, people that might have been in the vehicle.
            Vehicle.ResetVehicle(startingInfo.StartingMonies);

            // Add all the player data we collected from attached game Windows states.
            var crewNumber = 1;
            foreach (var name in startingInfo.PlayerNames)
            {
                // First name in list is always the leader.
                var personLeader = (startingInfo.PlayerNames.IndexOf(name) == 0) && (crewNumber == 1);
                Vehicle.AddPerson(new Person(startingInfo.PlayerProfession, name, personLeader));
                crewNumber++;
            }

            // Set the starting month to match what the user selected.
            Time.SetMonth(startingInfo.StartingMonth);
        }

        public static void Create()
        {
            if (Instance != null)
                throw new InvalidOperationException(
                    "Unable to create new instance of game simulation since it already exists!");

            Instance = new GameSimulationApp();
            Instance.OnPostCreate();
        }

    
        private void OnPostCreate()
        {
            Scoring = new ScoringModule();

            // Allows for other players to see deaths of previous players on the trail.
            Tombstone = new TombstoneModule();
        }

     
        protected override void OnPreDestroy()
        {
            // Notify modules of impending doom allowing them to save data.
            Scoring.Destroy();
            Tombstone.Destroy();
            Time.Destroy();
            EventDirector.Destroy();
            Trail.Destroy();

            // Null the destroyed instances.
            Scoring = null;
            Tombstone = null;
            Time = null;
            EventDirector = null;
            Trail = null;
            TotalTurns = 0;
            Vehicle = null;

            // Destroys game simulation instance.
            Instance = null;
        }

        public override string OnPreRender()
        {
            // Total number of turns that have passed in the simulation.
            var tui = new StringBuilder();
            tui.AppendLine($"Turns: {TotalTurns:D4}");

            // Vehicle and location status.
            tui.AppendLine($"Vehicle: {Vehicle?.Status} - Location:{Trail?.CurrentLocation?.Status}");
            return tui.ToString();
        }

       
        protected override void OnFirstTick()
        {
            Restart();
        }

        
        public override void Restart()
        {
            // Reset turn counter back to zero.
            TotalTurns = 0;

            // Linear time simulation (should tick first).
            Time = new TimeModule();

            // Vehicle, weather, conditions, climate, tail, stats, event director, etc.
            EventDirector = new EventDirectorModule();
            Trail = new TrailModule();
            Vehicle = new Vehicle();

            // Resets the window manager in the base simulation.
            base.Restart();

            // Attach traveling Windows since that is the default and bottom most game Windows.
            WindowManager.Add(typeof(Travel));

            // Add the new game configuration screen that asks for names, profession, and lets user buy initial items.
            WindowManager.Add(typeof(MainMenu));
        }
    }
}