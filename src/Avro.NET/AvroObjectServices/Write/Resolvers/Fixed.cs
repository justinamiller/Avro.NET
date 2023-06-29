using AvroNET.AvroObjectServices.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;
using AvroNET.Infrastructure.Exceptions;
using AvroNET.AvroObjectServices.Schemas.AvroTypes;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Fixed
    {
        internal Encoder.WriteItem Resolve(FixedSchema es)
        {
            return (value, encoder) =>
            {
                if (!(value is Fixed) || !((AvroFixed)value).Schema.Equals(es))
                {
                    throw new AvroTypeMismatchException("[GenericFixed] required to write against [Fixed] schema but found " + value.GetType());
                }

                AvroFixed ba = (AvroFixed)value;
                encoder.WriteFixed(ba.Value);
            };
        }
    }
}
