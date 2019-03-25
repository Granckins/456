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
        public List<EventCouch> FilterSortDocument( )
        {
            var FS = new FilterSort();
            var res1 = new List<EventCouch>();
            


            res1 = Repository.GetFilterSortDocuments( );


            return res1;
        }

    }
}