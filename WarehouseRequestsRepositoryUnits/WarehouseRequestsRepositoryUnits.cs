using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Warehouse.Model;
using IRepositoryRequestsUnits;
using System.Threading.Tasks;
using Warehouse.Model.Db;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Warehouse.Core.Repositories
{
    public class WarehouseRequestsRepositoryUnits
    {

        async Task<string> HTTP_GET(string url)
        {
            var TARGETURL = url;

            HttpClientHandler handler = new HttpClientHandler();

            Console.WriteLine("GET: + " + TARGETURL);

            // ... Use HttpClient.            
            HttpClient client = new HttpClient(handler);

            var byteArray = Encoding.ASCII.GetBytes("admin:root");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.GetAsync(TARGETURL);
            HttpContent content = response.Content;

            // ... Check Status Code                                
            Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

            // ... Read the string.
            string result = await content.ReadAsStringAsync();

            // ... Display the result.
            if (result != null)
            {
                return result;
            }
            return "";
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
     
    public   UserIdentity GetUserIdentityByName(string username, string password1)
        {



            var url = "http://localhost:5984/_fti/local/users/_design/foo/by_username?q=UserName:" + "\"" + username + "\"^10";
            Task<string> task = HTTP_GET(url);
            task.Wait();
            var res= task.Result;
            
           var user = new User();
           
            
                var lucene = JsonConvert.DeserializeObject<LuceneRequest<User>>(res);

            if (lucene.rows.Count <= 0)
            {
                return new UserIdentity { UserId = "000000000000000000000000" };
            }

            user = lucene.rows.First().fields;
            user.Id = lucene.rows.First().id;

            var salt = System.Text.Encoding.UTF8.GetBytes(user.Password);
            

          
            if (user.Password == MD5Hash(password1))
            {
                return new UserIdentity
                {
                    UserId = user.Id,
                    Name = user.UserName,
                    Password = user.Password,

                };
            }
            else
                return new UserIdentity { UserId = "000000000000000000000000" };
        }
        public CouchRequest<EventCouch> GetFilterSortDocuments(int page = 1, int limit = 10, bool archive = false, FilterSort FS = null)
        {
            CouchRequest<EventCouch> list = new CouchRequest<EventCouch>();
            limit = 100;
            var skip = (page - 1) * limit;
            var q = "";
            var sort = "";

            if (FS != null)
            {
                foreach (var qq in FS.Filters)
                {
                    if (qq.value == "Наименование")
                        qq.value = "Наименование_изделия";
                    if (qq.value != "")
                        if (qq.value.Contains("-"))
                            q += qq.name.Replace(" ", "_") + ":" + qq.value + "^1 AND ";
                        else
                            q += qq.name.Replace(" ", "_") + ":" + qq.value + "*^1 AND ";
                }

                q += "archive:" + archive.ToString().ToLower();
                int sq = 0;
                if (FS.Sorts.Count > 0 && FS.Sorts[0].name != "Дата приёма" && FS.Sorts[0].name != "Дата выдачи")
                {
                    sort = "&sort=";
                    var qs = FS.Sorts[0];
                    if (qs.name == "Номер упаковки" || qs.name == "Количество")
                    {
                        if (qs.value == "1")
                            sort += "/" + qs.name.Replace(" ", "_") + "<int>";
                        else
                            sort += "\\" + qs.name.Replace(" ", "_") + "<int>";
                    }
                    if (qs.name == "Наименование изделия" || qs.name == "Заводской номер" || qs.name == "Обозначение" || qs.name == "Местонахождение на складе" || qs.name == "Система" || qs.name == "Ответственный" || qs.name == "Принадлежность")
                    {
                        if (qs.value == "1")
                            sort += "/" + qs.name.Replace(" ", "_");
                        else
                            sort += "\\" + qs.name.Replace(" ", "_");
                    }
                }
            }
            else
            {
                q += "archive:" + archive.ToString().ToLower();
            }
            var url = "http://localhost:5984/_fti/local/events/_design/searchdocuments/by_fields?q=" + q + sort + "&skip=" + skip + "&limit=" + limit;
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Credentials = new NetworkCredential("admin", "root");
            var response = request.GetResponse();

            var user = new User();
            var lucene1 = new LuceneRequest<EventCouch>();
            lucene1.rows = new List<Row<EventCouch>>();
            using (var responseStream = response.GetResponseStream())
            {
                var reader = new StreamReader(responseStream, Encoding.UTF8);
                var res = reader.ReadToEnd();
                var lucene = JsonConvert.DeserializeObject<LuceneRequest<EventWar>>(res);

                foreach (var l in lucene.rows)
                {
                  
                    EventCouch ev = new EventCouch();
                    ev = EventManager.ConvertEventWarToEventCouchParent(l.fields);
     

                    lucene1.rows.Add(new Row<EventCouch>() { id = l.id, fields = ev });
                }
                list.total_rows = lucene.total_rows;
           

                foreach (var r in lucene1.rows)
                {
                   list.rows.Add( r.fields  );
                    list.rows.Last()._id = r.id;
                }
         
            }
            return list;

        }

        public void Dispose()
        {

        }
        public object Clone()
        {
            return new WarehouseRequestsRepositoryUnits();
        }
    }
}
