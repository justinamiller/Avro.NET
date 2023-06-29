using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas
{
    internal enum AvroType
    {
        Null,
        Boolean,
        Int,
        Long,
        Float,
        Double,
        Bytes,
        String,
        Record,
        Enum,
        Array,
        Map,
        Union,
        Fixed,
        Error,
        Logical
    }
}
