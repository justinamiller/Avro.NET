using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        private readonly Skipper _skipper;
        private readonly TypeSchema _readerSchema;
        private readonly TypeSchema _writerSchema;

        internal Resolver(TypeSchema writerSchema, TypeSchema readerSchema)
        {
            _readerSchema = readerSchema;
            _writerSchema = writerSchema;

            _skipper = new Skipper();
        }

        internal T Resolve<T>(IReader reader, long itemsCount = 0)
        {
            if (itemsCount > 1)
            {
                return (T)ResolveArray(
                        _writerSchema,
                        _readerSchema,
                        reader, typeof(T), itemsCount);
            }

            var result = Resolve(_writerSchema, _readerSchema, reader, typeof(T));
            return (T)result;
        }

        internal object Resolve(
            TypeSchema writerSchema,
            TypeSchema readerSchema,
            IReader reader,
            Type type)
        {
            try
            {
                switch (writerSchema.Type)
                {
                    case AvroType.Null:
                        return null;
                    case AvroType.Boolean:
                        return reader.ReadBoolean();
                    case AvroType.Int:
                        return ResolveInt(type, reader);
                    case AvroType.Long:
                        return ResolveLong(type, reader);
                    case AvroType.Float:
                        return ResolveFloat(type, reader);
                    case AvroType.Double:
                        return ResolveDouble(type, reader);
                    case AvroType.String:
                        return ResolveString(type, reader);
                    case AvroType.Bytes:
                        return reader.ReadBytes();
                    case AvroType.Logical:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveLogical((LogicalTypeSchema)writerSchema, readerSchema, reader, type);
                    case AvroType.Error:
                    case AvroType.Record:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveRecord((RecordSchema)writerSchema, (RecordSchema)readerSchema, reader, type);
                    case AvroType.Enum:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveEnum((EnumSchema)writerSchema, readerSchema, reader, type);
                    case AvroType.Fixed:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveFixed((FixedSchema)writerSchema, readerSchema, reader, type);
                    case AvroType.Array:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveArray(writerSchema, readerSchema, reader, type);
                    case AvroType.Map:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveMap((MapSchema)writerSchema, readerSchema, reader, type);
                    case AvroType.Union:
                        readerSchema = FindBranchReaderSchema(writerSchema, readerSchema);
                        return ResolveUnion((UnionSchema)writerSchema, readerSchema, reader, type);
                    default:
                        throw new AvroException("Unknown schema type: " + writerSchema);
                }
            }
            catch (Exception e)
            {
                throw new AvroTypeMismatchException($"Unable to deserialize [{writerSchema.Name}] of schema [{writerSchema.Type}] to the target type [{type}]. Inner exception:", e);
            }
        }

        private TypeSchema FindBranchReaderSchema(TypeSchema writerSchema, TypeSchema readerSchema)
        {
            if (readerSchema.Type == AvroType.Union && writerSchema.Type != AvroType.Union)
            {
                readerSchema = FindBranch(readerSchema as UnionSchema, writerSchema);
            }

            return readerSchema;
        }
    }
}
