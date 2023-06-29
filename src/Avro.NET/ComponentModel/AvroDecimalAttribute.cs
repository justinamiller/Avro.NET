using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.ComponentModel
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class AvroDecimalAttribute : Attribute
    {
        public virtual int Scale { get; set; }

        public virtual int Precision { get; set; }
    }
}
