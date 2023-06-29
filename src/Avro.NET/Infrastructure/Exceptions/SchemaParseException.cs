using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Exceptions
{
    internal class SchemaParseException : AvroException
    {
        internal SchemaParseException(string s)
            : base(s)
        {
        }
    }
}
