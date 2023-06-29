using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.ComponentModel;
using AvroNET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal sealed class DurationSchema : LogicalTypeSchema
    {
        public DurationSchema() : this(typeof(TimeSpan))
        {
        }
        public DurationSchema(Type runtimeType) : base(runtimeType)
        {
            BaseTypeSchema = new FixedSchema(
                new NamedEntityAttributes(new SchemaName("duration"), new List<string>(), ""),
                12,
                typeof(TimeSpan));
        }

        internal override AvroType Type => AvroType.Logical;
        internal override TypeSchema BaseTypeSchema { get; set; }
        internal override string LogicalTypeName => "duration";

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            var baseSchema = (FixedSchema)BaseTypeSchema;
            writer.WriteStartObject();
            writer.WriteProperty("type", baseSchema.Type.ToString().ToLowerInvariant());
            writer.WriteProperty("size", baseSchema.Size);
            writer.WriteProperty("name", baseSchema.Name);
            writer.WriteProperty("logicalType", LogicalTypeName);
            writer.WriteEndObject();
        }

        internal override object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType)
        {
            byte[] baseBytes = (byte[])baseValue;
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(baseBytes); //reverse it so we get big endian.

            int months = BitConverter.ToInt32(baseBytes.Skip(0).Take(4).ToArray(), 0);
            int days = BitConverter.ToInt32(baseBytes.Skip(4).Take(4).ToArray(), 0);
            int milliseconds = BitConverter.ToInt32(baseBytes.Skip(8).Take(4).ToArray(), 0);

            var result = new TimeSpan(months * 30 + days, 0, 0, 0, milliseconds);

            if (readType == typeof(TimeOnly) || readType == typeof(TimeOnly?))
            {
                return TimeOnly.FromTimeSpan(result);
            }

            return result;
        }
    }
}
