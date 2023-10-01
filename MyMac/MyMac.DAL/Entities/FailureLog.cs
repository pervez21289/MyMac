using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMac.DAL.Entities
{
   

    public class FailureLog
    {
        public long FId { get; set; }

        public long? EmpId { get; set; }

        public string MacAddress { get; set; }
    }
}
