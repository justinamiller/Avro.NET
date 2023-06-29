using AvroNET.AvroObjectServices.Schemas;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Bool
    {
        internal void Resolve(object value, IWriter encoder)
        {
            value ??= default(int);

            if (value is not bool convertedValue)
            {
                throw new AvroTypeMismatchException(
                    $"[{typeof(bool)}] required to write against [{AvroType.Boolean}] schema but found type: [{value?.GetType()}]");
            }

            encoder.WriteBoolean(convertedValue);
        }
    }
}
