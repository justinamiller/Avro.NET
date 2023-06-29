using AvroNET.Features.GenerateModel.NetModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GenerateModel.Resolvers
{
    internal class EnumModelResolver
    {
        internal void ResolveEnum(JToken propValue, NetModel.NetModel model)
        {
            var result = new NetEnum();

            var name = propValue["name"].ToString().Split('.').Last();
            var symbols = (JArray)propValue["symbols"];

            result.Name = name;
            result.Symbols = symbols.Select(s => s.ToString()).ToList();

            model.NetTypes.Add(result);
        }
    }
}
