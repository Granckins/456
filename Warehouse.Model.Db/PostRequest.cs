﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Model.Db
{

    public class PostRequest<Obj>
    {

        [JsonProperty("page")]
        public int page { get; set; }

        [JsonProperty("filtername")]
        public string filtername { get; set; }
        [JsonProperty("filtervalue")]
        public string filtervalue { get; set; }

        [JsonProperty("sortname")]
        public string sortname { get; set; }
        [JsonProperty("sortvalue")]
        public string sortvalue { get; set; }

        [JsonProperty("datepr")]
        public string datepr { get; set; }
        [JsonProperty("datevd")]
        public string datevd { get; set; }

        [JsonProperty("limit")]
        public int limit { get; set; }
        [JsonProperty("archive_str")]
        public bool archive_str { get; set; }

        [JsonProperty("str")]
        public string str { get; set; }

        [JsonProperty("Data_priyoma_str1")]
        public DateTime? Data_priyoma_str1 { get; set; }
        [JsonProperty("Data_vydachi_str1")]
        public DateTime? Data_vydachi_str1 { get; set; }
        [JsonProperty("Data_priyoma_str2")]
        public DateTime? Data_priyoma_str2 { get; set; }
        [JsonProperty("Data_vydachi_str2")]
        public DateTime? Data_vydachi_str2 { get; set; }
        [JsonProperty("Primechanie_str")]
        public string Primechanie_str { get; set; }
        public Obj entity { get; set; }
    }
}
