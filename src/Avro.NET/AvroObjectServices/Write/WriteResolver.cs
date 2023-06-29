using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Write.Resolvers;
using AvroNET.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using AvroNET.Features.Serialize;

namespace AvroNET.AvroObjectServices.Write
{
    internal static class WriteResolver
    {
        internal delegate void Writer<in T>(T t);

        private static readonly Resolvers.Array Array;
        private static readonly Map Map;
        private static readonly Null Null;
        private static readonly Resolvers.String String;
        private static readonly Record Record;
        private static readonly Resolvers.Enum Enum;
        private static readonly Fixed Fixed;
        private static readonly Union Union;
        private static readonly Long Long;
        private static readonly Uuid Uuid;
        private static readonly Resolvers.Decimal Decimal;
        private static readonly Duration Duration;
        private static readonly TimestampMilliseconds TimestampMilliseconds;
        private static readonly TimestampMicroseconds TimestampMicroseconds;
        private static readonly Json Json;
        private static readonly Int Int;
        private static readonly Date Date;
        private static readonly TimeMilliseconds TimeMilliseconds;
        private static readonly Bool Bool;

        static WriteResolver()
        {
            Array = new Resolvers.Array();
            Null = new Null();
            Map = new Map();
            String = new Resolvers.String();
            Record = new Record();
            Enum = new Resolvers.Enum();
            Fixed = new Fixed();
            Union = new Union();
            Long = new Long();
            Uuid = new Uuid();
            Decimal = new Resolvers.Decimal();
            Duration = new Duration();
            TimestampMilliseconds = new TimestampMilliseconds();
            TimestampMicroseconds = new TimestampMicroseconds();
            Json = new Json();
            Int = new Int();
            Date = new Date();
            TimeMilliseconds = new TimeMilliseconds();
            Bool = new Bool();
        }

        internal static Encoder.WriteItem ResolveWriter(TypeSchema schema)
        {
            switch (schema.Type)
            {
                case AvroType.Null:
                    return Null.Resolve;
                case AvroType.Boolean:
                    return Bool.Resolve;
                case AvroType.Int:
                    return Int.Resolve;
                case AvroType.Long:
                    return Long.Resolve;
                case AvroType.Float:
                    return (v, e) => Write<float>(v, schema.Type, e.WriteFloat);
                case AvroType.Double:
                    return (v, e) => Write<double>(v, schema.Type, e.WriteDouble);
                case AvroType.String:
                    return String.Resolve;
                case AvroType.Bytes:
                    return (v, e) => Write<byte[]>(v, schema.Type, e.WriteBytes);
                case AvroType.Error:
                case AvroType.Logical:
                    {
                        var logicalTypeSchema = (LogicalTypeSchema)schema;
                        switch (logicalTypeSchema.LogicalTypeName)
                        {
                            case LogicalTypeSchema.LogicalTypeEnum.Uuid:
                                return Uuid.Resolve((UuidSchema)logicalTypeSchema);
                            case LogicalTypeSchema.LogicalTypeEnum.Decimal:
                                return (v, e) => Decimal.Resolve((DecimalSchema)logicalTypeSchema, v, e);
                            case LogicalTypeSchema.LogicalTypeEnum.TimestampMilliseconds:
                                return (v, e) => TimestampMilliseconds.Resolve((TimestampMillisecondsSchema)logicalTypeSchema, v, e);
                            case LogicalTypeSchema.LogicalTypeEnum.TimestampMicroseconds:
                                return (v, e) => TimestampMicroseconds.Resolve((TimestampMicrosecondsSchema)logicalTypeSchema, v, e);
                            case LogicalTypeSchema.LogicalTypeEnum.Duration:
                                return (v, e) => Duration.Resolve((DurationSchema)logicalTypeSchema, v, e);
                            case LogicalTypeSchema.LogicalTypeEnum.Date:
                                return Date.Resolve();
                            case LogicalTypeSchema.LogicalTypeEnum.TimeMilliseconds:
                                return TimeMilliseconds.Resolve((TimeMillisecondsSchema)schema);
                        }
                    }
                    return String.Resolve;
                case AvroType.Record:
                    if (schema.RuntimeType == typeof(JObject))
                    {
                        return Json.Resolve((RecordSchema)schema);
                    }
                    return Record.Resolve((RecordSchema)schema);
                case AvroType.Enum:
                    return Enum.Resolve((EnumSchema)schema);
                case AvroType.Fixed:
                    return Fixed.Resolve((FixedSchema)schema);
                case AvroType.Array:
                    return Array.Resolve((ArraySchema)schema);
                case AvroType.Map:
                    return Map.Resolve((MapSchema)schema);
                case AvroType.Union:
                    return Union.Resolve((UnionSchema)schema);
                default:
                    return (v, e) =>
                        throw new AvroTypeMismatchException(
                            $"Tried to write against [{schema}] schema, but found [{v.GetType()}] type");
            }
        }

        /// <summary>
        /// A generic method to serialize primitive Avro types.
        /// </summary>
        /// <typeparam name="S">Type of the C# type to be serialized</typeparam>
        /// <param name="value">The value to be serialized</param>
        /// <param name="tag">The schema type tag</param>
        /// <param name="writer">The writer which should be used to write the given type.</param>
        private static void Write<S>(object value, AvroType tag, Writer<S> writer)
        {
            value ??= default(S);

            if (value is not S convertedValue)
            {
                throw new AvroTypeMismatchException(
                    $"[{typeof(S)}] required to write against [{tag}] schema but found type: [{value?.GetType()}]");
            }

            writer(convertedValue);
        }
    }
}
