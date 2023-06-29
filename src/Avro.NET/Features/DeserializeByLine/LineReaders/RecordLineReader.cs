using AvroNET.AvroObjectServices.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.DeserializeByLine.LineReaders
{
    internal class RecordLineReader<T> : ILineReader<T>
    {
        private readonly Reader _dataReader;
        private readonly Resolver _resolver;

        private bool _hasNext;

        public RecordLineReader(Reader dataReader, Resolver resolver)
        {
            _dataReader = dataReader;
            _resolver = resolver;
            _hasNext = true;
        }

        public bool HasNext()
        {
            if (_hasNext)
            {
                _hasNext = false;
                return true;
            }

            return _hasNext;
        }

        public T ReadNext()
        {
            return _resolver.Resolve<T>(_dataReader);
        }

        public void Dispose()
        {

        }
    }
}
