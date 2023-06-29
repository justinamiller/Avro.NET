using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class represents an int schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#schema_primitive">the specification</a>.
    /// </summary>
    internal class IntSchema : PrimitiveTypeSchema
    {
        internal IntSchema()
            : this(typeof(int))
        {
        }

        internal IntSchema(Type runtimeType)
            : this(runtimeType, new Dictionary<string, string>())
        {
        }

        internal IntSchema(Type runtimeType, Dictionary<string, string> attributes)
            : base(runtimeType, attributes)
        {
        }

        internal override AvroType Type => AvroType.Int;
    }
}
