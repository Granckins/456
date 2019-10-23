using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Warehouse.Core.Repositories;
using Warehouse.Model;
using Warehouse.Model.Db;

namespace WarehouseCore.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        WarehouseRequestsRepositoryUnits Repository = new WarehouseRequestsRepositoryUnits();
        [HttpGet("[action]")]
        public CouchRequest<EventCouch> FilterSortDocument(string filter, int pagesize, string sort  , string order  ,int page, string warehouse  )
        {
            var FS = new FilterSort();
            var res1 = new CouchRequest<EventCouch>();


            //Repository.Update();
            res1 = Repository.GetFilterSortDocuments( filter,  pagesize,  sort,  order,  page, "");


            return res1;
        }
         [HttpPost ]
        public async Task<FileContentResult> ExportData(string fileName)
        {
            var res1 = new CouchRequest<EventCouchFull>();
            res1 = Repository.GetAllEvents();
            string output = JsonConvert.SerializeObject(res1.rows);
            byte[] contents = System.Text.Encoding.UTF8.GetBytes(output);


            return File(contents, "application/json", "test.json");
        }
        //[HttpPost("[action]")]
        //public FileContentResult ExportData( )
        //{

        //    var res1 = new CouchRequest<EventCouchFull>();

        //        res1 = Repository.GetAllEvents();

        //    //foreach (var rr in res1.rows)
        //    //{
        //    //    var revs = Repository.GetRevisionListEvent(rr._id);
        //    //    var r1 = Repository.GetRevisionFiesldsEvent(rr._id, revs);
        //    //    rr.value._revs = r1.ToEventCouch();
        //    //    rr.value._id = rr.id;
        //    //}
        //    string output = JsonConvert.SerializeObject(res1.rows);
        //    byte[] contents = System.Text.Encoding.UTF8.GetBytes(output);

        //    return File(contents, "application/json", "test.json");

        //}
    }
}