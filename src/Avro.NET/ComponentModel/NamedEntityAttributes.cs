using AvroNET.AvroObjectServices.BuildSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.ComponentModel
{
    /// <summary>
    ///     Standard attributes supported by named schemas.
    /// </summary>
    internal sealed class NamedEntityAttributes
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NamedEntityAttributes" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="doc">The doc.</param>
        internal NamedEntityAttributes(SchemaName name, IEnumerable<string> aliases, string doc)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (aliases == null)
            {
                throw new ArgumentNullException("aliases");
            }

            Name = name;
            Aliases = new List<string>(aliases);
            Doc = string.IsNullOrEmpty(doc) ? string.Empty : doc;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        internal SchemaName Name { get; private set; }

        /// <summary>
        ///     Gets the aliases.
        /// </summary>
        internal List<string> Aliases { get; private set; }

        /// <summary>
        ///     Gets the doc.
        /// </summary>
        internal string Doc { get; private set; }
    }
}
