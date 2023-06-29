using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas.Abstract
{
    /// <summary>
    ///     Base class for all type schemas.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html">the specification</a>.
    /// </summary>
    internal abstract class TypeSchema : BuildSchema.Schema
    {
        protected TypeSchema(Type runtimeType, IDictionary<string, string> attributes) : base(attributes)
        {
            if (runtimeType == null)
            {
                throw new ArgumentNullException("runtimeType");
            }

            RuntimeType = runtimeType;
        }

        internal Type RuntimeType { get; set; }

        internal abstract AvroType Type { get; }

        internal virtual bool CanRead(TypeSchema writerSchema) { return Type == writerSchema.Type; }

        internal virtual string Name => Type.ToString().ToLower(CultureInfo.InvariantCulture);
    }
}
