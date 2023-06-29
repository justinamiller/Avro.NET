using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvroNET.AvroObjectServices.Read;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        protected virtual object ResolveUnion(UnionSchema writerSchema, TypeSchema readerSchema, IReader d, Type type)
        {
            int index = d.ReadUnionIndex();

            if (index >= writerSchema.Schemas.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(writerSchema.Schemas),
                    $"Cannot get union member of index [{index}]. Union size: [{writerSchema.Schemas.Count}]");
            }

            TypeSchema ws = writerSchema.Schemas[index];

            if (readerSchema is UnionSchema unionSchema)
                readerSchema = FindBranch(unionSchema, ws);
            else
            if (!readerSchema.CanRead(ws))
                throw new AvroException("Schema mismatch. Reader: " + _readerSchema + ", writer: " + _writerSchema);

            return Resolve(ws, readerSchema, d, type);
        }

        protected static TypeSchema FindBranch(UnionSchema us, TypeSchema writerSchema)
        {
            foreach (var readSchema in us.Schemas)
            {
                if (readSchema.CanRead(writerSchema))
                {
                    return readSchema;
                }
            }

            throw new AvroException("Unable to find matching schema for " + writerSchema + " in " + us);
        }
    }
}
