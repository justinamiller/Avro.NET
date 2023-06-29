using AvroNET;
using AvroNET.AvroObjectServices.FileHeader;
using AvroNET.AvroObjectServices.Read;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GetSchema
{
    internal class HeaderDecoder
    {
        internal string GetSchema(Stream stream)
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

                return header.GetMetadata(DataFileConstants.SchemaMetadataKey);
            }
        }
    }
}
