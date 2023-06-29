using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    /// <summary>
    ///     Class represents a string schema.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#schema_primitive">the specification</a>.
    /// </summary>
    internal class StringSchema : PrimitiveTypeSchema
    {
        internal StringSchema()
            : this(typeof(string))
        {
        }

        internal StringSchema(Type type)
            : this(type, new Dictionary<string, string>())
        {
        }

        internal StringSchema(Type type, Dictionary<string, string> attributes)
            : base(type, attributes)
        {
        }

        internal override AvroType Type => AvroType.String;
    }
}
