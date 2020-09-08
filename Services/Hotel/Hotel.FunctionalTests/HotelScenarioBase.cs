using System.IO;
using System.Reflection;
using hotel.api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Hotel.FunctionalTests
{
    public class HotelScenarioBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(HotelScenarioBase))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.Development.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();

            var testServer = new TestServer(hostBuilder);
            
            return testServer;
        }

        public static class Get
        {
            public static string HotelsList()
            {
                return "api/v1/Hotel/HotelsList";
            }

            public static string HotelById(int id)
            {
                return $"api/v1/Hotel/{id}";
            }

            public static string ItemByName(string name)
            {
                return $"api/v1/Hotel/SearchHotel?name={name}";
            }

        }
    }
}