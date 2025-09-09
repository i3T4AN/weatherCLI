using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run -- \"City, State\" [--f]");
            return 1;
        }

        string city = args[0];
        bool useFahrenheit = args.Length > 1 && args[1].Equals("--f", StringComparison.OrdinalIgnoreCase);

        string? apiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Error: WEATHER_API_KEY environment variable must be set.");
            return 1;
        }

        string baseUrl = "https://api.openweathermap.org/data/2.5/weather";
        string units = useFahrenheit ? "imperial" : "metric";
        string tempUnit = useFahrenheit ? "°F" : "°C";

        try
        {
            using HttpClient client = new HttpClient();
            string url = $"{baseUrl}?q={Uri.EscapeDataString(city)}&appid={apiKey}&units={units}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: API returned {(int)response.StatusCode} {response.ReasonPhrase}");
                return 1;
            }

            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);

            string cityName = doc.RootElement.GetProperty("name").GetString() ?? city;
            var weather = doc.RootElement.GetProperty("weather")[0];
            string description = weather.GetProperty("description").GetString() ?? "N/A";

            var main = doc.RootElement.GetProperty("main");
            double temp = main.GetProperty("temp").GetDouble();
            int humidity = main.GetProperty("humidity").GetInt32();

            Console.WriteLine($"City: {cityName}");
            Console.WriteLine($"Conditions: {description}");
            Console.WriteLine($"Temperature: {temp} {tempUnit}");
            Console.WriteLine($"Humidity: {humidity}%");
            Console.WriteLine($"Summary: {description}, {temp} {tempUnit}, {humidity}% humidity");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return 1;
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Error: Unexpected API response format.");
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            return 1;
        }

        return 0;
    }
}
