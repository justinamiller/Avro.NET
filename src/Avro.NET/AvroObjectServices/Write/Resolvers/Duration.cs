using AvroNET.AvroObjectServices.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Duration
    {
        internal void Resolve(DurationSchema schema, object logicalValue, IWriter writer)
        {
            var duration = (TimeSpan)logicalValue;

            var baseSchema = (FixedSchema)schema.BaseTypeSchema;
            byte[] bytes = new byte[baseSchema.Size];
            var monthsBytes = BitConverter.GetBytes(0);
            var daysBytes = BitConverter.GetBytes(duration.Days);

            var milliseconds = ((duration.Hours * 60 + duration.Minutes) * 60 + duration.Seconds) * 1000 +
                               duration.Milliseconds;
            var millisecondsBytes = BitConverter.GetBytes(milliseconds);


            System.Array.Copy(monthsBytes, 0, bytes, 0, 4);
            System.Array.Copy(daysBytes, 0, bytes, 4, 4);
            System.Array.Copy(millisecondsBytes, 0, bytes, 8, 4);


            if (!BitConverter.IsLittleEndian)
                System.Array.Reverse(bytes); //reverse it so we get little endian.

            writer.WriteFixed(bytes);
        }
    }
}
