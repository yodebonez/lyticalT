using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Models
{
    public class Tenant 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string TennantCode { get; set; }

        public DateTime CreatedOn { get; set; }
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public bool IsDeleted { set; get; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(100)]
        public string ModifiedBy { get; set; }

    }
}
