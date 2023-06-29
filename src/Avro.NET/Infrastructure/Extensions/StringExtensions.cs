using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Extensions
{
    internal static class StringExtensions
    {
        public static string TryReplace(this string @string, string pattern, string replacement)
        {
            if (@string.Contains(pattern))
                @string = @string.Replace(pattern, replacement);

            return @string;
        }
    }
}
