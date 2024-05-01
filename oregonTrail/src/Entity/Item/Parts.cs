

namespace OregonTrailDotNet.Entity.Item
{
   
    public static class Parts
    {
        
        public static SimItem Oxen => new SimItem(Entities.Animal, "Oxen", "oxen", "ox", 20, 20, 0, 1, 0, 4);

        
        public static SimItem Axle => new SimItem(Entities.Axle, "Vehicle Axle", "axles", "axle", 3, 10, 0, 1, 0, 2);

       
        public static SimItem Tongue => new SimItem(Entities.Tongue, "Vehicle Tongue", "tongues", "tongue", 3, 10, 0, 1, 0, 2);

       
        public static SimItem Wheel => new SimItem(Entities.Wheel, "Vehicle Wheel", "wheels", "wheel", 3, 10, 0, 1, 0, 2);
    }
}