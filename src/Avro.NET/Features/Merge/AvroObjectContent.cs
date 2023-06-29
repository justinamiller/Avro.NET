using AvroNET.AvroObjectServices.FileHeader.Codec;
using AvroNET.AvroObjectServices.FileHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.Merge
{
    internal class AvroObjectContent
    {
        internal Header Header { get; set; }
        internal List<DataBlock> DataBlocks { get; set; }
        internal AbstractCodec Codec { get; set; }

        internal AvroObjectContent()
        {
            DataBlocks = new List<DataBlock>();
        }
    }
}
