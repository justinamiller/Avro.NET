using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas.Abstract
{
    internal abstract class PrimitiveTypeSchema : TypeSchema
    {
        protected PrimitiveTypeSchema(Type runtimeType, Dictionary<string, string> attributes)
            : base(runtimeType, attributes)
        {
        }

        internal override void ToJsonSafe(JsonTextWriter writer, HashSet<NamedSchema> seenSchemas)
        {
            writer.WriteValue(CultureInfo.InvariantCulture.TextInfo.ToLower(this.Type.ToString()));
        }

        internal override bool CanRead(TypeSchema writerSchema)
        {
            if (writerSchema is UnionSchema || Type == writerSchema.Type) return true;
            AvroType t = writerSchema.Type;
            switch (Type)
            {
                case AvroType.Double:
                    return t == AvroType.Int || t == AvroType.Long || t == AvroType.Float;
                case AvroType.Float:
                    return t == AvroType.Int || t == AvroType.Long;
                case AvroType.Long:
                    return t == AvroType.Int;
                case AvroType.String:
                    return t == AvroType.String || t == AvroType.Null;
                case AvroType.Int:
                    return t == AvroType.Long;
                default:
                    return false;
            }
        }
    }
}
