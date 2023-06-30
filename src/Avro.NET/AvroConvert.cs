using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Features.Deserialize;
using AvroNET.Features.GenerateModel;
using AvroNET.Features.GetSchema;
using AvroNET.Features.JsonToAvro;
using AvroNET.Features.Merge;
using Newtonsoft.Json;
using AvroNET.Features.Serialize;
using AvroNET.AvroObjectServices.Write;
using AvroNET.AvroObjectServices.Read;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

//for unit tests
[assembly: InternalsVisibleTo("Avro.NetUnitTest")]
namespace AvroNET
{
    public static class AvroConvert
    {
        #region Avro2Json
        /// <summary>
        /// Converts Avro object directly to JSON format
        /// </summary>
        public static string Avro2Json(byte[] avro)
        {
            using (var stream = new MemoryStream(avro))
            {
                var decoder = new Features.AvroToJson.Decoder();
                var deserialized = decoder.Decode(stream, null);
                var json = JsonConvert.SerializeObject(deserialized);

                return json;
            }
        }


        /// <summary>
        /// Converts Avro object compatible with given <paramref name="avroSchema"/> directly to JSON format
        /// </summary>
        public static string Avro2Json(byte[] avro, string avroSchema)
        {
            using (var stream = new MemoryStream(avro))
            {
                var decoder = new Features.AvroToJson.Decoder();
                var deserialized = decoder.Decode(stream, Schema.Create(avroSchema));
                var json = JsonConvert.SerializeObject(deserialized);

                return json;
            }
        }
        #endregion

        #region Deserialize
        /// <summary>
        /// Deserializes Avro object to .NET type
        /// </summary>
        public static T Deserialize<T>(byte[] avroBytes)
        {
            using (var stream = new MemoryStream(avroBytes))
            {
                var decoder = new Decoder();
                var deserialized = decoder.Decode<T>(
                    stream,
                    Schema.Create(typeof(T))
                );
                return deserialized;
            }
        }

        /// <summary>
        /// Deserializes Avro object to .NET type
        /// </summary>
        public static dynamic Deserialize(byte[] avroBytes, Type targetType)
        {
            object result = typeof(AvroConvert)
                            .GetMethod(nameof(Deserialize), new[] { typeof(byte[]) })
                            ?.MakeGenericMethod(targetType)
                            .Invoke(null, new object[] { avroBytes });

            return result;
        }
        #endregion

        #region DeserializeByLine
        /// <summary>
        /// Opens Avro object deserializer which allows to read large collection of Avro objects one by one
        /// </summary>
        public static ILineReader<T> OpenDeserializer<T>(Stream stream)
        {
            var reader = Features.DeserializeByLine.Decoder.OpenReader<T>(stream, Schema.Create(typeof(T)));

            return reader;
        }
        #endregion

        #region DeserializeHeadless
        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static T DeserializeHeadless<T>(byte[] avroBytes, string schema)
        {
            return DeserializeHeadless<T>(avroBytes, Schema.Create(schema), BuildSchema(typeof(T)), 1);
        }

        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static T DeserializeHeadless<T>(byte[] avroBytes, string schema, int numberOfRows)
        {
            return DeserializeHeadless<T>(avroBytes, Schema.Create(schema), BuildSchema(typeof(T)), numberOfRows);
        }

        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static T DeserializeHeadless<T>(byte[] avroBytes)
        {
            return DeserializeHeadless<T>(avroBytes, BuildSchema(typeof(T)), BuildSchema(typeof(T)), 1);
        }

        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static dynamic DeserializeHeadless(byte[] avroBytes, Type targetType)
        {
            object result = typeof(AvroConvert)
                .GetMethod(nameof(DeserializeHeadless), new[] { typeof(byte[]) })
                ?.MakeGenericMethod(targetType)
                .Invoke(null, new object[] { avroBytes });

            return result;
        }

        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static dynamic DeserializeHeadless(byte[] avroBytes, string schema, Type targetType)
        {
            object result = typeof(AvroConvert)
                .GetMethod(nameof(DeserializeHeadless), new[] { typeof(byte[]), typeof(string) })
                ?.MakeGenericMethod(targetType)
                .Invoke(null, new object[] { avroBytes, schema });

            return result;
        }


        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static T DeserializeHeadless<T>(byte[] avroBytes, string writeSchema, string readSchema, int numberOfRows)
        {
            return DeserializeHeadless<T>(avroBytes, Schema.Create(writeSchema), Schema.Create(readSchema), numberOfRows);
        }


        /// <summary>
        /// Deserializes Avro object, which does not contain header, to .NET type
        /// </summary>
        public static dynamic DeserializeHeadless(byte[] avroBytes, string writeSchema, string readSchema, Type targetType)
        {
            var result = typeof(AvroConvert)
                .GetMethod(nameof(DeserializeHeadless), new[] { typeof(byte[]), typeof(string), typeof(string), typeof(int) })
                ?.MakeGenericMethod(targetType)
                .Invoke(null, new object[] { avroBytes, writeSchema, readSchema, 1 });

            return result;
        }

        private static T DeserializeHeadless<T>(byte[] avroBytes, TypeSchema writeSchema, TypeSchema readSchema, int numberOfRows)
        {
            var reader = new Reader(new MemoryStream(avroBytes));
            var resolver = new Resolver(writeSchema, readSchema);
            var result = resolver.Resolve<T>(reader, numberOfRows);

            return result;
        }

        private static TypeSchema BuildSchema(Type type)
        {
            var schemaBuilder = new ReflectionSchemaBuilder(new AvroSerializerSettings());
            return schemaBuilder.BuildSchema(type);
        }
        #endregion

        #region GenerateModel
        /// <summary>
        /// Generates C# .NET classes from given AVRO object containing schema
        /// </summary>
        public static string GenerateModel(byte[] avroBytes)
        {
            var generateClassHandler = new GenerateModel();
            var result = generateClassHandler.FromAvroObject(avroBytes);

            return result;
        }

        /// <summary>
        /// Generates C# .NET classes from given AVRO schema
        /// </summary>
        public static string GenerateModel(string schema)
        {
            var generateClassHandler = new GenerateModel();
            var result = generateClassHandler.FromAvroSchema(schema);

            return result;
        }
        #endregion

        #region GenerateSchema
        /// <summary>
        /// Generates schema for given .NET Type
        /// </summary>
        public static string GenerateSchema(Type type)
        {
            var schemaBuilder = new ReflectionSchemaBuilder(new AvroSerializerSettings());
            var schema = schemaBuilder.BuildSchema(type);

            return schema.ToString();
        }


        /// <summary>
        /// Generates schema for given .NET Type
        /// <paramref name="includeOnlyDataContractMembers"/> indicates if only classes with DataContractAttribute and properties marked with DataMemberAttribute should be returned
        /// </summary>
        public static string GenerateSchema(Type type, bool includeOnlyDataContractMembers)
        {
            var builder = new ReflectionSchemaBuilder(new AvroSerializerSettings(includeOnlyDataContractMembers));
            var schema = builder.BuildSchema(type);

            return schema.ToString();
        }
        #endregion

        #region GetSchema
        /// <summary>
        /// Extracts data schema from given AVRO object
        /// </summary>
        public static string GetSchema(byte[] avroBytes)
        {
            using (var stream = new MemoryStream(avroBytes))
            {
                var headerDecoder = new HeaderDecoder();
                var schema = headerDecoder.GetSchema(stream);

                return schema;
            }
        }


        /// <summary>
        /// Extracts data schema from AVRO file under given path
        /// </summary>
        public static string GetSchema(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var headerDecoder = new HeaderDecoder();
                var schema = headerDecoder.GetSchema(stream);

                return schema;
            }
        }


        /// <summary>
        /// Extracts data schema from given AVRO stream
        /// </summary>
        public static string GetSchema(Stream avroStream)
        {
            var headerDecoder = new HeaderDecoder();
            var schema = headerDecoder.GetSchema(avroStream);

            return schema;
        }

        /// <summary>
        /// Generates Avro schema for given JSON serialized object
        /// </summary>
        ///

        public static string GenerateFromJson(string json)
        {
            var schemaBuilder = new JsonSchemaBuilder();
            var token = JToken.Parse(json);
            var schema = schemaBuilder.BuildSchema(token);

            return schema.ToString();
        }
        #endregion

        #region Json2Avro
        /// <summary>
        /// Converts JSON object directly to Avro format
        /// Warning! The dynamic implementations is an experimental feature. Use generic overload if possible.
        /// </summary>
        public static byte[] Json2Avro(string json)
        {
            var decoder = new JsonToAvroDecoder();
            return decoder.DecodeJson(json, CodecType.None);

        }

        /// <summary>
        /// Converts JSON object directly to Avro format
        /// Warning! The dynamic implementations is an experimental feature. Use generic overload if possible.
        /// </summary>
        public static byte[] Json2Avro(string json, CodecType codecType)
        {
            var decoder = new JsonToAvroDecoder();
            return decoder.DecodeJson(json, codecType);

        }
        #endregion

        #region Merge
        /// <summary>
        /// Merge multiple Avro objects of type T into one Avro object of type IEnumerable T
        /// </summary>
        public static byte[] Merge<T>(IEnumerable<byte[]> avroObjects)
        {
            var itemSchema = Schema.Create(typeof(T));
            var targetSchema = Schema.Create(typeof(List<T>));
            var mergeDecoder = new MergeDecoder();

            List<DataBlock> avroDataBlocks = new List<DataBlock>();

            avroObjects = avroObjects.ToList();
            for (int i = 0; i < avroObjects.Count(); i++)
            {
                var avroFileContent = mergeDecoder.ExtractAvroObjectContent(avroObjects.ElementAt(i));
                if (!itemSchema.CanRead(avroFileContent.Header.Schema))
                {
                    throw new InvalidAvroObjectException($"Schema from object of index [{i}] is not compatible with schema of type [{typeof(T)}]");
                }

                avroDataBlocks.AddRange(avroFileContent.DataBlocks);
            }

            using (MemoryStream resultStream = new MemoryStream())
            {
                using (var encoder = new MergeEncoder(resultStream))
                {
                    encoder.WriteHeader(targetSchema.ToString(), CodecType.None);

                    encoder.WriteData(avroDataBlocks);
                }

                var result = resultStream.ToArray();
                return result;

            }
        }
        #endregion

        #region Serialize
        /// <summary>
        /// Serializes given object to Avro format (including header with metadata)
        /// </summary>
        public static byte[] Serialize(object obj)
        {
            return Serialize(obj, CodecType.None);
        }

        /// <summary>
        /// Serializes given object to Avro format (including header with metadata)
        /// Choosing <paramref name="codecType"/> reduces output object size
        /// </summary>
        public static byte[] Serialize(object obj, CodecType codecType)
        {
            using MemoryStream resultStream = new MemoryStream();
            var schema = Schema.Create(obj);
            using (var writer = new Encoder(schema, resultStream, codecType))
            {
                writer.Append(obj);
            }
            byte[] result = resultStream.ToArray();
            return result;
        }
        #endregion

        #region SerializeHeadless
        /// <summary>
        /// Serializes given object to Avro format - <c>excluding</c> header
        /// </summary>
        public static byte[] SerializeHeadless(object obj, string schema)
        {
            MemoryStream resultStream = new MemoryStream();
            var encoder = new Writer(resultStream);
            var schemaObject = Schema.Create(schema);
            var writer = WriteResolver.ResolveWriter(schemaObject);

            writer(obj, encoder);

            var result = resultStream.ToArray();
            return result;
        }

        /// <summary>
        /// Serializes given object to Avro format - <c>excluding</c> header
        /// </summary>
        public static byte[] SerializeHeadless(object obj, Type objectType)
        {
            MemoryStream resultStream = new MemoryStream();
            var encoder = new Writer(resultStream);
            var schemaObject = BuildSchema(objectType);
            var writer = WriteResolver.ResolveWriter(schemaObject);

            writer(obj, encoder);

            var result = resultStream.ToArray();
            return result;
        }
        #endregion
    }//end class
}//end namespace
