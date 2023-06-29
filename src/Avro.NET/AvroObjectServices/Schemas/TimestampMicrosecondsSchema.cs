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
    internal sealed class TimestampMicrosecondsSchema : LogicalTypeSchema
    {
        public TimestampMicrosecondsSchema() : this(typeof(DateTime))
        {
        }
        public TimestampMicrosecondsSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new LongSchema();
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => LogicalTypeEnum.TimestampMicroseconds;

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            var noMicroseconds = (long)baseValue;
            var result = DateTimeExtensions.UnixEpochDateTime.AddMilliseconds(noMicroseconds / 1000);

            if (readType == typeof(DateTimeOffset) || readType == typeof(DateTimeOffset?))
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(noMicroseconds / 1000);
            }
            else
            {
                return result;
            }
        }
    }
}
