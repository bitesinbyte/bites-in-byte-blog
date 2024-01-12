using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.OData.SampleService.Models.TripPin;
using ODataBenchmark.Models;
namespace ODataBenchmark;

[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Declared)]
public class BenchmarkODataClient
{
    private string _baseUrl = "https://services.odata.org/V4/(S(y5tuj04bxbfsxzimbxbnauqg))/TripPinServiceRW/";
    private string _userName = "scottketchum";

    [Benchmark]
    public void FetchAll()
    {
        var context = new DefaultContainer(new Uri(_baseUrl));
        context.People.ToList();
    }

    [Benchmark]
    public void FetchOne()
    {
        var context = new DefaultContainer(new Uri(_baseUrl));
        var peopleEnum = context.People.Where(x => x.UserName == _userName).First();
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
    public void AddOne()
    {
        var context = new DefaultContainer(new Uri(_baseUrl));
        var person = new Person
        {
            FirstName = people.FirstName,
            LastName = people.LastName,
            Gender = PersonGender.Male,
            UserName = "bitesInByte_OdataClient"
        };
        person.Emails.Add(people.Emails[0]);
        person.AddressInfo.Add(
            new Location
            {
                Address = people.AddressInfo[0].Address,
                City = new City
                {
                    CountryRegion = people.AddressInfo[0].City.CountryRegion,
                    Name = people.AddressInfo[0].City.Name,
                    Region = people.AddressInfo[0].City.Region
                }
            });
        context.AddToPeople(person);
        context.SaveChanges();
    }

    [Benchmark]
    public void Delete()
    {
        var context = new DefaultContainer(new Uri(_baseUrl));
        var person = context.People.ByKey(userName: "bitesInByte_OdataClient").GetValue();
        context.DeleteObject(person);
        context.SaveChanges();
    }
}
