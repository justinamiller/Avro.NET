using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.FileHeader.Codec;
using AvroNET.AvroObjectServices.FileHeader;
using AvroNET.AvroObjectServices.Read;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvroNET;

namespace AvroNET.Features.Merge
{
    internal class MergeDecoder
    {
        internal AvroObjectContent ExtractAvroObjectContent(byte[] avroObject)
        {
            using (var stream = new MemoryStream(avroObject))
            {
                var reader = new Reader(stream);

                // validate header 
                byte[] firstBytes = new byte[DataFileConstants.AvroHeader.Length];

                try
                {
                    reader.ReadFixed(firstBytes);
                }
                catch (EndOfStreamException)
                {
                    //stream shorter than AvroHeader
                }

                //does not contain header
                if (!firstBytes.SequenceEqual(DataFileConstants.AvroHeader))
                {
                    throw new InvalidAvroObjectException("Object does not contain Avro Header");
                }
                else
                {
                    AvroObjectContent result = new AvroObjectContent();
                    var header = reader.ReadHeader();
                    result.Codec = AbstractCodec.CreateCodecFromString(header.GetMetadata(DataFileConstants.CodecMetadataKey));

                    reader.ReadFixed(header.SyncData);

                    result.Header = header;
                    result.Header.Schema = Schema.Create(result.Header.GetMetadata(DataFileConstants.SchemaMetadataKey));

                    if (reader.IsReadToEnd())
                    {
                        return result;
                    }

                    do
                    {
                        var blockContent = new DataBlock
                        {
                            ItemsCount = reader.ReadLong(),
                            Data = reader.ReadDataBlock(header.SyncData, result.Codec)
                        };

                        result.DataBlocks.Add(blockContent);

                    } while (!reader.IsReadToEnd());

                    return result;
                }
            }
        }
    }
}
