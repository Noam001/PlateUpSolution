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
            string orderdate = "2026-04-16";
            string ordertime = "18:00";
            Hour weather = await GetWeather(orderdate, ordertime);
            Console.WriteLine($"Date: {orderdate} {ordertime} , temp: {weather.Temp} , icon: {weather.Icon} , con: {weather.Conditions}");
            Console.ReadLine();
        }
        static async Task<Hour> GetWeather(string orderDate, string orderTime)
        {
            string date = orderDate + "T" + orderTime + ":00";
            string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Ashkelon,Israel/{date}?key=YXLRG4K97Z69YFDVRLKP9GNUS&unitGroup=metric&include=hours";

            using HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement firstDay = doc.RootElement.GetProperty("days")[0];
            JsonElement hours = firstDay.GetProperty("hours");

            string targetHour = orderTime + ":00"; // "13:00:00"
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            foreach (JsonElement hour in hours.EnumerateArray())
            {
                if (hour.GetProperty("datetime").GetString() == targetHour)
                {
                    return JsonSerializer.Deserialize<Hour>(hour.GetRawText(), options);
                }
            }

            return null; // אם לא נמצאה השעה
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
