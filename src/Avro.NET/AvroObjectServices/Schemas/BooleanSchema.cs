using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class represents a boolean schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#schema_primitive">the specification</a>.
    /// </summary>
    internal sealed class BooleanSchema : PrimitiveTypeSchema
    {
        internal BooleanSchema()
            : this(new Dictionary<string, string>())
        {
        }

        internal BooleanSchema(Dictionary<string, string> attributes)
            : base(typeof(bool), attributes)
        {
        }

        internal override AvroType Type => AvroType.Boolean;
    }
}
