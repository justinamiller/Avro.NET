using AvroNET.AvroObjectServices.Schemas;
using AvroNET.AvroObjectServices.Schemas.AvroTypes;
using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Write.Resolvers
{
    internal class Decimal
    {
        internal void Resolve(DecimalSchema schema, object logicalValue, IWriter writer)
        {
            var avroDecimal = new AvroDecimal((decimal)logicalValue);
            var logicalScale = schema.Scale;
            var scale = avroDecimal.Scale;

            //Resize value to match schema Scale property
            int sizeDiff = logicalScale - scale;
            if (sizeDiff < 0)
            {
                throw new AvroTypeException(
                    $@"Decimal Scale for value [{logicalValue}] is equal to [{scale}]. This exceeds default setting [{logicalScale}].
Consider adding following attribute to your property:
[AvroDecimal(Precision = 28, Scale = {scale})]
");
            }

            string trailingZeros = new string('0', sizeDiff);
            var logicalValueString = logicalValue.ToString();

            string valueWithTrailingZeros;
            if (logicalValueString.Contains(avroDecimal.SeparatorCharacter.ToString()))
            {
                valueWithTrailingZeros = $"{logicalValue}{trailingZeros}";
            }
            else
            {
                valueWithTrailingZeros = $"{logicalValue}{avroDecimal.SeparatorCharacter}{trailingZeros}";
            }

            avroDecimal = new AvroDecimal(valueWithTrailingZeros);

            var buffer = avroDecimal.UnscaledValue.ToByteArray();
            System.Array.Reverse(buffer);

            var result = AvroType.Bytes == schema.BaseTypeSchema.Type
                ? (object)buffer
                : (object)new AvroFixed(
                    (FixedSchema)schema.BaseTypeSchema,
                    GetDecimalFixedByteArray(buffer, ((FixedSchema)schema.BaseTypeSchema).Size,
                        avroDecimal.Sign < 0 ? (byte)0xFF : (byte)0x00));

            writer.WriteBytes((byte[])result);
        }


        private static byte[] GetDecimalFixedByteArray(byte[] sourceBuffer, int size, byte fillValue)
        {
            var paddedBuffer = new byte[size];

            var offset = size - sourceBuffer.Length;

            for (var idx = 0; idx < size; idx++)
            {
                paddedBuffer[idx] = idx < offset ? fillValue : sourceBuffer[idx - offset];
            }

            return paddedBuffer;
        }
    }
}
