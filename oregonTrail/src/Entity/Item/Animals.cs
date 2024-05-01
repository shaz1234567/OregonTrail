

namespace OregonTrailDotNet.Entity.Item
{
   
    public static class Animals
    {
        
        public static SimItem Bear => new(Entities.Food, "Bear", "pounds", "pound", 2000, 0);

        
        public static SimItem Buffalo => new(Entities.Food, "Buffalo", "pounds", "pound", 2000, 0,
            GameSimulationApp.Instance.Random.Next(350, 500));

        
        public static SimItem Caribou => new(Entities.Food, "Caribou", "pounds", "pound", 2000, 0,
            GameSimulationApp.Instance.Random.Next(300, 350));

        
        public static SimItem Deer => new(Entities.Food, "Deer", "pounds", "pound", 2000, 0, 50);

       
        public static SimItem Duck => new(Entities.Food, "Duck", "pounds", "pound", 2000, 0);

       
        public static SimItem Goose => new(Entities.Food, "Goose", "pounds", "pound", 2000, 0, 2);

       
        public static SimItem Rabbit => new(Entities.Food, "Rabbit", "pounds", "pound", 2000, 0, 2);

      
        public static SimItem Squirrel => new(Entities.Food, "Squirrel", "pounds", "pound", 2000, 0);
    }
}