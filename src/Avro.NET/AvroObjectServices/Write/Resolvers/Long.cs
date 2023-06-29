using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Long
    {
        internal void Resolve(object value, IWriter encoder)
        {
            if (value is not long converted)
            {
                try //Resolve ulong
                {
                    encoder.WriteLong(Convert.ToInt64(value));
                    return;
                }
                catch
                {
                    throw new AvroTypeMismatchException("[Long] required to write against [Long] schema but found " + value.GetType());
                }
            }

            encoder.WriteLong(converted);
        }
    }
}
