using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Int
    {
        internal void Resolve(object value, IWriter encoder)
        {
            value ??= default(int);

            if (value is not int i)
            {
                try //Resolve short and uint
                {
                    encoder.WriteInt(Convert.ToInt32(value));
                    return;
                }
                catch
                {
                    throw new AvroTypeMismatchException("[Int] required to write against [Int] schema but found " + value.GetType());
                }
            }

            encoder.WriteInt(i);
        }
    }
}
