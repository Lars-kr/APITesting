using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Model
{
    public class DepartmentModel
    {
        public int DepartmentId { get; set; }
        public int LicenceNo { get; set; }
        public int Users { get; set; }
        public string SupplierName { get; set; }
        public string StoreAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
