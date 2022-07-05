using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Models
{
    public class AuditBase
    {
        public AuditBase()
        {
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }
        public DateTime CreatedOn { get; set; }
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public bool IsDeleted { set; get; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(100)]
        public string ModifiedBy { get; set; }

    }
}
