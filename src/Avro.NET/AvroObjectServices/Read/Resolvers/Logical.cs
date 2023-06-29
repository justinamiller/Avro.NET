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
        private object ResolveLogical(LogicalTypeSchema writeSchema, TypeSchema readSchema, IReader reader, Type type)
        {
            var baseWriteSchema = writeSchema.BaseTypeSchema;

            var baseReadSchema = readSchema;
            if (readSchema is LogicalTypeSchema logicalReadSchema)
            {
                baseReadSchema = logicalReadSchema.BaseTypeSchema;
            }

            var value = Resolve(baseWriteSchema, baseReadSchema, reader, type);

            var result = writeSchema.ConvertToLogicalValue(value, writeSchema, type);

            return result;
        }
    }
}
