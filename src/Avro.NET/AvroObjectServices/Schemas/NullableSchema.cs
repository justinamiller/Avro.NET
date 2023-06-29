using AvroNET.AvroObjectServices.Schemas.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    /// Artificial node for nullable types.
    /// </summary>
    internal sealed class NullableSchema : TypeSchema
    {
        private readonly TypeSchema valueSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableSchema" /> class.
        /// </summary>
        /// <param name="nullableType">Type of the nullable.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="valueSchema">The value type schema.</param>
        internal NullableSchema(
            Type nullableType,
            IDictionary<string, string> attributes,
            TypeSchema valueSchema)
            : base(nullableType, attributes)
        {
            this.valueSchema = valueSchema;
        }

        internal NullableSchema(
            Type nullableType,
            TypeSchema valueSchema)
            : this(nullableType, new Dictionary<string, string>(), valueSchema)
        {
        }

        internal TypeSchema ValueSchema
        {
            get { return this.valueSchema; }
        }

        internal override AvroType Type => valueSchema.Type;

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            this.valueSchema.ToJsonSafe(writer, seenSchemas);
        }
    }
}
