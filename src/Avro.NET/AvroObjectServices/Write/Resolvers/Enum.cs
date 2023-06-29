using AvroNET.AvroObjectServices.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;
using AvroNET.Infrastructure.Exceptions;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Enum
    {
        internal Encoder.WriteItem Resolve(EnumSchema schema)
        {
            return (value, e) =>
            {
                if (!schema.Symbols.Contains(value.ToString()))
                {
                    throw new AvroTypeException(
                        $"[Enum] Provided value is not of the enum [{schema.Name}] members");
                }

                e.WriteEnum(schema.GetValueBySymbol(value.ToString()));
            };
        }
    }
}
