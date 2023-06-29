using AvroNET.AvroObjectServices.FileHeader.Codec;
using AvroNET.AvroObjectServices.FileHeader;
using AvroNET.AvroObjectServices.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvroNET.AvroObjectServices.Schemas.Abstract;

namespace AvroNET.Features.Serialize
{
    internal class Encoder : IDisposable
    {
        internal delegate void WriteItem(object value, IWriter encoder);

        private readonly AbstractCodec _codec;
        private readonly Stream _outStream;
        private readonly Writer _writer;

        private MemoryStream _memoryChunk;
        private Writer _chunkWriter;

        private readonly WriteItem _writeItem;
        private int _blockCount;
        private readonly int _syncInterval;
        private readonly Header _header;


        internal Encoder(TypeSchema schema, Stream outOutStream, CodecType codecType)
        {
            _codec = AbstractCodec.CreateCodec(codecType);
            _outStream = outOutStream;
            _header = new Header();
            _syncInterval = DataFileConstants.DefaultSyncInterval;

            _blockCount = 0;
            _writer = new Writer(_outStream);
            _memoryChunk = new MemoryStream();
            _chunkWriter = new Writer(_memoryChunk);

            GenerateSyncData();
            _header.AddMetadata(DataFileConstants.CodecMetadataKey, _codec.Name);
            _header.AddMetadata(DataFileConstants.SchemaMetadataKey, schema.ToString());

            _writeItem = WriteResolver.ResolveWriter(schema);

            _writer.WriteHeader(_header);
        }
        private void GenerateSyncData()
        {
            _header.SyncData = new byte[16];

            Random random = new Random();
            random.NextBytes(_header.SyncData);
        }

        internal void Append(object datum)
        {
            _writeItem(datum, _chunkWriter);

            _blockCount++;

            //write buffer if bigger than sync interval
            if (_memoryChunk.Position >= _syncInterval)
            {
                WriteBuffer();
            }
        }

        private void WriteBuffer()
        {
            if (_blockCount > 0)
            {
                _writer.WriteDataBlock(_codec.Compress(_memoryChunk), _header.SyncData, _blockCount);

                // reset memory buffer
                _blockCount = 0;
                _memoryChunk = new MemoryStream();
                _chunkWriter = new Writer(_memoryChunk);
            }
        }

        public void Dispose()
        {
            WriteBuffer();
            _memoryChunk.Flush();
            _memoryChunk.Dispose();
            _outStream.Flush();
            _outStream.Dispose();
        }
    }
}
