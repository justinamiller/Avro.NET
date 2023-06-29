using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Write;
using AvroNET.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvroNET.AvroObjectServices.Schemas.Abstract.LogicalTypeSchema;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal sealed class TimeMicrosecondsSchema : LogicalTypeSchema
    {
        private static readonly TimeSpan _maxTime = new TimeSpan(23, 59, 59);

        public TimeMicrosecondsSchema() : this(typeof(TimeSpan))
        {
        }
        public TimeMicrosecondsSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new LongSchema();
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => LogicalTypeEnum.TimeMicrosecond;
        internal void Serialize(object logicalValue, IWriter writer)
        {
            var time = (TimeSpan)logicalValue;

            if (time > _maxTime)
                throw new ArgumentOutOfRangeException(nameof(logicalValue), "A 'time-micros' value can only have the range '00:00:00' to '23:59:59'.");

            writer.WriteLong((long)(time - DateTimeExtensions.UnixEpochDateTime.TimeOfDay).TotalMilliseconds * 1000);
        }

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            var noMs = (long)baseValue / 1000;
            return DateTimeExtensions.UnixEpochDateTime.TimeOfDay.Add(TimeSpan.FromMilliseconds(noMs));
        }
    }
}
