using AvroNET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas.Abstract
{
    /// <summary>
    ///     Class representing an named schema: record, enumeration or fixed.
    ///     For more details please see <a href="http://avro.apache.org/docs/current/spec.html#Names">the specification</a>.
    /// </summary>
    internal abstract class NamedSchema : TypeSchema
    {
        private readonly NamedEntityAttributes attributes;

        internal NamedSchema(
            NamedEntityAttributes nameAttributes,
            Type runtimeType,
            Dictionary<string, string> attributes)
            : base(runtimeType, attributes)
        {
            if (nameAttributes == null)
            {
                throw new ArgumentNullException("nameAttributes");
            }

            this.attributes = nameAttributes;
        }

        internal string FullName => this.attributes.Name.FullName;

        internal override string Name => this.attributes.Name.Name;

        internal string Namespace => this.attributes.Name.Namespace;

        internal ReadOnlyCollection<string> Aliases => this.attributes.Aliases.AsReadOnly();

        internal string Doc => this.attributes.Doc;
    }
}
