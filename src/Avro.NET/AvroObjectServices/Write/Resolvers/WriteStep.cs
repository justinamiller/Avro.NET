using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvroNET.Features.Serialize.Encoder;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class WriteStep
    {
        internal WriteItem WriteField { get; set; }
        internal string FiledName { get; set; }
    }
}
