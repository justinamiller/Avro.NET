using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        internal object ResolveInt(Type readType, IReader reader)
        {
            int value = reader.ReadInt();

            return readType == typeof(int) ? value : ConvertValue(readType, value);
        }

        private object ConvertValue(Type readType, object value)
        {
            switch (readType)
            {
                case not null when readType == typeof(int):
                case not null when readType == typeof(int?):
                    return Convert.ToInt32(value);

                case not null when readType == typeof(uint):
                case not null when readType == typeof(uint?):
                    return Convert.ToUInt32(value);

                case not null when readType == typeof(long):
                case not null when readType == typeof(long?):
                    return Convert.ToInt64(value);

                case not null when readType == typeof(ulong?):
                case not null when readType == typeof(ulong):
                    return Convert.ToUInt64(value);

                case not null when readType == typeof(short):
                case not null when readType == typeof(short?):
                    return Convert.ToInt16(value);

                case not null when readType == typeof(ushort):
                case not null when readType == typeof(ushort?):
                    return Convert.ToUInt16(value);

                case not null when readType == typeof(decimal):
                case not null when readType == typeof(decimal?):
                    return Convert.ToDecimal(value);

                case not null when readType == typeof(float):
                case not null when readType == typeof(float?):
                    return Convert.ToSingle(value);

                case not null when readType == typeof(double):
                case not null when readType == typeof(double?):
                    return Convert.ToDouble(value);

                case not null when readType == typeof(char?):
                case not null when readType == typeof(char):
                    return Convert.ToChar(value);

                case not null when readType == typeof(byte):
                case not null when readType == typeof(byte?):
                    return Convert.ToByte(value);

                case not null when readType == typeof(sbyte):
                case not null when readType == typeof(sbyte?):
                    return Convert.ToSByte(value);
            }

            return value;
        }
    }
}
