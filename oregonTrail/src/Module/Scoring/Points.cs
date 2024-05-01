

using System;
using OregonTrailDotNet.Entity.Item;

namespace OregonTrailDotNet.Module.Scoring
{
    
    public sealed class Points
    {
        
        private const string DEFAULT_DISPLAY_NAME = "";

      
        private readonly string _optionalDisplayName;

        
        private readonly int _perAmount;

     
        public Points(SimItem resource, string optionalDisplayName = DEFAULT_DISPLAY_NAME)
        {
            // Complain if the per amount is zero, the developer is doing it wrong.
            if (resource.PointsPerAmount <= 0)
                throw new ArgumentException("Per amount is less than zero, default value is one for a reason!");

            // Setup point tabulator basics.
            Resource = resource;
            PointsAwarded = resource.PointsAwarded;
            _optionalDisplayName = optionalDisplayName;
            _perAmount = resource.PointsPerAmount;
        }

      
        public int PointsAwarded { get; }

      
        private SimItem Resource { get; }

       
        public override string ToString()
        {
            // Check if optional display name is being used.
            var displayName = Resource.Name;
            if (!string.IsNullOrEmpty(_optionalDisplayName) &&
                !string.IsNullOrWhiteSpace(_optionalDisplayName))
                displayName = _optionalDisplayName;

            // Check if per amount is default value of one.
            return _perAmount == 1
                ? $"{displayName}"
                : $"{displayName} (per {_perAmount})";
        }
    }
}