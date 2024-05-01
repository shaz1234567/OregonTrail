

namespace OregonTrailDotNet.Entity.Item
{
    
    public static class Resources
    {
       
        public static SimItem Clothing => new SimItem(Entities.Clothes, "Clothing", "sets", "set", 50, 10, 1, 1, 0, 2);

       
        public static SimItem Bullets => new SimItem(Entities.Ammo, "Ammunition", "boxes", "box", 99, 2, 0, 20, 0, 1, 50);

        
        public static SimItem Food => new SimItem(Entities.Food, "Food", "pounds", "pound", 2000, 0.20f, 1, 1, 0, 1, 25);

       
        public static SimItem Vehicle => new SimItem(Entities.Vehicle, "Vehicle", "vehicles", "vehicle", 2000, 50, 500, 1, 0, 50);

        
        public static SimItem Person => new SimItem(Entities.Person, "Person", "people", "person", 2000, 0, 1, 1, 0, 800);

       
        public static SimItem Cash => new SimItem(Entities.Cash, "Cash", "dollars", "dollar", int.MaxValue, 1, 0, 1, 0, 1, 5);
    }
}