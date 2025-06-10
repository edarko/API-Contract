using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIExercise
{
    public class VotesDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string image_id;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sub_id;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string created_at;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string value;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string currency_code;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public imageDTO image;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message;

    }

    public class imageDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url;
    }
}
