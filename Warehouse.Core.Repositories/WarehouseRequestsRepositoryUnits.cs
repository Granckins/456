using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Warehouse.Model;

namespace Warehouse.Core.Repositories
{
    public class WarehouseRequestsRepositoryUnits : IRepositoryRequestsUnits
    {
        public UserIdentity GetUserIdentityByName(string username)
        {

            var url = "http://localhost:5984/_fti/local/users/_design/foo/by_username?q=UserName:" + "\"" + username + "\"^10";
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Credentials = new NetworkCredential("admin", "root");
            var response = request.GetResponse();

            var user = new User();
            using (var responseStream = response.GetResponseStream())
            {
                var reader = new StreamReader(responseStream, Encoding.UTF8);
                var res = reader.ReadToEnd();
                var lucene = JsonConvert.DeserializeObject<LuceneRequest<User>>(res);

                if (lucene.rows.Count <= 0)
                {
                    return new UserIdentity { UserId = "000000000000000000000000" };
                }
                user = lucene.rows.First().fields;
                user.Id = lucene.rows.First().id;
            }

            return new UserIdentity
            {
                UserId = user.Id,
                Name = user.UserName,
                Password = user.Password,

            };
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
