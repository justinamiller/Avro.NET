using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        internal object ResolveDouble(Type readType, IReader reader)
        {
            double value = reader.ReadDouble();

            return readType == typeof(double) ? value : ConvertValue(readType, value);
        }
    }
}
