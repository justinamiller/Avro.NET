using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class represents a double schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#schema_primitive">the specification</a>.
    /// </summary>
    internal sealed class DoubleSchema : PrimitiveTypeSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleSchema"/> class.
        /// </summary>
        internal DoubleSchema() : this(new Dictionary<string, string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleSchema"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        internal DoubleSchema(Dictionary<string, string> attributes) : base(typeof(double), attributes)
        {
        }

        internal override AvroType Type => AvroType.Double;
    }
}
