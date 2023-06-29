using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Null
    {
        internal void Resolve(object value, IWriter encoder)
        {
            if (value != null)
            {
                throw new AvroTypeMismatchException("[Null] required to write against [Null] schema but found " + value.GetType());
            }
            encoder.WriteNull();
        }
    }
}
