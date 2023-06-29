using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.Merge
{
    internal class DataBlock
    {
        internal long ItemsCount { get; set; }
        internal byte[] Data { get; set; }
    }
}
