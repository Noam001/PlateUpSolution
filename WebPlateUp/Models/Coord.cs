namespace WebAppPlateUp.Models
{
    public class Coord
    {
        public double lon { get; set; }
        public double lat{ get; set; }

    }
    public class Weather
    {
       public int id { get; set; }
       public string main { get; set; }
       public string description{ get; set; }
       public string icon{ get; set; }
    }
    public class WeatherApi
    {
        public Weather weather { get; set; }
        public Coord Coord { get; set; }
    }
}
