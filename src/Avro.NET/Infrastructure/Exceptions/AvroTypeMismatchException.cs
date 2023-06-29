using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Exceptions
{
    internal class AvroTypeMismatchException : Exception
    {
        internal AvroTypeMismatchException(string s)
            : base(s)
        {

        }
        internal AvroTypeMismatchException(string s, Exception inner)
            : base(s, inner)
        {

        }
    }
}
