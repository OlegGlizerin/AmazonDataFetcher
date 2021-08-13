using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Extentions
{
    public static class StringExtentions
    {
        public static string RemoveHours(this string date)
        {
            return date.Split(" ")[0].Replace("/", "");
        }
    }
}
