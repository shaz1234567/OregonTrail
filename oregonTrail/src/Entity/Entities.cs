

using WolfCurses.Utility;

namespace OregonTrailDotNet.Entity
{
    
    public enum Entities
    {
       
        [Description("Oxen              @AMT@")] Animal = 1,

       
        [Description("Food              @AMT@")] Food = 2,

        
        [Description("Clothing          @AMT@")] Clothes = 3,

        
        [Description("Ammunition        @AMT@")] Ammo = 4,

     
        [Description("Vehicle wheels    @AMT@")] Wheel = 5,

        
        [Description("Vehicle axles     @AMT@")] Axle = 6,

      
        [Description("Vehicle tongues   @AMT@")] Tongue = 7,

     
        Vehicle = 8,

       
        Person = 9,

       
        Cash = 10,

      
        Location = 11
    }
}