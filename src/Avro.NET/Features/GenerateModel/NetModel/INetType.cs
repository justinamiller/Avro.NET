using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GenerateModel.NetModel
{
    internal interface INetType
    {
        string Name { get; set; }
        void Write(StringBuilder sb);
    }
}
