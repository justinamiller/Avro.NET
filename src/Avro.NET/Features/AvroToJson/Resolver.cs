﻿using AvroNET.AvroObjectServices.Read;
using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Schemas.AvroTypes;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.AvroToJson
{
    internal partial class Resolver
    {
        private readonly TypeSchema _readerSchema;

        internal Resolver(TypeSchema readerSchema)
        {
            _readerSchema = readerSchema;
        }

        internal object Resolve(IReader reader)
        {
            var result = Resolve(_readerSchema, reader);
            return result;
        }

        internal object Resolve(TypeSchema readerSchema, IReader d)
        {
            switch (readerSchema.Type)
            {
                case AvroType.Null:
                    return null;
                case AvroType.Boolean:
                    return d.ReadBoolean();
                case AvroType.Int:
                    return d.ReadInt();
                case AvroType.Long:
                    return ResolveLong(d);
                case AvroType.Float:
                    return d.ReadFloat();
                case AvroType.Double:
                    return d.ReadDouble();
                case AvroType.String:
                    return ResolveString(d);
                case AvroType.Bytes:
                    return d.ReadBytes();
                case AvroType.Error:
                case AvroType.Record:
                    return ResolveRecord((RecordSchema)readerSchema, d);
                case AvroType.Enum:
                    return ResolveEnum((EnumSchema)readerSchema, d);
                case AvroType.Fixed:
                    return ResolveFixed((FixedSchema)readerSchema, d);
                case AvroType.Array:
                    return ResolveArray(readerSchema, d);
                case AvroType.Map:
                    return ResolveMap((MapSchema)readerSchema, d);
                case AvroType.Union:
                    return ResolveUnion((UnionSchema)readerSchema, d);
                case AvroType.Logical:
                    return ResolveLogical((LogicalTypeSchema)readerSchema, d);
                default:
                    throw new AvroException("Unknown schema type: " + readerSchema);
            }

        }

        protected object ResolveLong(IReader reader)
        {
            long value = reader.ReadLong();

            return value;
        }

        protected object ResolveString(IReader reader)
        {
            var value = reader.ReadString();
            return value;
        }

        protected virtual Dictionary<string, object> ResolveRecord(RecordSchema readerSchema, IReader dec)
        {
            var result = new Dictionary<string, object>();

            foreach (var rf in readerSchema.Fields)
            {
                string name = rf.Name;
                object value = Resolve(rf.TypeSchema, dec) ?? rf.DefaultValue;

                result.Add(name, value);
            }

            return result;
        }

        protected virtual object ResolveFixed(FixedSchema readerSchema, IReader d)
        {
            AvroFixed ru = new AvroFixed(readerSchema);
            byte[] bb = ((AvroFixed)ru).Value;
            d.ReadFixed(bb);
            return ru.Value;
        }

        protected virtual object ResolveEnum(EnumSchema readerSchema, IReader d)
        {
            int position = d.ReadEnum();
            string value = readerSchema.Symbols[position];
            return value;
        }

        protected virtual object ResolveMap(MapSchema readerSchema, IReader d)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            for (int n = (int)d.ReadMapStart(); n != 0; n = (int)d.ReadMapNext())
            {
                for (int j = 0; j < n; j++)
                {
                    string k = d.ReadString();
                    result.Add(k, Resolve(readerSchema.ValueSchema, d));
                }
            }

            return result;
        }

        internal object ResolveArray(TypeSchema readerSchema, IReader d)
        {
            if (readerSchema.Type == AvroType.Array)
            {
                readerSchema = ((ArraySchema)readerSchema).ItemSchema;
            }

            if (readerSchema.Name == "KeyValuePair2")
            {
                return ResolveDictionary((RecordSchema)readerSchema, d);
            }

            object[] result = Array.Empty<object>();
            int i = 0;

            for (int n = (int)d.ReadArrayStart(); n != 0; n = (int)d.ReadArrayNext())
            {
                if (result.Length < i + n)
                {
                    Array.Resize(ref result, i + n);
                }

                for (int j = 0; j < n; j++, i++)
                {
                    result[i] = Resolve(readerSchema, d);
                }
            }

            return result;
        }

        protected object ResolveDictionary(RecordSchema readerSchema, IReader d)
        {
            dynamic resultDictionary = new Dictionary<object, object>();

            for (int n = (int)d.ReadArrayStart(); n != 0; n = (int)d.ReadArrayNext())
            {
                for (int j = 0; j < n; j++)
                {
                    dynamic key = Resolve(readerSchema.GetField("Key").TypeSchema, d);
                    dynamic value = Resolve(readerSchema.GetField("Value").TypeSchema, d);
                    resultDictionary.Add(key, value);
                }
            }
            return resultDictionary;
        }

        protected virtual object ResolveUnion(UnionSchema readerSchema, IReader d)
        {
            int index = d.ReadUnionIndex();
            TypeSchema ws = readerSchema.Schemas[index];
            return Resolve(FindBranch(readerSchema, ws), d);
        }

        protected static TypeSchema FindBranch(UnionSchema us, TypeSchema schema)
        {
            var resultSchema = us.Schemas.FirstOrDefault(s => s.Name == schema.Name);

            if (resultSchema == null)
            {
                throw new AvroException("No matching schema for " + schema + " in " + us);
            }

            return resultSchema;
        }

        private object ResolveLogical(LogicalTypeSchema readerSchema, IReader reader)
        {
            var baseValue = Resolve(readerSchema.BaseTypeSchema, reader);
            return readerSchema.ConvertToLogicalValue(baseValue, readerSchema, readerSchema.RuntimeType);
        }
    }
}
