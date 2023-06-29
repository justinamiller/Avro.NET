using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.BuildSchema
{
    /// <summary>
    ///     Specifies Avro serializer settings.
    /// </summary>
    internal sealed class AvroSerializerSettings
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AvroSerializerSettings" /> class.
        /// </summary>
        internal AvroSerializerSettings(bool includeOnlyDataContractMembers = false)
        {
            this.GenerateDeserializer = true;
            this.GenerateSerializer = true;
            this.Resolver = new AvroContractResolver(includeOnlyDataContractMembers: includeOnlyDataContractMembers);
            this.MaxItemsInSchemaTree = 1024;
            this.UsePosixTime = false;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to generate a serializer.
        /// </summary>
        /// <value>
        ///     <c>True</c> if the serializer should be generated; otherwise, <c>false</c>.
        /// </value>
        internal bool GenerateSerializer { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to generate a deserializer.
        /// </summary>
        /// <value>
        ///     <c>True</c> if the deserializer should be generated; otherwise, <c>false</c>.
        /// </value>
        internal bool GenerateDeserializer { get; set; }

        /// <summary>
        ///     Gets or sets a contract resolver.
        /// </summary>
        internal AvroContractResolver Resolver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether DateTime values will be serialized in the Posix format (as a number
        /// of seconds passed from the start of the Unix epoch) or as a number of ticks.
        /// </summary>
        /// <value>
        ///   <c>True</c> if to use Posix format; otherwise, <c>false</c>.
        /// </value>
        internal bool UsePosixTime { get; set; }

        /// <summary>
        ///     Gets or sets the maximum number of items in the schema tree.
        /// </summary>
        /// <value>
        ///     The maximum number of items in the schema tree.
        /// </value>
        internal int MaxItemsInSchemaTree { get; set; }
    }
}
