using System.Text.Json.Serialization;

namespace ODataBenchmark.Models
{
    public class ResponseData<T>
    {
        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
    }
}
