using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader.Codec
{
    internal class GZipCodec : AbstractCodec
    {
        internal override string Name { get; } = "gzip";
        internal override byte[] Decompress(byte[] compressedData)
        {
            using var compressedStream = new MemoryStream(compressedData);
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }

        internal override MemoryStream Compress(MemoryStream toCompress)
        {
            var toCompressBytes = toCompress.ToArray();
            using var compressedStream = new MemoryStream();
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress, leaveOpen: true);
            zipStream.Write(toCompressBytes, 0, toCompressBytes.Length);

            return compressedStream;
        }
    }
}
