using System.Net.Http.Headers;

using EasyNetQ;

namespace PositiveTechnologies.FibonacciServer.Client.Http;

public static class HttpClientInternal
{
    private const string ApiUrl = "http://localhost:5216/Fibonacci/generate/";

    public static async Task SendFibonacciRequest(int fibonacciNumber)
    {
        using var client = new HttpClient();

        client.BaseAddress = new Uri(ApiUrl + fibonacciNumber);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await client.GetAsync(ApiUrl);

        await Subscribe();

        Console.WriteLine("Complete");
    }

    private static async Task Subscribe()
    {
        using IBus bus = RabbitHutch.CreateBus("host=localhost");

        await bus.PubSub.SubscribeAsync<int>(
            "fibonacci",
            async number =>
            {
                Console.WriteLine($"Received number: {number}");
                await SendFibonacciRequest(number);
            });
    }
}