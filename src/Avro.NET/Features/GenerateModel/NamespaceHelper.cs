using AvroNET.Features.GenerateModel.NetModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvroNET.Features.GenerateModel
{
    internal class NamespaceHelper
    {
        internal void EnsureUniqueNames(NetModel.NetModel model)
        {
            foreach (IGrouping<string, INetType> netTypes in model.NetTypes.GroupBy(c => c.Name))
            {
                if (netTypes.Count() == 1)
                {
                    continue;
                }


                foreach (var netClass in netTypes.OfType<NetClass>().ToList())
                {
                    foreach (var avroField in model.NetTypes.OfType<NetClass>().ToList()
                        .SelectMany(c => c.Fields)
                        .Where(f => (f.FieldType == netClass.Name ||
                                    f.FieldType == netClass.Name + "[]" ||
                                    f.FieldType == netClass.Name + "?") &&
                                    f.Namespace == netClass.ClassNamespace))
                    {
                        avroField.FieldType = avroField.Namespace + avroField.FieldType;
                    }

                    netClass.Name = netClass.ClassNamespace + netClass.Name;
                }
            }
        }

        internal string ExtractNamespace(JObject typeObj, string longName, string shortName)
        {
            string @namespace = "";
            if (typeObj.ContainsKey("namespace"))
            {
                @namespace = typeObj["namespace"].ToString();
            }
            else
            {
                int place = longName.LastIndexOf(shortName, StringComparison.InvariantCulture);
                if (place >= 0)
                {
                    @namespace = longName.Remove(place, shortName.Length);
                }
            }

            @namespace = @namespace.Replace(".", "");

            return @namespace;
        }
    }
}
