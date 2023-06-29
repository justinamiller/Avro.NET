using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.BuildSchema
{
    /// <summary>
    ///     Represents serialization information about a C# type.
    /// </summary>
    internal sealed class TypeSerializationInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TypeSerializationInfo" /> class.
        /// </summary>
        internal TypeSerializationInfo()
        {
            this.Aliases = new List<string>();
            this.Doc = string.Empty;
            this.Nullable = false;
        }

        /// <summary>
        ///     Gets or sets the Avro name of the type.
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        ///     Gets or sets the Avro namespace of the type.
        /// </summary>
        internal string Namespace { get; set; }

        /// <summary>
        ///     Gets the aliases.
        /// </summary>
        internal ICollection<string> Aliases { get; private set; }

        /// <summary>
        ///     Gets or sets the doc attribute.
        /// </summary>
        internal string Doc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this type can represent null values.
        /// </summary>
        /// <value>
        ///   <c>True</c> if nullable; otherwise, <c>false</c>.
        /// </value>
        internal bool Nullable { get; set; }
    }
}
