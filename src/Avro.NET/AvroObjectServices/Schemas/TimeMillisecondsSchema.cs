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
    internal sealed class TimeMillisecondsSchema : LogicalTypeSchema
    {
        internal static readonly TimeOnly MaxTime = new TimeOnly(23, 59, 59);

        public TimeMillisecondsSchema() : this(typeof(TimeSpan))
        {
        }
        public TimeMillisecondsSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new IntSchema();
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => LogicalTypeEnum.TimeMilliseconds;

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            var noMs = (int)baseValue;
            return DateTimeExtensions.UnixEpochTime.Add(TimeSpan.FromMilliseconds(noMs));
        }
    }
}
