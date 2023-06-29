using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Exceptions
{
    internal class AvroTypeException : AvroException
    {
        internal AvroTypeException(string s)
            : base(s)
        {
        }
    }
}
