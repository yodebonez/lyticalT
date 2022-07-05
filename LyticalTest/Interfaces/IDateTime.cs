using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime Date { get; }
    }
}
