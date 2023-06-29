using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Schemas.AvroTypes;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AvroNET.Features.Serialize;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Union
    {
        internal Encoder.WriteItem Resolve(UnionSchema unionSchema)
        {
            var branchSchemas = unionSchema.Schemas.ToArray();
            var branchWriters = new Encoder.WriteItem[branchSchemas.Length];
            int branchIndex = 0;
            foreach (var branch in branchSchemas)
            {
                branchWriters[branchIndex++] = WriteResolver.ResolveWriter(branch);
            }


            return (v, e) => WriteUnion(unionSchema, branchSchemas, branchWriters, v, e);
        }

        /*TODO:
         * FIXME: This method of determining the Union branch has problems. If the data is IDictionary<string, object>
         * if there are two branches one with record schema and the other with map, it choose the first one. Similarly if
         * the data is byte[] and there are fixed and bytes schemas as branches, it choose the first one that matches.
         * Also it does not recognize the arrays of primitive types.
         */
        private bool UnionBranchMatches(TypeSchema typeSchema, object obj)
        {
            if (obj == null && typeSchema.Type != AvroType.Null) return false;
            switch (typeSchema.Type)
            {
                case AvroType.Null:
                    return obj == null;
                case AvroType.Boolean:
                    return obj is bool;
                case AvroType.Int:
                    return (obj is short or ushort or int or uint or char or byte or sbyte);
                case AvroType.Long:
                    return (obj is long or ulong);
                case AvroType.Float:
                    return obj is float;
                case AvroType.Double:
                    return obj is double;
                case AvroType.Bytes:
                    return obj is byte[];
                case AvroType.String:
                    return true;
                case AvroType.Error:
                    return true;
                case AvroType.Record:
                    {
                        var type = obj?.GetType();
                        if (type == null) return false;
                        return type.FullName.Equals((typeSchema as RecordSchema).FullName)
                               || (type.IsGenericType && type.Name.Contains("AnonymousType"))
                               || type == typeof(ExpandoObject);
                    }
                case AvroType.Enum:
                    return obj is System.Enum;
                case AvroType.Array:
                    return !(obj is byte[]);
                case AvroType.Map:
                    return true;
                case AvroType.Union:
                    return false; // Union directly within another union not allowed!
                case AvroType.Fixed:
                    //return obj is GenericFixed && (obj as GenericFixed)._schema.Equals(s);
                    return obj is AvroFixed &&
                           (obj as AvroFixed).Schema.FullName.Equals((typeSchema as FixedSchema).FullName);
                case AvroType.Logical:
                    // return (sc as LogicalTypeSchema).IsInstanceOfLogicalType(obj);
                    return true;
                default:
                    throw new AvroException("Unknown schema type: " + typeSchema.Type);
            }
        }

        private void WriteUnion(UnionSchema unionSchema, TypeSchema[] branchSchemas, Encoder.WriteItem[] branchWriters, object value, IWriter encoder)
        {
            int index = ResolveUnion(unionSchema, branchSchemas, value);
            encoder.WriteUnionIndex(index);
            branchWriters[index](value, encoder);
        }

        private int ResolveUnion(UnionSchema us, TypeSchema[] branchSchemas, object obj)
        {
            for (int i = 0; i < branchSchemas.Length; i++)
            {
                if (UnionBranchMatches(branchSchemas[i], obj)) return i;
            }

            throw new AvroException("Cannot find a match for " + obj.GetType() + " in " + us);
        }
    }
}
