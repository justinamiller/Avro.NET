using AvroNET.AvroObjectServices.FileHeader.Codec;
using AvroNET.AvroObjectServices.Read;
using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Features.DeserializeByLine.LineReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.DeserializeByLine
{
    internal class BaseLineReader<T> : ILineReader<T>
    {
        private readonly Reader _reader;
        private readonly byte[] _syncDate;
        private readonly AbstractCodec _codec;
        private readonly TypeSchema _writeSchema;
        private readonly TypeSchema _readSchema;
        private ILineReader<T> _lineReaderInternal;

        internal BaseLineReader(Reader reader, byte[] syncDate, AbstractCodec codec, TypeSchema writeSchema, TypeSchema readSchema)
        {
            _reader = reader;
            _syncDate = syncDate;
            _codec = codec;
            _writeSchema = writeSchema;
            _readSchema = readSchema;

            if (_reader.IsReadToEnd())
            {
                return;
            }

            LoadNextDataBlock();
        }


        public bool HasNext()
        {
            var hasNext = _lineReaderInternal != null && _lineReaderInternal.HasNext();

            if (!hasNext)
            {
                hasNext = !_reader.IsReadToEnd();

                if (hasNext)
                {
                    LoadNextDataBlock();
                    return _lineReaderInternal.HasNext();
                }
            }

            return hasNext;
        }

        private void LoadNextDataBlock()
        {
            var resolver = new Resolver(_writeSchema, _readSchema);

            var itemsCount = _reader.ReadLong();

            var dataBlock = _reader.ReadDataBlock(_syncDate, _codec);
            var dataReader = new Reader(new MemoryStream(dataBlock));


            if (itemsCount > 1)
            {
                _lineReaderInternal = new BlockLineReader<T>(dataReader, resolver, itemsCount);
                return;
            }

            if (_writeSchema.Type == AvroType.Array)
            {
                _lineReaderInternal = new ListLineReader<T>(dataReader, new Resolver(((ArraySchema)_writeSchema).ItemSchema, _readSchema));
                return;
            }

            _lineReaderInternal = new RecordLineReader<T>(dataReader, resolver);
        }

        public T ReadNext()
        {
            return _lineReaderInternal.ReadNext();
        }

        public void Dispose()
        {
            _lineReaderInternal?.Dispose();
        }
    }
}
