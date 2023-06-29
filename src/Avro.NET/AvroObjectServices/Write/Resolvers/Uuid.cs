using AvroNET.AvroObjectServices.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;
using AvroNET.Infrastructure.Exceptions;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Uuid
    {
        internal Encoder.WriteItem Resolve(UuidSchema schema)
        {
            return (value, encoder) =>
            {
                if (value is not Guid guid)
                {
                    throw new AvroTypeMismatchException($"[Guid] required to write against [string] of [Uuid] schema but found [{value.GetType()}]");
                }

                encoder.WriteString(guid.ToString());
            };
        }
    }
}
