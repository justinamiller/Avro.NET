using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvroNET.AvroObjectServices.Schemas.Abstract.LogicalTypeSchema;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal sealed class TimestampMillisecondsSchema : LogicalTypeSchema
    {
        public TimestampMillisecondsSchema() : this(typeof(DateTime))
        {
        }
        public TimestampMillisecondsSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new LongSchema();
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => LogicalTypeEnum.TimestampMilliseconds;

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            var noMs = (long)baseValue;
            var result = DateTimeExtensions.UnixEpochDateTime.AddMilliseconds(noMs);


            if (readType == typeof(DateTimeOffset) || readType == typeof(DateTimeOffset?))
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(noMs);
            }
            if (readType == typeof(DateOnly) || readType == typeof(DateOnly?))
            {
                return DateOnly.FromDateTime(result);
            }
            else
            {
                return result;
            }
        }
    }
}
