using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Model.Db
{
     public class Warehouses
    { 
        [JsonProperty("_id")]
        public string _id { get; set; } 
        [JsonProperty("_rev")]
        public string _rev { get; set; }
        [JsonProperty("warehouse_name")]
        public string warehouse_name { get; set; }
    }
}
