using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Model.Db
{

    public class RowCouch
    {
        public string id { get; set; }
        public string key { get; set; }
        public int page { get; set; }

        public string filtername { get; set; }
        public string filtervalue { get; set; }
        public string sortname { get; set; }
        public string sortvalue { get; set; }
        public string datepr { get; set; }
        public string datevd { get; set; }

        public int limit { get; set; }
        public bool archive_str { get; set; }
    

    }

    public class CouchRequest<Obj>
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public RowCouch row { get; set; }
        public List<Obj> rows { get; set; }
        public List<Warehouse> wars { get; set; }
        public CouchRequest()
        {
            rows = new List<Obj>();
            wars = new List<Warehouse>();
        }
    }
    public class CouchRequestMultiKey<Obj>
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public List<RowCouchMultiKey<Obj>> rows { get; set; }
    }
    public class RowCouchMultiKey<Obj>
    {
        public string id { get; set; }
        public List<string> key { get; set; }

        public Obj value { get; set; }
    }
}
