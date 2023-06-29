using System;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;
using AvroNET.Infrastructure.Extensions;
using AvroNET.Infrastructure.Exceptions;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Date
    {
        internal Encoder.WriteItem Resolve()
        {
            return (value, encoder) =>
            {
                if (value is not DateOnly dateOnly)
                {
                    throw new AvroTypeMismatchException($"[DateOnly] required to write against [Int] of [Date] schema but found [{value.GetType()}]");
                }

                var result = dateOnly.DayNumber - DateTimeExtensions.UnixEpochDate.DayNumber;
                encoder.WriteInt(result);
            };
        }
    }
}
