using AvroNET.AvroObjectServices.Schemas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using AvroNET.Features.Serialize;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Json
    {
        internal Encoder.WriteItem Resolve(RecordSchema recordSchema)
        {
            WriteStep[] writeSteps = new WriteStep[recordSchema.Fields.Count];

            int index = 0;
            foreach (RecordFieldSchema field in recordSchema.Fields)
            {
                var record = new WriteStep
                {
                    WriteField = WriteResolver.ResolveWriter(field.TypeSchema),
                    FiledName = field.Aliases.FirstOrDefault() ?? field.Name
                };
                writeSteps[index++] = record;
            }

            void RecordResolver(object v, IWriter e)
            {
                WriteRecordFields(v, writeSteps, e);
            }


            return RecordResolver;
        }

        private void WriteRecordFields(object recordObj, WriteStep[] writers, IWriter encoder)
        {
            HandleJObject((JObject)recordObj, writers, encoder);
        }

        private void HandleJObject(JObject jObject, WriteStep[] writers, IWriter encoder)
        {
            foreach (var writer in writers)
            {
                object value = jObject.Properties().FirstOrDefault(x => x.Name == writer.FiledName)?.Value;

                switch (value)
                {
                    case JObject _:
                        break;
                    case JArray jArray:
                        value = jArray.ToObject<object[]>();
                        value = ((object[])value).Select(SwitchJsonType);
                        break;
                    case JValue jValue:
                        value = jValue.Value;
                        break;
                }

                writer.WriteField(value, encoder);
            }
        }

        private object SwitchJsonType(object value)
        {
            switch (value)
            {
                case JObject _:
                    break;
                case JArray jArray:
                    value = jArray.ToObject<object[]>();
                    break;
                case JValue jValue:
                    value = jValue.Value;
                    break;
            }

            return value;
        }
    }
}
