using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace MyMac.DAL.Entities
{

    public class EmpLogin
    {
     
        public long LoginId { get; set; }

        public long? EmpId { get; set; }

        [MaxLength(50)]
        public string MAC { get; set; }

        public DateTime? LoginTime { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        [MaxLength(50)]
        public string IP { get; set; }

        [MaxLength(100)]
        public string Cordinates { get; set; }
    }
}
