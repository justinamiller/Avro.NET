using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        protected virtual object ResolveEnum(EnumSchema writerSchema, TypeSchema readerSchema, IReader d, Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
            }

            int position = d.ReadEnum();
            string value = writerSchema.Symbols[position];
            return Enum.Parse(type, value);
        }
    }
}
