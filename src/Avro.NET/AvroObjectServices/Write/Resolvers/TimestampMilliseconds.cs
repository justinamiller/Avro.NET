using AvroNET.AvroObjectServices.Schemas;
using AvroNET.Infrastructure.Exceptions;
using AvroNET.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class TimestampMilliseconds
    {
        internal void Resolve(TimestampMillisecondsSchema schema, object logicalValue, IWriter writer)
        {
            if (schema.BaseTypeSchema is not LongSchema)
            {
                throw new AvroTypeMismatchException(
                    $"[TimestampMilliseconds] required to write against [long] of [Long] schema but found [{schema.BaseTypeSchema}]");
            }

            DateTime date;
            switch (logicalValue)
            {
                case DateTimeOffset x:
                    date = x.DateTime;
                    break;

                default:
                    date = (DateTime)logicalValue;
                    break;
            }

            writer.WriteLong((long)(date - DateTimeExtensions.UnixEpochDateTime).TotalMilliseconds);
        }
    }
}
