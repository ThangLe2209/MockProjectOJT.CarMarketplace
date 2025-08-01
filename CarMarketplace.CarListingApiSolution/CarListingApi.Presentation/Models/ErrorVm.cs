using Newtonsoft.Json;

namespace CarListingApi.Presentation.Models
{
    public class ErrorVm
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Path { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
