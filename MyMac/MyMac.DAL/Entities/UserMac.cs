using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace MyMac.DAL.Entities
{

    public class MacLogin
    {
        
        public long MacId { get; set; }

        public long? EmpId { get; set; }

        [MaxLength(50)]
        public string MacAddress { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? AddedDate { get; set; }

        public bool? Status { get; set; }

        public DateTime? Expiry { get; set; }

        public DateTime? Type { get; set; }

        public bool? IsLock { get; set; }

        public short? LoginAttempt { get; set; }
    }
}
