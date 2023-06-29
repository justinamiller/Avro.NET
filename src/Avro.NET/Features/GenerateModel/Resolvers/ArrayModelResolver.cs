using AvroNET;
using AvroNET.Features.GenerateModel.NetModel;
using AvroNET.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GenerateModel.Resolvers
{
    internal class ArrayModelResolver
    {
        internal NetClassField ResolveArray(JObject typeObj)
        {
            var avroField = new NetClassField();

            // If this is an array of a specific class that's being defined in this area of the json
            if (typeObj["items"] is JObject && ((JObject)typeObj["items"])["type"].ToString() == "record")
            {
                avroField.FieldType = ((JObject)typeObj["items"])["name"] + "[]";
                avroField.Namespace = ((JObject)typeObj["items"])["namespace"]?.ToString();
            }
            else if (typeObj["items"] is JValue value)
            {
                avroField.FieldType = value + "[]";
            }
            else
            {
                throw new InvalidAvroObjectException($"{typeObj}");
            }

            return avroField;
        }
    }
}
