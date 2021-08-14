using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Extentions
{
    public static class StringExtentions
    {
        public static string RemoveHoursAndSlashes(this string date)
        {
            return date.Split(" ")[0].Replace("/", "");
        }
    }
}
