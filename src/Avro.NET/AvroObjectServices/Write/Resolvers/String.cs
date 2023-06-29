using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class String
    {
        internal void Resolve(object value, IWriter writer)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (value is not string convertedValue)
                convertedValue = value.ToString();

            writer.WriteString(convertedValue);
        }
    }
}
