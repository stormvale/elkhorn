namespace API.Weather.Tests;

public class WeatherTests
{
    [Fact]
    public void WeatherForecast_CelciusToFahrenheit_WorksCorrectly()
    {
        WeatherForecast forecast = new(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), 5, "Cold");
        
        Assert.NotNull(forecast);
        Assert.Equal(40, forecast.TemperatureF);
    }
}