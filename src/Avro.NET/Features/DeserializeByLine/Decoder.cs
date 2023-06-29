using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.FileHeader.Codec;
using AvroNET.AvroObjectServices.FileHeader;
using AvroNET.AvroObjectServices.Read;
using AvroNET.Features.DeserializeByLine.LineReaders;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvroNET.AvroObjectServices.Schemas.Abstract;

namespace AvroNET.Features.DeserializeByLine
{
    internal class Decoder
    {
        internal static ILineReader<T> OpenReader<T>(Stream stream, TypeSchema readSchema)
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

            //headless
            if (!firstBytes.SequenceEqual(DataFileConstants.AvroHeader))
            {
                if (readSchema == null)
                {
                    throw new MissingSchemaException("Provide valid schema for the Avro data");
                }
                var resolver = new Resolver(readSchema, readSchema);
                stream.Seek(0, SeekOrigin.Begin);
                return new ListLineReader<T>(reader, resolver);
            }
            else
            {
                var header = reader.ReadHeader();

                readSchema = readSchema ?? Schema.Create(header.GetMetadata(DataFileConstants.SchemaMetadataKey));
                TypeSchema writeSchema = Schema.Create(header.GetMetadata(DataFileConstants.SchemaMetadataKey));

                // read in sync data 
                reader.ReadFixed(header.SyncData);
                var codec = AbstractCodec.CreateCodecFromString(header.GetMetadata(DataFileConstants.CodecMetadataKey));


                return new BaseLineReader<T>(reader, header.SyncData, codec, writeSchema, readSchema);
            }
        }
    }
}
