﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GenerateModel.NetModel
{
    internal class NetClass : INetType
    {
        public string Name { get; set; }
        public string ClassNamespace { get; set; }
        public List<NetClassField> Fields { get; set; } = new List<NetClassField>();

        public void Write(StringBuilder sb)
        {
            sb.AppendLine($"public class {Name}");
            sb.AppendLine("{");
            foreach (NetClassField field in Fields)
            {
                if (!string.IsNullOrWhiteSpace(field.Doc))
                {
                    sb.AppendLine("\t/// <summary>");
                    sb.AppendLine($"\t/// {field.Doc}");
                    sb.AppendLine("\t/// </summary>");
                }

                if (!string.IsNullOrWhiteSpace(field.Default))
                {
                    string defaultVal;
                    switch (field.FieldType)
                    {
                        case "string":
                            defaultVal = $"\"{field.Default}\"";
                            break;
                        case "bool":
                            bool.TryParse(field.Default, out bool parsedVal);
                            defaultVal = $"{parsedVal.ToString().ToLower()}";
                            break;
                        default:
                            defaultVal = $"{field.Default}";
                            break;
                    }
                    sb.AppendLine($"	[DefaultValue({defaultVal})]");
                }
                sb.AppendLine($"	public {field.FieldType} {field.Name} {{ get; set; }}");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }
    }
}
