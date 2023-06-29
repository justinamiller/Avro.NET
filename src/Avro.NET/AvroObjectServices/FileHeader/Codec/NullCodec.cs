using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader.Codec
{
    internal class NullCodec : AbstractCodec
    {
        internal override string Name { get; } = "null";
        internal override byte[] Decompress(byte[] toDecompress)
        {
            return toDecompress;
        }

        internal override MemoryStream Compress(MemoryStream toCompress)
        {
            return toCompress;
        }
    }
}
