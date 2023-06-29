using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Schema representing an array.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#Arrays">the specification</a>.
    /// </summary>
    internal sealed class ArraySchema : TypeSchema
    {
        private readonly TypeSchema itemSchema;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArraySchema" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="runtimeType">Type of the runtime.</param>
        /// <param name="attributes">The attributes.</param>
        internal ArraySchema(
            TypeSchema item,
            Type runtimeType,
            Dictionary<string, string> attributes)
            : base(runtimeType, attributes)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.itemSchema = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArraySchema"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="runtimeType">Type of the runtime.</param>
        internal ArraySchema(
            TypeSchema item,
            Type runtimeType)
            : this(item, runtimeType, new Dictionary<string, string>())
        {
        }

        /// <summary>
        ///     Gets the item schema.
        /// </summary>
        internal TypeSchema ItemSchema
        {
            get { return this.itemSchema; }
        }

        /// <summary>
        ///     Converts current not to JSON according to the avro specification.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="seenSchemas">The seen schemas.</param>
        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            writer.WriteStartObject();
            writer.WriteProperty("type", "array");
            writer.WritePropertyName("items");
            this.itemSchema.ToJson(writer, seenSchemas);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Gets the type of the schema as string.
        /// </summary>
        internal override AvroType Type => AvroType.Array;
    }
}
