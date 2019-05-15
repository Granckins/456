using Newtonsoft.Json;
using System;
using System.Collections.Generic; 

namespace Warehouse.Model.Db
{
    public class Warehouse
    {
        [JsonProperty("archive")]
        public bool archive { get; set; }
        [JsonProperty("_rev")]
        public string _rev { get; set; }
        [JsonProperty("_id")]
        public string _id { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name{ get; set; }
    }
}
