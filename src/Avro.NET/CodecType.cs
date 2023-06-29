using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET
{
    public enum CodecType
    {
        /// <summary>
        /// no compression
        /// </summary>
        Null = 0,
        /// <summary>
        /// deflat compression
        /// </summary>
        Deflate = 1,
        /// <summary>
        /// gzip compression
        /// </summary>
        GZip = 2,
    }
}
