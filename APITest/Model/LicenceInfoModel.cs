using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Model
{
    public class LicenceInfoModel
    {
        [Required]
        public int LicenceNo { get; set; }
        public int CustomerNo { get; set; }
        public string UniqueKey { get; set; }
        public DateTime DateTo { get; set; }
        public string CompanyName { get; set; }
        

    }
}
