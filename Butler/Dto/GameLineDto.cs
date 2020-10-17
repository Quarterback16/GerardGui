using Newtonsoft.Json;

namespace Butler.Dto
{
    public class GameLineDto
    {
        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("spread")]
        public decimal Spread { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }

}
