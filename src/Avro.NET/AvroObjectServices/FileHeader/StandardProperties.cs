using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader
{
    /// <summary>
    ///     Class containing standard properties for different avro types.
    /// </summary>
    internal static class StandardProperties
    {
        internal static readonly HashSet<string> Primitive = new HashSet<string> { AvroKeywords.Type };

        internal static readonly HashSet<string> Record = new HashSet<string>
        {
            AvroKeywords.Type,
            AvroKeywords.Name,
            AvroKeywords.Namespace,
            AvroKeywords.Doc,
            AvroKeywords.Aliases,
            AvroKeywords.Fields
        };

        internal static readonly HashSet<string> Enumeration = new HashSet<string>
        {
            AvroKeywords.Type,
            AvroKeywords.Name,
            AvroKeywords.Namespace,
            AvroKeywords.Doc,
            AvroKeywords.Aliases,
            AvroKeywords.Symbols
        };
    }
}
