using AvroNET.AvroObjectServices.FileHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AvroNETUnitTest.FileHeader
{
    public class HeaderTests
    {
        [Fact]
        public void GetMetadata_ItExists_ValueIsReturned()
        {
            //Arrange
            var sut = new Header();
            var expectedValue = "xdddd";
            var key = "yo";

            sut.AddMetadata(key, System.Text.Encoding.UTF8.GetBytes(expectedValue));


            //Act
            var result = sut.GetMetadata(key);


            //Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Fact]
        public void GetMetadata_ItDoesNotExist_NullIsReturned()
        {
            //Arrange
            var sut = new Header();


            //Act
            var result = sut.GetMetadata("yyyyy");


            //Assert
            Assert.IsNull(result);
        }
    }
}
