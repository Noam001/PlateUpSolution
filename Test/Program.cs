using Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Currency> list = CurrencyListTest().Result;
            int count = 1;
            foreach(var currency in list)
            {
                Console.WriteLine($"{count}. {currency.symbol} - {currency.name}");
                count++;
            }
            Console.WriteLine("Select Currency number from >> ");
            int from = int.Parse(Console.ReadLine());
            Console.WriteLine("Select Currency number to >> ");
            int to = int.Parse(Console.ReadLine());
            Console.WriteLine("Inter Sum >> ");
            int sum = int.Parse(Console.ReadLine());
            Console.ReadLine();
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
    }
}
