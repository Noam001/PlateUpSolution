using Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using WebApiClient;
using WebAppPlateUp.Models;

namespace Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string orderdate = "2020-12-15";
            string ordertime = "13:00";
            Day weather = await GetWeather(orderdate, ordertime);
            PrintWeatherModel(weather);
            Console.ReadLine();
        }
        static async Task<Day> GetWeather(string orderDate, string orderTime)
        {

            string date = orderDate + "T" + orderTime + ":00";
            string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/London,UK/{date}?key=YXLRG4K97Z69YFDVRLKP9GNUS";
            using HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(url);

            // כאן הקסם קורה:
            using JsonDocument doc = JsonDocument.Parse(json);

            // אנחנו ניגשים ישירות לאיבר הראשון בתוך המערך "days"
            JsonElement firstDayJson = doc.RootElement.GetProperty("days")[0];

            // עכשיו אנחנו הופכים רק את החלק הזה למחלקה שלנו
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Day weatherDay = JsonSerializer.Deserialize<Day>(firstDayJson.GetRawText(), options);

            return weatherDay;
        }
        static void PrintWeatherModel(Day weather)
        {
            Console.WriteLine($"Date: {weather.Datetime} , temp: {weather.Temp} , icon: {weather.Icon} , con: {weather.Conditions}");
        }


        static string GenerateSalt()
        {
            byte[] saltbytes = new byte[16];
            RandomNumberGenerator.Fill(saltbytes);
            return Convert.ToBase64String(saltbytes);
        }
        static string CalculateHash(string password, string salt)
        {
            string s = password + salt;
            byte[] pass = System.Text.Encoding.UTF8.GetBytes(s);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(pass);
                return Convert.ToBase64String(bytes);
            }
        }
        static void ViewHash()
        {
            string pass = "q1b8c7d2";
            string salt = GenerateSalt();
            Console.WriteLine(salt);
            string hash = CalculateHash(pass, salt);
            Console.WriteLine(hash);
        }
    }
}
