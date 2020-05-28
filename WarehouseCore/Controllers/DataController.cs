using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            res1 = Repository.GetFilterSortDocuments( filter,  pagesize,  sort,  order,  page, warehouse);


            return res1;
        }

    }
}