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
    internal sealed class DateSchema : LogicalTypeSchema
    {
        public DateSchema() : this(typeof(DateTime))
        {
        }
        public DateSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new IntSchema();
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => LogicalTypeEnum.Date;

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            var noDays = (int)baseValue;
            return DateTimeExtensions.UnixEpochDate.AddDays(noDays);
        }
    }
}
