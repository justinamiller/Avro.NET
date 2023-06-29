using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Exceptions
{
    internal class MissingSchemaException : Exception
    {
        internal MissingSchemaException(string s)
            : base(s)
        {
        }

        internal MissingSchemaException(string s, Exception inner)
            : base(s, inner)
        {
        }
    }
}
