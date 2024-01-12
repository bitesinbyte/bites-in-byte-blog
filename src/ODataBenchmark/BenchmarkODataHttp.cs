using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using ODataBenchmark.Models;
using System.Text;
using System.Text.Json;
namespace ODataBenchmark;

[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Declared)]
public class BenchmarkOdataHttp
{
    private string _baseUrl = "https://services.odata.org/V4/(S(y5tuj04bxbfsxzimbxbnauqg))/TripPinServiceRW/";
    private string _userName = "scottketchum";

    private HttpClient _httpClient;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _httpClient = new HttpClient();
    }

    [Benchmark]
    public async Task FetchAll()
    {
        var response = await _httpClient.GetStringAsync($"{_baseUrl}/People");
        var peopleHttpData = JsonSerializer.Deserialize<ResponseData<People>>(response);
    }

    [Benchmark]
    public async Task FetchOne()
    {
        var response = await _httpClient.GetStringAsync($"{_baseUrl}/People('{_userName}')");
        var peopleHttpData = JsonSerializer.Deserialize<ResponseData<People>>(response);
    }

    private readonly People people = new()
    {
        FirstName = "bites",
        LastName = "byte",
        Age = "10",
        Emails = new List<string> { "test@test.com" },
        MiddleName = "In",
        Gender = "Male",
        AddressInfo = new List<AddressInfo>
        {
            new AddressInfo
            {
                Address = "Earth",
                City  =new AddressCityInfo
                {
                    CountryRegion = "DE",
                    Name = "Duesseldorf",
                    Region = "NRW"
                }
            }
        }
    };


    [Benchmark]
    public async Task AddOne()
    {
        people.UserName = "bitesInByte_Http";
        var httpMessage = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/People")
        {
            Content = new StringContent(JsonSerializer.Serialize(people), Encoding.UTF8, "application/json")
        };
        await _httpClient.SendAsync(httpMessage);
    }

    [Benchmark]
    public async Task Delete()
    {
        await _httpClient.DeleteAsync($"{_baseUrl}/People(UserName='bitesInByte_Http')");
    }
}
