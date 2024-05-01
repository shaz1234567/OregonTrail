

using OregonTrailDotNet.Module.Time;

namespace OregonTrailDotNet.Entity.Location.Weather
{
   
    public class ClimateData
    {
       
        public ClimateData(
            Month month,
            float averageTemp,
            float tempMax,
            float tempMin,
            float rainfall,
            int avgHumidity)
        {
            Month = month;
            Temperature = averageTemp;
            TemperatureMax = tempMax;
            TemperatureMin = tempMin;
            Rainfall = rainfall;
            Humidity = avgHumidity;
        }

        
        public Month Month { get; }

        
        public float Temperature { get; }

     
        public float TemperatureMax { get; }

       
        public float TemperatureMin { get; }

     
        
        public float Rainfall { get; }

       
        public int Humidity { get; }
    }
}