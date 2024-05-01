

using OregonTrailDotNet.Entity.Person;

namespace OregonTrailDotNet.Module.Scoring
{
   
    public sealed class HealthScoring
    {
        
        
        public HealthScoring(HealthStatus partyHealthStatus, int pointsPerPerson)
        {
            PartyHealthStatus = partyHealthStatus;
            PointsPerPerson = pointsPerPerson;
        }

       
        public HealthStatus PartyHealthStatus { get; }

        
        public int PointsPerPerson { get; }
    }
}