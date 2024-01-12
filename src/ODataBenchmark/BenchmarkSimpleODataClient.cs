using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using ODataBenchmark.Models;
using Simple.OData.Client;
namespace ODataBenchmark;

[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Declared)]
public class BenchmarkSimpleODataClient
{
    private string _baseUrl = "https://services.odata.org/V4/(S(y5tuj04bxbfsxzimbxbnauqg))/TripPinServiceRW/";
    private string _userName = "scottketchum";

    private ODataClient _client;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _client = new ODataClient(_baseUrl);
    }


    [Benchmark]
    public async Task FetchAll()
    {
        var dataEnum = await _client
           .For<People>("People")
           .FindEntriesAsync();
        var data = dataEnum.ToList();
    }

    [Benchmark]
    public async Task FetchOne()
    {
        var dataEnum = await _client
           .For<People>("People")
          .Filter(x => x.UserName == _userName)
          .FindEntriesAsync();
        var data = dataEnum.First();
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
        people.UserName = "bitesInByte_SimpleOdata";
        await _client
           .For<People>("People")
           .Set(people).InsertEntryAsync();
    }
    [Benchmark]
    public async Task Delete()
    {
        await _client
           .For<People>("People")
           .Key("bitesInByte_SimpleOdata")
           .DeleteEntryAsync();
    }
}
