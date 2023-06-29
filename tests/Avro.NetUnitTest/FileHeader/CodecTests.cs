using AvroNET.AvroObjectServices.FileHeader.Codec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AvroNETUnitTest.FileHeader
{
    public class CodecTests
    {
        [Fact]
        public void CreateCodecFromString_NonExistingString_DefaultCodecIsReturned()
        {
            //Arrange


            //Act
            var result = AbstractCodec.CreateCodecFromString("NonExistingCodec");


            //Assert
            Assert.IsInstanceOfType(result, typeof(NullCodec));
        }
    }
}
