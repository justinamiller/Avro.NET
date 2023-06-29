using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET
{
    public class InvalidAvroObjectException : Exception
    {
        internal InvalidAvroObjectException(string s)
            : base(s)
        {
        }

        internal InvalidAvroObjectException(string s, Exception inner)
            : base(s, inner)
        {
        }
    }
}
