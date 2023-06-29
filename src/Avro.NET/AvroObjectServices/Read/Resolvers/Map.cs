using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        protected virtual object ResolveMap(MapSchema writerSchema, TypeSchema readerSchema, IReader d, Type type)
        {
            var containingTypes = type.GetGenericArguments();
            dynamic result = Activator.CreateInstance(type);

            TypeSchema stringSchema = new StringSchema();

            MapSchema rs = (MapSchema)readerSchema;
            for (int n = (int)d.ReadMapStart(); n != 0; n = (int)d.ReadMapNext())
            {
                for (int j = 0; j < n; j++)
                {
                    dynamic key = Resolve(stringSchema, stringSchema, d, containingTypes[0]);
                    dynamic value = Resolve(writerSchema.ValueSchema, rs.ValueSchema, d, containingTypes[1]);
                    result.Add(key, value);
                }
            }

            return result;
        }
    }
}
