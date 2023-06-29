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
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET;

namespace AvroNET.Features.Deserialize
{
    internal class Decoder
    {
        internal T Decode<T>(Stream stream, TypeSchema readSchema)
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
                var header = reader.ReadHeader();

                TypeSchema writeSchema = Schema.Create(header.GetMetadata(DataFileConstants.SchemaMetadataKey));
                readSchema ??= writeSchema;
                var resolver = new Resolver(writeSchema, readSchema);

                // read in sync data 
                reader.ReadFixed(header.SyncData);
                var codec = AbstractCodec.CreateCodecFromString(header.GetMetadata(DataFileConstants.CodecMetadataKey));


                return Read<T>(reader, header, codec, resolver);
            }
        }


        internal T Read<T>(Reader reader, Header header, AbstractCodec codec, Resolver resolver)
        {
            if (reader.IsReadToEnd())
            {
                return default;
            }

            long itemsCount = 0;
            byte[] data = Array.Empty<byte>();

            do
            {
                itemsCount += reader.ReadLong();
                var dataBlock = reader.ReadDataBlock(header.SyncData, codec);

                int dataBlockSize = data.Length;
                Array.Resize(ref data, dataBlockSize + dataBlock.Length);
                Array.Copy(dataBlock, 0, data, dataBlockSize, dataBlock.Length);

            } while (!reader.IsReadToEnd());


            reader = new Reader(new MemoryStream(data));

            return resolver.Resolve<T>(reader, itemsCount);
        }
    }
}
