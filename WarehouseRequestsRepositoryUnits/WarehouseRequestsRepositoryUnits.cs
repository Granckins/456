﻿using System;
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

           

            // ... Use HttpClient.            
            HttpClient client = new HttpClient(handler);

            var byteArray = Encoding.ASCII.GetBytes("admin:root");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.GetAsync(TARGETURL);
            HttpContent content = response.Content;

         
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



            var user = new User();
            var lucene1 = new LuceneRequest<EventCouch>();
            lucene1.rows = new List<Row<EventCouch>>();

            var page = 1;
            var limit = 20;
            var skip = (page - 1) * limit;
            var url = "http://localhost:5984/_fti/local/events/_design/searchdocuments/by_fields?q=archive:false" + "&skip=" + skip + "&limit=" + limit;

            Task<string> task = HTTP_GET(url);
            task.Wait();
            var res = task.Result;

            var lucene11 = JsonConvert.DeserializeObject<LuceneRequest<EventWar>>(res);

            foreach (var l in lucene11.rows)
            {

                EventCouch ev = new EventCouch();
                ev = EventManager.ConvertEventWarToEventCouchParent(l.fields);


                lucene1.rows.Add(new Row<EventCouch>() { id = l.id, fields = ev });
            }
            var total_rows = lucene11.total_rows;
            int pages = total_rows / limit;
            if (total_rows % limit > 0)
                pages++;

            CouchRequest<EventCouch> list = new CouchRequest<EventCouch>();

            for (page = 1; page <= pages; page++)
            {
                skip = (page - 1) * limit;
                url = "http://localhost:5984/_fti/local/events/_design/searchdocuments/by_fields?q=archive:false" + "&skip=" + skip + "&limit=" + limit;

                task = HTTP_GET(url);
                task.Wait();
                res = task.Result;

                lucene11 = JsonConvert.DeserializeObject<LuceneRequest<EventWar>>(res);
                lucene1 = new LuceneRequest<EventCouch>();
                lucene1.rows = new List<Row<EventCouch>>();
                foreach (var l in lucene11.rows)
                {

                    EventCouch ev = new EventCouch();
                    ev = EventManager.ConvertEventWarToEventCouchParent(l.fields);


                    lucene1.rows.Add(new Row<EventCouch>() { id = l.id, fields = ev });
                }
                list = new CouchRequest<EventCouch>();


                foreach (var r in lucene1.rows)
                {
                    list.rows.Add(r.fields);
                    list.rows.Last()._id = r.id;

                }

                foreach (var r in list.rows)
                {
                    r.warehouse = "6ded85b9bca49078e3d47e1508000796";


                    var json = JsonConvert.SerializeObject(r);
                    var request = (HttpWebRequest)WebRequest.Create("http://localhost:5984/events/" + r._id);
                    if (r._id=="47704e4bf3ed1cfb7b49c4359e024480")
            {

            }
                    ServicePointManager.DefaultConnectionLimit = 1000;
                    request.Credentials = new NetworkCredential("admin", "root");
                    request.Method = "PUT";
                    request.ContentType = "application/json";
                    request.KeepAlive = false;
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {


                        var o = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(json);


                        var json1 = JsonConvert.SerializeObject(o);

                        streamWriter.Write(json1);

                    }
                    try
                    {
                        var httpResponse = (HttpWebResponse)request.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var responseText = streamReader.ReadToEnd();

                            var response = JsonConvert.DeserializeObject<ResponseCouch>(responseText);

                        }
                    }
                    catch (Exception e)
                    {

                    }

                }






            }





              url = "http://localhost:5984/_fti/local/users/_design/foo/by_username?q=UserName:" + "\"" + username + "\"^10";
             task = HTTP_GET(url);
            task.Wait();
               res= task.Result;
            
              user = new User();
           
            
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


        public CouchRequest<EventCouch> GetFilterSortDocuments(string filter="", int pagesize=10, string sort="Номер_упаковки", string order="", int page=0, string warehouse="")
        {
            CouchRequest<EventCouch> list = new CouchRequest<EventCouch>();
          var  limit = pagesize;
            var skip = (page -1) * limit;
            if (filter == null)
                filter = "";
            var q = "";
            if (filter != "")
            {
                q ="( "+filter + " ) AND archive:false ";
            }
            else
            {
                q = "archive:false  ";
            }
            var sort1 = "&sort="; 
                    if (sort== "Номер_упаковки" || sort == "Количество")
                    {
                         if (order!="desc")
                         sort1 += "/" +sort+ "<int>";
                         else
                            sort1 += "\\" +sort + "<int>";
                    }
            else
            {
                if (order != "desc")
                      sort1 += "/" + sort;
                    else
                    sort1 += "\\" +sort;
            }

            if (warehouse == null||warehouse=="")
                warehouse = "";
            else
            q = "(" + q+")"+" AND "+"warehouse:"+warehouse;

            var url = "http://localhost:5984/_fti/local/events/_design/searchdocuments/by_fields?q=" + q + sort1 + "&skip=" + skip + "&limit=" + limit;
  

            var user = new User();
            var lucene1 = new LuceneRequest<EventCouch>();
            lucene1.rows = new List<Row<EventCouch>>();

            Task<string> task = HTTP_GET(url);
            task.Wait();
            var res = task.Result;
  
                var lucene = JsonConvert.DeserializeObject<LuceneRequest<EventWar>>(res);

                foreach (var l in lucene.rows)
                {
                  
                    EventCouch ev = new EventCouch();
                    ev = EventManager.ConvertEventWarToEventCouchParent(l.fields);
     

                    lucene1.rows.Add(new Row<EventCouch>() { id = l.id, fields = ev });
                }
                list.total_rows = lucene.total_rows;
           

   

          //   url = "http://localhost:5984/_fti/local/warehouses/_design/searchdocuments/by_fields?q=archive:false";
 
          //  task = HTTP_GET(url);
          //  task.Wait();
          // res = task.Result;

          //var  lucene123 = JsonConvert.DeserializeObject<LuceneRequest<Warehouse.Model.Db.Warehouse>>(res);

     

          //  foreach (var r in lucene123.rows)
          //  {
          //      r.fields.id = r.id;
          //      list.wars.Add(r.fields); 
          //  }

            return list;

        }
        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public string GetUUID()
        {
            var url = "http://localhost:5984/_uuids";
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Credentials = new NetworkCredential("admin", "root");
            var response = request.GetResponse();

            try
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var res = reader.ReadToEnd();

                    var uuid = JsonConvert.DeserializeObject<UUID>(res);
                    return uuid.uuids.First();
                }
            }
            catch (Exception e)
            {

            }



            return null;
        }
        public void Update() {

            var url = "http://localhost:5984/_fti/local/events/_design/searchdocuments/by_fields?q=archive:false&limit=80";


            var user = new User();
            var lucene1 = new LuceneRequest<EventCouch>();
            lucene1.rows = new List<Row<EventCouch>>();

            Task<string> task = HTTP_GET(url);
            task.Wait();
            var res = task.Result;

            var lucene = JsonConvert.DeserializeObject<LuceneRequest<EventWar>>(res);

            foreach (var l in lucene.rows)
            {

                EventCouch ev = new EventCouch();
                ev = EventManager.ConvertEventWarToEventCouchParent(l.fields);


                lucene1.rows.Add(new Row<EventCouch>() { id = l.id, fields = ev });
            }
            CouchRequest<EventCouch> list = new CouchRequest<EventCouch>();
            list.total_rows = lucene.total_rows;


            foreach (var r in lucene1.rows)
            {
                list.rows.Add(r.fields);
                list.rows.Last()._id = r.id;
            }
            int i = 0;
            foreach (var r in list.rows)
            {
                r.warehouse = "6ded85b9bca49078e3d47e1508000796";

                //var id1 = "";

                ////    id1 = GetUUID();
                //DateTime UpdatedTime = r.Data_vydachi ?? new DateTime();
               
                //r.Data_vydachi = new DateTimeOffset(r.Data_vydachi).ToUnixTimeMilliseconds();
                var json = JsonConvert.SerializeObject(r);
                var request = (HttpWebRequest)WebRequest.Create("http://localhost:5984/events/" + r._id);

                ServicePointManager.DefaultConnectionLimit = 1000;

                request.Credentials = new NetworkCredential("admin", "root");
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.KeepAlive = false;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
 

                        var o = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(json);


                        var json1 = JsonConvert.SerializeObject(o);

                        streamWriter.Write(json1);
                     
                }
            }
             
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
