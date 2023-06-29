using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Schemas.AvroTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        protected virtual object ResolveFixed(FixedSchema writerSchema, TypeSchema readerSchema, IReader d, Type type)
        {
            FixedSchema rs = (FixedSchema)readerSchema;

            AvroFixed ru = new AvroFixed(rs);
            byte[] bb = ((AvroFixed)ru).Value;
            d.ReadFixed(bb);

            if (type == typeof(Guid))
            {
                return new Guid(ru.Value);
            }

            return ru.Value;
        }
    }
}
