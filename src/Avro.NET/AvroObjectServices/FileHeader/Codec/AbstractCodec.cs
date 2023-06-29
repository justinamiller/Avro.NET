using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader.Codec
{
    internal abstract class AbstractCodec
    {
        internal abstract string Name { get; }

        internal abstract byte[] Decompress(byte[] compressedData);

        internal abstract MemoryStream Compress(MemoryStream toCompress);

        internal static AbstractCodec CreateCodec(CodecType codecType)
        {
            switch (codecType)
            {
                case CodecType.Deflate:
                    return new DeflateCodec();
                case CodecType.GZip:
                    return new GZipCodec();
                default://include null
                    return new NullCodec();
            }
        }

        internal static AbstractCodec CreateCodecFromString(string codecName)
        {
            var parsedSuccessfully = Enum.TryParse<CodecType>(codecName, true, out var codecType);
            return parsedSuccessfully ? CreateCodec(codecType) : new NullCodec();
        }
    }
}
