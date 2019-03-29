using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Model.Db
{
    public class EventCouch
    {
        [JsonProperty("archive")]
        public bool archive { get; set; }
        [JsonProperty("_rev")]
        public string _rev { get; set; }
        [JsonProperty("_id")]
        public string _id { get; set; }
        [JsonProperty("Nomer_upakovki")]
        public int Nomer_upakovki { get; set; }
        [JsonProperty("Naimenovanie_izdeliya")]
        public string Naimenovanie_izdeliya { get; set; }
        [JsonProperty("Zavodskoj_nomer")]
        public string Zavodskoj_nomer { get; set; }
        [JsonProperty("Kolichestvo")]
        public int Kolichestvo { get; set; }
        [JsonProperty("Oboznachenie")]
        public string Oboznachenie { get; set; } 
        [JsonProperty("Sistema")]
        public string Sistema { get; set; }
        [JsonProperty("Prinadlezhnost")]
        public string Prinadlezhnost { get; set; }

        [JsonProperty("Stoimost")]
        public float Stoimost { get; set; }
        [JsonProperty("Otvetstvennyj")]
        public string Otvetstvennyj { get; set; }
        [JsonProperty("Mestonahozhdenie_na_sklade")]
        public string Mestonahozhdenie_na_sklade { get; set; }
        [JsonProperty("Ves_brutto")]
        public float Ves_brutto { get; set; }
        [JsonProperty("Ves_netto")]
        public float Ves_netto { get; set; }
        [JsonProperty("Dlina")]
        public float Dlina { get; set; }
        [JsonProperty("Shirina")]
        public float Shirina { get; set; }
        [JsonProperty("Vysota")]
        public float Vysota { get; set; }

        [JsonProperty("Data_priyoma")]
        public DateTime? Data_priyoma { get; set; }
        [JsonProperty("Otkuda")]
        public string Otkuda { get; set; }
        [JsonProperty("Data_vydachi")]
        public DateTime? Data_vydachi { get; set; }
        [JsonProperty("Kuda")]
        public string Kuda { get; set; }
        [JsonProperty("Nomer_plomby")]
        public string Nomer_plomby { get; set; }
        [JsonProperty("Primechanie")]
        public string Primechanie { get; set; }
        [JsonProperty("Dobavil")]
        public string Dobavil { get; set; }
        [JsonProperty("Data_ismenen")]
        public DateTime Data_ismenen { get; set; }
        public List<RevsInfo> _revs_info { get; set; }
        public EventCouch()
        {
            _revs_info = new List<RevsInfo>();
           
            this.archive = false;
        }
        public string ToString()
        {
            var sod = "";
            var str = "";
           


            str = Nomer_upakovki.ToString().Replace(';', ',') + ";" +
             (Naimenovanie_izdeliya == null ? "" : Naimenovanie_izdeliya).Replace(';', ',') + ";" +
          (Zavodskoj_nomer == null ? "" : Zavodskoj_nomer).Replace(';', ',') + ";" +
              Kolichestvo.ToString().Replace(';', ',') + ";" +
                 (Mestonahozhdenie_na_sklade == null ? "" : Mestonahozhdenie_na_sklade).Replace(';', ',') + ";" +
          (Oboznachenie == null ? "" : Oboznachenie).Replace(';', ',') + ";" +
         sod + ";" +
         (Sistema == null ? "" : Sistema).Replace(';', ',') + ";" +
             (Otvetstvennyj == null ? "" : Otvetstvennyj).Replace(';', ',') + ";" +
         (Prinadlezhnost == null ? "" : Prinadlezhnost).Replace(';', ',') + ";";
            if (Data_priyoma != null)
                str += (Data_priyoma.Value.Date == new DateTime(1, 1, 1).Date ? "" : Data_priyoma.Value.Day + "." + Data_priyoma.Value.Month + "." + Data_priyoma.Value.Year) + ";";
            else
                str += ";";

            str += (Otkuda == null ? "" : Otkuda).Replace(';', ',') + ";";


            if (Data_vydachi != null)
                str += (Data_vydachi.Value.Date == new DateTime(1, 1, 1).Date ? "" : Data_vydachi.Value.Day + "." + Data_vydachi.Value.Month + "." + Data_vydachi.Value.Year) + ";";
            else
                str += ";";

            str += (Kuda == null ? "" : Kuda).Replace(';', ',') + ";" +
             (Nomer_plomby == null ? "" : Nomer_plomby).Replace(';', ',') + ";" +
               Stoimost.ToString().Replace(';', ',') + ";" +
              Ves_brutto.ToString().Replace(';', ',') + ";" +
           Ves_netto.ToString().Replace(';', ',') + ";" +
              Dlina.ToString().Replace(';', ',') + ";" +
      Shirina.ToString().Replace(';', ',') + ";" +
          Vysota.ToString().Replace(';', ',') + ";" +

              (Primechanie == null ? "" : Primechanie).Replace(';', ',') + ";" +
              (Dobavil == null ? "" : Dobavil).Replace(';', ',') + ";\n";
            return str;
        }


    }
}
