using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        internal object ResolveFloat(Type readType, IReader reader)
        {
            float value = reader.ReadFloat();

            return readType == typeof(float) ? value : ConvertValue(readType, value);
        }
    }
}
