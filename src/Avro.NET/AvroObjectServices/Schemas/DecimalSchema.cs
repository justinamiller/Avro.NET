using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Schemas.AvroTypes;
using AvroNET.Infrastructure.Exceptions;
using AvroNET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal sealed class DecimalSchema : LogicalTypeSchema
    {
        internal override AvroType Type => AvroType.Logical;

        internal override TypeSchema BaseTypeSchema { get; set; }
        internal int Precision { get; set; }
        internal int Scale { get; set; }

        internal override string LogicalTypeName => "decimal";

        public DecimalSchema() : this(typeof(decimal))
        {
        }
        public DecimalSchema(Type runtimeType) : this(runtimeType, 29, 14)  //Default C# values
        {
        }

        public DecimalSchema(Type runtimeType, int precision, int scale) : base(runtimeType)
        {
            BaseTypeSchema = new BytesSchema();

            if (precision <= 0)
                throw new AvroTypeException("Property [Precision] of [Decimal] schema has to be greater than 0");

            if (scale < 0)
                throw new AvroTypeException("Property [Scale] of [Decimal] schema has to be greater equal 0");

            if (scale > precision)
                throw new AvroTypeException("Property [Scale] of [Decimal] schema has to be lesser equal [Precision]");

            Scale = scale;
            Precision = precision;
        }

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            writer.WriteStartObject();
            writer.WriteProperty("type", BaseTypeSchema.Type.ToString().ToLowerInvariant());
            writer.WriteProperty("logicalType", LogicalTypeName);
            writer.WriteProperty("precision", Precision);
            writer.WriteProperty("scale", Scale);
            writer.WriteEndObject();
        }

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            var buffer = AvroType.Bytes == schema.BaseTypeSchema.Type
                ? (byte[])baseValue
                : ((AvroFixed)baseValue).Value;

            Array.Reverse(buffer);
            var avroDecimal = new AvroDecimal(new BigInteger(buffer), Scale);


            var value = AvroDecimal.ToDecimal(avroDecimal);

            if (readType != typeof(decimal))
            {
                if (readType == typeof(int))
                {
                    return Convert.ToInt32(value);
                }

                if (readType == typeof(short))
                {
                    return Convert.ToInt16(value);
                }

                if (readType == typeof(double))
                {
                    return Convert.ToDouble(value);
                }


                if (readType == typeof(long))
                {
                    return Convert.ToInt64(value);
                }

                if (readType == typeof(float))
                {
                    return Convert.ToSingle(value);
                }
            }

            return value;
        }



    }
}
