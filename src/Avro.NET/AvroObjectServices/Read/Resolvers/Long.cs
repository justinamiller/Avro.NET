using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        internal object ResolveLong(Type readType, IReader reader)
        {
            long value = reader.ReadLong();

            return readType == typeof(long) ? value : ConvertValue(readType, value);
        }
    }
}
