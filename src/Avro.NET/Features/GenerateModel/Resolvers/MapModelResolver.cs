using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GenerateModel.Resolvers
{
    internal class MapModelResolver
    {
        internal string ResolveMap(JObject typeObj)
        {
            string valueTypeString;
            var valueType = typeObj["values"];

            if (valueType is JArray)
            {
                if (valueType.Count() == 2
                    && string.Equals(valueType[0].ToString(), "Null", StringComparison.InvariantCultureIgnoreCase))
                {
                    valueTypeString = valueType[1] + "?";
                }
                else
                {
                    valueTypeString = "object";
                }
            }
            else
            {
                valueTypeString = valueType.ToString();
            }


            return $"Dictionary<string,{valueTypeString}>";

        }
    }
}
