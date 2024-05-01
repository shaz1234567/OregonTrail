

using System;

namespace OregonTrailDotNet.Entity.Item
{
    
    public sealed class SimItem : IEntity
    {
        
        public SimItem(
            Entities category,
            string name,
            string pluralForm,
            string delineatingUnit,
            int maxQuantity,
            float cost,
            int weight = 1,
            int minimumQuantity = 1,
            int startingQuantity = 0,
            int pointsAwarded = 1,
            int pointsPerAmount = 1)
        {
            
            if (minimumQuantity <= 0)
                throw new ArgumentException(
                    "Cannot set minimum quantity of an SimItem to be zero, you cannot have nothing of something!");

            // Setup quantity based on minimum amount.
            StartingQuantity = startingQuantity;
            MinQuantity = minimumQuantity;
            MaxQuantity = maxQuantity;
            Quantity = startingQuantity;

            // Scoring information for points tabulation if player wins the game.
            PointsPerAmount = pointsPerAmount;
            PointsAwarded = pointsAwarded;

           
            if (PointsPerAmount <= 0)
                PointsPerAmount = 1;

            Category = category;
            Name = name;
            PluralForm = pluralForm;
            DelineatingUnit = delineatingUnit;

          
            Cost = cost;

            // Weight of the SimItem, traditionally this was done in pounds.
            Weight = weight;
        }

        public SimItem(SimItem oldItem, int newQuantity)
        {
            // Check that new quantity is greater than ceiling.
            if (newQuantity > oldItem.MaxQuantity)
                newQuantity = oldItem.MaxQuantity;

            // Check that new quantity is not less than floor.
            if (newQuantity < oldItem.MinQuantity)
                newQuantity = oldItem.MinQuantity;

            // Set updated quantity values, plus ceiling and floor.
            Quantity = newQuantity;
            MinQuantity = oldItem.MinQuantity;
            MaxQuantity = oldItem.MaxQuantity;
            StartingQuantity = oldItem.StartingQuantity;

            // Scoring information for points tabulation if player wins the game.
            PointsPerAmount = oldItem.PointsPerAmount;
            PointsAwarded = oldItem.PointsAwarded;

            // Ensure the values for points per amount are not zero.
            if (PointsPerAmount <= 0)
                PointsPerAmount = 1;

            // Display name and SimItem entity type.
            Name = oldItem.Name;
            Category = oldItem.Category;
            Cost = oldItem.Cost;
            DelineatingUnit = oldItem.DelineatingUnit;
            PluralForm = oldItem.PluralForm;
            Weight = oldItem.Weight;
        }


        public int Points
        {
            get
            {
                // Check quantity is above zero.
                if (Quantity <= 0)
                    return 0;

                // Check that quantity is above divisor for point calculation.
                if (Quantity < PointsPerAmount)
                    return 0;

                // Figure out how many points for this quantity.
                var points = Quantity/PointsPerAmount*PointsAwarded;

                // Return the result to the caller.
                return points;
            }
        }

       
        public int MinQuantity { get; }

      
        public int Quantity { get; private set; }

      
        public float Cost { get; }

        public string DelineatingUnit { get; }

       
        public string PluralForm { get; }

      
        private int Weight { get; }

      
        private int StartingQuantity { get; }

    
        public int PointsAwarded { get; }

      
        public int PointsPerAmount { get; }

     
        public int TotalWeight => Weight*Quantity;

     
        public float TotalValue => Cost*Quantity;

      
        public int MaxQuantity { get; }

        
        public Entities Category { get; }

        
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
            // Nothing to see here, move along...
        }

        public void Reset()
        {
            Quantity = StartingQuantity;

            
        }

   
        public string ToString(bool storeMode)
        {
            return !storeMode
                ? $"{Cost:F2} per {DelineatingUnit}"
                : (Quantity*Cost).ToString("C2");
        }

      
        public override string ToString()
        {
            return ToString(false);
        }

   
        public void ReduceQuantity(int amount)
        {
            // Subtract the amount from the quantity.
            var simulatedSubtraction = Quantity - amount;

            // Check that amount is not below minimum floor.
            if (simulatedSubtraction <= 0)
            {
                Quantity = 0;
                return;
            }

            // Check that amount is not above maximum ceiling.
            if (simulatedSubtraction > MaxQuantity)
            {
                Quantity = MaxQuantity;
                return;
            }

            // Set the quantity to desired amount.
            Quantity = simulatedSubtraction;
        }

       
        public void AddQuantity(int amount)
        {
          
            var simulatedAddition = Quantity + amount;

            if (simulatedAddition < 0)
            {
                Quantity = 0;
                return;
            }

            if (simulatedAddition > MaxQuantity)
            {
                Quantity = MaxQuantity;
                return;
            }

            Quantity = simulatedAddition;
        }

     
        public void Repair()
        {
        }
    }
}