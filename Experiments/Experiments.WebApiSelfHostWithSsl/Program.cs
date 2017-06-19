using Microsoft.Owin.Hosting;
using System;
using System.Net;
using System.Net.Http;

namespace Experiments.WebApiSelfHostWithSsl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var baseServerAddress = "https://+:9443";
            var baseAddress = "https://localhost:9443/";

            // Start OWIN host
            using (var x = WebApp.Start<Startup>(url: baseServerAddress))
            {
                // Create HttpCient and make a request to api/values
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(baseAddress + "api/values").Result;
                    Console.WriteLine(response);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }

                Console.ReadLine();
            }
        }
    }
}