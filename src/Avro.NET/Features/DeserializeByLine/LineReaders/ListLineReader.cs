using AvroNET.AvroObjectServices.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.DeserializeByLine.LineReaders
{
    internal class ListLineReader<T> : ILineReader<T>
    {
        private readonly IReader reader;
        private readonly Resolver resolver;
        private int itemsCount;

        public ListLineReader(IReader reader, Resolver resolver)
        {
            this.reader = reader;
            this.resolver = resolver;

            itemsCount = (int)reader.ReadArrayStart();
        }
        public bool HasNext()
        {
            if (itemsCount == 0)
            {
                itemsCount = (int)reader.ReadArrayNext();
                return itemsCount != 0;
            }
            else
            {
                return true;
            }
        }

        public T ReadNext()
        {
            var result = resolver.Resolve<T>(reader);
            itemsCount--;
            return result;
        }

        public void Dispose()
        {
        }
    }
}
