using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvroNET.AvroObjectServices.Schemas.Abstract.LogicalTypeSchema;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal sealed class UuidSchema : LogicalTypeSchema
    {
        public UuidSchema() : this(typeof(Guid))
        {
        }
        public UuidSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new StringSchema();
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => LogicalTypeEnum.Uuid;

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            if (baseValue is Guid)
                return baseValue;
            else
            {
                return Guid.Parse((string)baseValue);
            }
        }
    }
}
