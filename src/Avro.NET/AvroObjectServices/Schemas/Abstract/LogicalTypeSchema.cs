using AvroNET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas.Abstract
{
    internal abstract class LogicalTypeSchema : TypeSchema
    {
        internal class LogicalTypeEnum
        {
            internal const string
                Uuid = "uuid",
                TimestampMilliseconds = "timestamp-millis",
                TimestampMicroseconds = "timestamp-micros",
                Decimal = "decimal",
                Duration = "duration",
                TimeMilliseconds = "time-millis",
                TimeMicrosecond = "time-micros ",
                Date = "date";
        }

        internal abstract TypeSchema BaseTypeSchema { get; set; }
        internal abstract string LogicalTypeName { get; }


        protected LogicalTypeSchema(Type runtimeType) : base(runtimeType, new Dictionary<string, string>())
        {
        }

        internal override bool CanRead(TypeSchema writerSchema)
        {
            return writerSchema.Type == Type || writerSchema.Type == BaseTypeSchema.Type;
        }

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            BaseTypeSchema.ToJsonSafe(writer, seenSchemas);
            writer.WriteProperty("logicalType", LogicalTypeName);
            writer.WriteEndObject();
        }

        internal abstract object ConvertToLogicalValue(object baseValue, LogicalTypeSchema schema, Type readType);
    }
}
