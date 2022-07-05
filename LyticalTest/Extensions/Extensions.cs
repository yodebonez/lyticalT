using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Extensions
{
    public static class Extensions
    {
        public static string IdentityErrorsToList(this IEnumerable<IdentityError> results)
        {
            var errors = "";
            foreach (var item in results)
            {
                errors += item.Description + Environment.NewLine;
            }

            return errors;
        }
    }
}
