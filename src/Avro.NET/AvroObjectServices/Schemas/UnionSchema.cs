using AvroNET.AvroObjectServices.Schemas.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class representing a union schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#Unions">the specification</a>.
    /// </summary>
    internal sealed class UnionSchema : TypeSchema
    {
        private readonly List<TypeSchema> schemas;

        internal UnionSchema(
            List<TypeSchema> schemas,
            Type runtimeType,
            Dictionary<string, string> attributes)
            : base(runtimeType, attributes)
        {
            if (schemas == null)
            {
                throw new ArgumentNullException("schemas");
            }
            this.schemas = schemas;
        }

        internal UnionSchema(params TypeSchema[] schemas) : this(new List<TypeSchema>(schemas), typeof(List<>))
        {
        }


        internal UnionSchema(
            List<TypeSchema> schemas,
            Type runtimeType)
            : this(schemas, runtimeType, new Dictionary<string, string>())
        {
        }

        internal ReadOnlyCollection<TypeSchema> Schemas => schemas.AsReadOnly();

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            writer.WriteStartArray();
            this.schemas.ForEach(_ => _.ToJson(writer, seenSchemas));
            writer.WriteEndArray();
        }

        internal override AvroType Type => AvroType.Union;
    }
}
