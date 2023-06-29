using AvroNET.ComponentModel;
using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal sealed class RecordFieldSchema : Schema
    {
        internal string Name => NamedEntityAttributes.Name.Name;
        internal string Namespace => NamedEntityAttributes.Name.Namespace;
        internal ReadOnlyCollection<string> Aliases => NamedEntityAttributes.Aliases.AsReadOnly();
        internal string Doc => NamedEntityAttributes.Doc;
        internal TypeSchema TypeSchema { get; }
        internal bool HasDefaultValue { get; }
        internal object DefaultValue { get; }
        internal int Position { get; }
        internal NamedEntityAttributes NamedEntityAttributes { get; }


        internal RecordFieldSchema(
            NamedEntityAttributes namedEntityAttributes,
            TypeSchema typeSchema,
            bool hasDefaultValue,
            object defaultValue,
            int position)
            : this(namedEntityAttributes, typeSchema, hasDefaultValue, defaultValue, position, new Dictionary<string, string>())
        {
        }

        internal RecordFieldSchema(
            NamedEntityAttributes namedEntityAttributes,
            TypeSchema typeSchema,
            bool hasDefaultValue,
            object defaultValue,
            int position,
            Dictionary<string, string> attributes)
            : base(attributes)
        {
            NamedEntityAttributes = namedEntityAttributes;
            TypeSchema = typeSchema;
            HasDefaultValue = hasDefaultValue;
            DefaultValue = defaultValue;
            Position = position;
        }

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            writer.WriteStartObject();

            writer.WriteProperty("name", Name);
            writer.WriteOptionalProperty("namespace", Namespace);
            writer.WriteOptionalProperty("doc", Doc);
            writer.WriteOptionalProperty("aliases", Aliases);
            writer.WritePropertyName("type");
            TypeSchema.ToJson(writer, seenSchemas);
            writer.WriteOptionalProperty("default", DefaultValue, HasDefaultValue);

            writer.WriteEndObject();
        }

    }
}
