using AvroNET.AvroObjectServices.Schemas;
using AvroNET.Features.Serialize;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Array
    {
        internal Encoder.WriteItem Resolve(ArraySchema schema)
        {
            var itemWriter = WriteResolver.ResolveWriter(schema.ItemSchema);
            return (d, e) => WriteArray(itemWriter, d, e);
        }

        private void WriteArray(Encoder.WriteItem itemWriter, object @object, IWriter encoder)
        {
            List<object> list = EnsureArrayObject(@object);

            long l = list?.Count ?? 0;
            encoder.WriteArrayStart();
            encoder.WriteItemCount(l);
            WriteArrayValues(list, itemWriter, encoder, l);
            encoder.WriteArrayEnd();
        }

        private List<object> EnsureArrayObject(object value)
        {
            var enumerable = value as IEnumerable;
            List<object> list = enumerable?.Cast<object>().ToList();

            return list;
        }

        private void WriteArrayValues(List<object> list, Encoder.WriteItem itemWriter, IWriter encoder, long count)
        {
            if (list == null)
            {
                itemWriter(null, encoder);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    itemWriter(list[i], encoder);
                }
            }
        }
    }
}
