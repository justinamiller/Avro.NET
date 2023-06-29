using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader.Codec
{
    internal class DeflateCodec : AbstractCodec
    {
        internal override string Name { get; } = "deflate";

        internal override MemoryStream Compress(MemoryStream toCompress)
        {
            var toCompressBytes = toCompress.ToArray();
            using MemoryStream outStream = new MemoryStream();

            using DeflateStream compress = new DeflateStream(outStream, CompressionMode.Compress, true);
            compress.Write(toCompressBytes, 0, toCompressBytes.Length);
            return outStream;
        }

        internal override byte[] Decompress(byte[] compressedData)
        {
            using MemoryStream inStream = new MemoryStream(compressedData);
            using MemoryStream outStream = new MemoryStream();

            using (DeflateStream decompress =
                        new DeflateStream(inStream,
                        CompressionMode.Decompress))
            {
                CopyTo(decompress, outStream);
            }
            return outStream.ToArray();
        }

        private static void CopyTo(Stream from, Stream to)
        {
            byte[] buffer = new byte[4096];
            int read;
            while ((read = from.Read(buffer, 0, buffer.Length)) != 0)
            {
                to.Write(buffer, 0, read);
            }
        }
    }
}
