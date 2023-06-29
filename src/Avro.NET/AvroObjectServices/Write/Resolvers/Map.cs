using AvroNET.AvroObjectServices.Schemas;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Map
    {
        internal Encoder.WriteItem Resolve(MapSchema mapSchema)
        {
            var itemWriter = WriteResolver.ResolveWriter(mapSchema.ValueSchema);
            return (v, e) => WriteMap(itemWriter, v, e);
        }

        private void WriteMap(Encoder.WriteItem itemWriter, object value, IWriter encoder)
        {
            EnsureMapObject(value);
            encoder.WriteMapStart();
            encoder.WriteItemCount(GetMapSize(value));
            WriteMapValues(value, itemWriter, encoder);
            encoder.WriteMapEnd();
        }

        private void EnsureMapObject(object value)
        {
            if (value == null || !(value is IDictionary)) if (value != null) throw new AvroException("[IDictionary] required to write against [Map] schema but found " + value.GetType());
        }

        private static long GetMapSize(object value)
        {
            return ((IDictionary)value).Count;
        }

        private void WriteMapValues(object map, Encoder.WriteItem valueWriter, IWriter encoder)
        {
            foreach (DictionaryEntry entry in ((IDictionary)map))
            {
                encoder.StartItem();
                encoder.WriteString(entry.Key.ToString());
                valueWriter(entry.Value, encoder);
            }
        }
    }
}
