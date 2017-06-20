using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Experiments.AutomaticCaller
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                var tasks = new[]
                {
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest),
                    Task.Run(SendRequest)
                };
                Task.WaitAll(tasks);
            }
        }

        private static async Task SendRequest()
        {
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:57910/") })
            {
                var response = await client.GetAsync("api/Caller");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }
}