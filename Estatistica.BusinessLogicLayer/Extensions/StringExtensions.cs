using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Extensions
{
    public static class StringExtensions
    {
        public static string PascalCaseToStringWithSpaces(this string input)
        {
            return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }
    }
}
