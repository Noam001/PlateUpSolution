using Models;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ModelValidation();
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
    }
}
