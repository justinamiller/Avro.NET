using AvroNET.AvroObjectServices.Schemas.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.BuildSchema
{
    internal abstract class Schema
    {
        protected Schema(IDictionary<string, string> attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            Attributes = new Dictionary<string, string>(attributes);
        }

        internal Dictionary<string, string> Attributes { get; set; }

        internal void AddAttribute(string attribute, string value)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Attributes.Add(attribute, value);
        }

        public override string ToString()
        {
            using (var result = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonTextWriter(result);
                this.ToJson(writer, new HashSet<NamedSchema>());
                return result.ToString();
            }
        }

        internal void ToJson(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            this.ToJsonSafe(writer, seenSchemas);
        }


        internal abstract void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas);


        internal static TypeSchema Create(string schemaInJson)
        {
            if (string.IsNullOrEmpty(schemaInJson))
            {
                throw new ArgumentNullException("schemaInJson");
            }

            return new TypeSchemaBuilder().BuildSchema(schemaInJson);
        }

        internal static TypeSchema Create(object obj)
        {
            var builder = new ReflectionSchemaBuilder();
            var schema = builder.BuildSchema(obj?.GetType());

            return schema;
        }

        internal static TypeSchema Create(Type type)
        {
            var builder = new ReflectionSchemaBuilder();
            var schema = builder.BuildSchema(type);

            return schema;
        }
    }
}
