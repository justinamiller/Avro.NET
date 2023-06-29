﻿using AvroNET.AvroObjectServices.BuildSchema;
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

namespace AvroNET.Features.AvroToJson
{
    internal class Decoder
    {
        internal object Decode(Stream stream, TypeSchema schema)
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
                if (schema == null)
                {
                    throw new MissingSchemaException("Provide valid schema for the Avro data");
                }
                var resolver = new Resolver(schema);
                stream.Seek(0, SeekOrigin.Begin);
                return resolver.Resolve(reader);
            }
            else
            {
                var header = reader.ReadHeader();

                schema = schema ?? Schema.Create(header.GetMetadata(DataFileConstants.SchemaMetadataKey));
                var resolver = new Resolver(schema);

                reader.ReadFixed(header.SyncData);
                var codec = AbstractCodec.CreateCodecFromString(header.GetMetadata(DataFileConstants.CodecMetadataKey));

                return Read(reader, header, codec, resolver);
            }
        }


        internal object Read(Reader reader, Header header, AbstractCodec codec, Resolver resolver)
        {
            if (reader.IsReadToEnd())
            {
                return string.Empty;
            }


            var result = new List<object>();

            do
            {
                long itemsCount = reader.ReadLong();
                var data = reader.ReadDataBlock(header.SyncData, codec);

                reader = new Reader(new MemoryStream(data));

                if (itemsCount > 1)
                {
                    for (int i = 0; i < itemsCount; i++)
                    {
                        result.Add(resolver.Resolve(reader));
                    }
                }
                else
                {
                    return resolver.Resolve(reader);
                }

            } while (!reader.IsReadToEnd());


            return result;
        }
    }
}