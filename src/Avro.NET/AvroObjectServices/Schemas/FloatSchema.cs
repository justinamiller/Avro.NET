using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class represents a float schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#schema_primitive">the specification</a>.
    /// </summary>
    internal sealed class FloatSchema : PrimitiveTypeSchema
    {
        internal FloatSchema()
            : this(new Dictionary<string, string>())
        {
        }

        internal FloatSchema(Dictionary<string, string> attributes)
            : base(typeof(float), attributes)
        {
        }

        internal FloatSchema(Type runtimeType)
            : base(runtimeType, new Dictionary<string, string>())
        {
        }

        internal override AvroType Type => AvroType.Float;
    }
}
