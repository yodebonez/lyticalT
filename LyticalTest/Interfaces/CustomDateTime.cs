using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Interfaces
{
    public class CustomDateTime : IDateTime
    {
        public System.DateTime Now => NowDate();

        public DateTime Date => NowDate().Date;

        private System.DateTime NowDate()
        {
            return System.DateTime.UtcNow.AddHours(1);
        }
    }
}
