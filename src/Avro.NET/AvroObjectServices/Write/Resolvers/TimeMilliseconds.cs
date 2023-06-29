using AvroNET.AvroObjectServices.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;
using AvroNET.Infrastructure.Extensions;
using AvroNET.Infrastructure.Exceptions;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class TimeMilliseconds
    {
        internal Encoder.WriteItem Resolve(TimeMillisecondsSchema schema)
        {
            return (value, encoder) =>
            {
                if (value is not TimeOnly timeOnly)
                {
                    throw new AvroTypeMismatchException(
                        $"[TimeOnly] required to write against [Int] of [time-millis] schema but found [{value.GetType()}]");
                }

                if (timeOnly > TimeMillisecondsSchema.MaxTime)
                    throw new ArgumentOutOfRangeException(nameof(TimeMilliseconds), "A 'time-millis' value can only have the range '00:00:00' to '23:59:59'.");

                encoder.WriteInt((int)(timeOnly - DateTimeExtensions.UnixEpochTime).TotalMilliseconds);
            };
        }
    }
}
