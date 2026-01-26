using Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using WebApiClient;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            Console.WriteLine(date);
            Console.ReadLine();
        }
        static void TestWebClient()  
        {
            WebClient<Meal> webClient = new WebClient<Meal>();
            webClient.Schema = "http";
            webClient.Host = "localhost";
            webClient.Port = 5035;
            webClient.Path = "api/Guest/GetMealDetails";
            webClient.AddParameter("mealId", "5");
            Meal meal = webClient.Get();
            Console.WriteLine($"Name: {meal.MealName}, Price: {meal.MealPrice}.");
        }
        static void CurrencyTest()
        {
            List<Currency> list = CurrencyListTest().Result;
            int count = 1;
            foreach (var currency in list)
            {
                Console.WriteLine($"{count}. {currency.symbol} - {currency.name}");
                count++;
            }
            Console.WriteLine("Select Currency number from >> ");
            int from = int.Parse(Console.ReadLine());
            Console.WriteLine("Select Currency number to >> ");
            int to = int.Parse(Console.ReadLine());
            Console.WriteLine("Inter Sum >> ");
            int amount = int.Parse(Console.ReadLine());
            ConvertResult r = GetConvertResult(list[from - 1].symbol, list[to - 1].symbol, amount).Result;
            Console.WriteLine($"{r.result.amountToConvert} {r.result.from} = {r.result.convertedAmount} {r.result.to}");
        }
        static void ModelValidation()
        {
            Client client = new Client();
            client.ClientId = "12345678910";
            client.ClientName = "a";
            Dictionary<string, List<string>> errors = client.AllErrors();
            if (!client.IsValid)
            {
                foreach (var error in errors)
                {
                    foreach (var errorMsg in error.Value)
                    {
                        Console.WriteLine($"{errorMsg}");
                    }

                }
            }
        }

        static async Task<List<Currency>> CurrencyListTest()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://currency-converter18.p.rapidapi.com/api/v1/supportedCurrencies"),
                Headers =
    {
        { "x-rapidapi-key", "666e2c0358msh2992bad87407b33p104059jsne30a192444cf" },
        { "x-rapidapi-host", "currency-converter18.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Currency>>(body); //מעביר את הגייסון לרשימה של המחלקה CURRENCY
            }
        }
        static async Task<ConvertResult> GetConvertResult(string from, string to, double amount)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://currency-converter18.p.rapidapi.com/api/v1/convert?from={from}&to={to}&amount={amount}"),
                Headers =
    {
        { "x-rapidapi-key", "666e2c0358msh2992bad87407b33p104059jsne30a192444cf" },
        { "x-rapidapi-host", "currency-converter18.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ConvertResult>(body);
            }
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
