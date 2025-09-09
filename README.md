# WeatherCLI

Single-file C# (.NET 6) console app that fetches current weather from OpenWeatherMap.  
Accepts a city argument, uses an API key from an environment variable, and prints conditions, temperature (°C or °F), humidity, and a short summary.

## Setup

1. Install [.NET 6 SDK](https://dotnet.microsoft.com/download).
2. Get a free API key from [OpenWeatherMap](https://openweathermap.org/api).
3. Set the environment variable:

### Linux / macOS
export WEATHER_API_KEY="your_api_key_here"

### Windows (PowerShell)
setx WEATHER_API_KEY "your_api_key_here"

## Usage

### Celsius (default)
dotnet run -- "Denver,CO"

### Fahrenheit
dotnet run -- "Denver,CO" --f

## File

- Program.cs — main console app
