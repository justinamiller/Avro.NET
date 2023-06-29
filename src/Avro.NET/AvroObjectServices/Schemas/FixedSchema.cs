using AvroNET.ComponentModel;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Represents a fixed schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#Fixed">the specification</a>.
    /// </summary>
    internal sealed class FixedSchema : NamedSchema
    {
        internal FixedSchema(NamedEntityAttributes namedEntityAttributes, int size, Type runtimeType)
            : this(namedEntityAttributes, size, runtimeType, new Dictionary<string, string>())
        {
        }

        internal FixedSchema(
            NamedEntityAttributes namedEntityAttributes,
            int size,
            Type runtimeType,
            Dictionary<string, string> attributes) : base(namedEntityAttributes, runtimeType, attributes)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            this.Size = size;
        }

        internal int Size { get; }

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            if (seenSchemas.Contains(this))
            {
                writer.WriteValue(this.FullName);
                return;
            }

            seenSchemas.Add(this);
            writer.WriteStartObject();
            writer.WriteProperty("type", "fixed");
            writer.WriteProperty("name", Name);
            writer.WriteOptionalProperty("namespace", Namespace);
            writer.WriteOptionalProperty("aliases", this.Aliases);
            writer.WriteProperty("size", this.Size);
            writer.WriteEndObject();
        }

        internal override AvroType Type => AvroType.Fixed;
    }
}
