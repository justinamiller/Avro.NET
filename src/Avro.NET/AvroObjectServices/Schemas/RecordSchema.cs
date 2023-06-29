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
using System.Xml.Linq;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class represents a record schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#schema_record">the specification</a>.
    /// </summary>
    internal sealed class RecordSchema : NamedSchema
    {
        private readonly List<RecordFieldSchema> fields;
        private readonly Dictionary<string, RecordFieldSchema> fieldsByName;

        internal RecordSchema(
            NamedEntityAttributes namedAttributes,
            Type runtimeType,
            Dictionary<string, string> attributes)
            : base(namedAttributes, runtimeType, attributes)
        {
            fields = new List<RecordFieldSchema>();
            fieldsByName = new Dictionary<string, RecordFieldSchema>(StringComparer.InvariantCultureIgnoreCase);
        }

        internal RecordSchema(string name, string @namespace) : this(new NamedEntityAttributes(new SchemaName(name, @namespace), new List<string>(), string.Empty), typeof(object))
        {
        }

        internal RecordSchema(NamedEntityAttributes namedAttributes, Type runtimeType)
            : this(namedAttributes, runtimeType, new Dictionary<string, string>())
        {
        }

        internal void AddField(RecordFieldSchema field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }

            fields.Add(field);
            fieldsByName.Add(field.Name, field);
        }

        internal bool TryGetField(string fieldName, out RecordFieldSchema result)
        {
            return fieldsByName.TryGetValue(fieldName, out result);
        }

        internal RecordFieldSchema GetField(string fieldName)
        {
            return fieldsByName[fieldName];
        }

        internal ReadOnlyCollection<RecordFieldSchema> Fields => fields.AsReadOnly();

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            if (seenSchemas.Contains(this))
            {
                writer.WriteValue(this.FullName);
                return;
            }

            seenSchemas.Add(this);
            writer.WriteStartObject();
            writer.WriteProperty("name", Name);
            writer.WriteOptionalProperty("namespace", Namespace);
            writer.WriteOptionalProperty("doc", Doc);
            writer.WriteOptionalProperty("aliases", Aliases);
            writer.WriteProperty("type", "record");
            writer.WritePropertyName("fields");
            writer.WriteStartArray();
            fields.ForEach(_ => _.ToJson(writer, seenSchemas));
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        internal override AvroType Type => AvroType.Record;

        internal override bool CanRead(TypeSchema writerSchema)
        {
            return Type == writerSchema.Type
                   || Fields.Count == 0; //hack to allow any item to be serialized to Object 
        }
    }
}
