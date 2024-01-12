namespace ODataBenchmark.Models;
public class People
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Gender { get; set; }
    public string Age { get; set; }
    public List<string> Emails { get; set; }
    public List<AddressInfo> AddressInfo { get; set; }
    public AddressInfo HomeAddress { get; set; }
    public long Concurrency => new();
}
public class AddressInfo
{
    public string Address { get; set; }
    public AddressCityInfo City { get; set; }

}
public class AddressCityInfo
{
    public string Name { get; set; }
    public string CountryRegion { get; set; }
    public string Region { get; set; }
}