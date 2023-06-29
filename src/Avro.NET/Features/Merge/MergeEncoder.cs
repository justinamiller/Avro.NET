using AvroNET.AvroObjectServices.FileHeader.Codec;
using AvroNET.AvroObjectServices.FileHeader;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvroNET.AvroObjectServices.Write;

namespace AvroNET.Features.Merge
{
    internal class MergeEncoder : IDisposable
    {
        private readonly AbstractCodec _codec;
        private readonly Stream _stream;
        private MemoryStream _tempBuffer;
        private readonly Writer _writer;
        private readonly Writer _tempWriter;
        private bool _isOpen;
        private int _blockCount;
        private readonly int _syncInterval;
        private readonly Header _header;


        internal MergeEncoder(Stream outStream)
        {
            _codec = new NullCodec();
            _stream = outStream;

            _syncInterval = DataFileConstants.DefaultSyncInterval;

            _blockCount = 0;
            _writer = new Writer(_stream);

            _tempBuffer = new MemoryStream();
            _tempWriter = new Writer(_tempBuffer);

            _isOpen = true;
            _header = new Header();
        }

        internal long Sync()
        {
            AssertOpen();
            WriteBlock();
            return _stream.Position;
        }

        internal void WriteHeader(string schema, CodecType codecType)
        {
            GenerateSyncData();
            _header.AddMetadata(DataFileConstants.CodecMetadataKey, AbstractCodec.CreateCodec(codecType).Name);
            _header.AddMetadata(DataFileConstants.SchemaMetadataKey, schema);

            _writer.WriteHeader(_header);
        }

        internal void WriteData(List<DataBlock> dataBlocks)
        {
            _tempWriter.WriteArrayStart();
            _tempWriter.WriteItemCount(dataBlocks.Select(x => x.ItemsCount).Sum());

            foreach (var dataBlock in dataBlocks)
            {
                _tempWriter.WriteBytesRaw(dataBlock.Data);
            }

            _tempWriter.WriteArrayEnd();

            _blockCount++;
            WriteIfBlockFull();
        }

        private void AssertOpen()
        {
            if (!_isOpen) throw new AvroRuntimeException("Cannot complete operation: avro file/stream not open");
        }

        private void WriteIfBlockFull()
        {
            if (_tempBuffer.Position >= _syncInterval)
                WriteBlock();
        }

        private void WriteBlock()
        {
            if (_blockCount > 0)
            {
                // byte[] dataToWrite = _tempBuffer.ToArray();

                _writer.WriteDataBlock(_codec.Compress(_tempBuffer), _header.SyncData, _blockCount);

                // reset block buffer
                _blockCount = 0;
                _tempBuffer = new MemoryStream();
            }
        }

        private void GenerateSyncData()
        {
            _header.SyncData = new byte[16];

            Random random = new Random();
            random.NextBytes(_header.SyncData);
        }

        public void Dispose()
        {
            Sync();
            _stream.Flush();
            _stream.Dispose();
            _isOpen = false;
        }
    }
}
