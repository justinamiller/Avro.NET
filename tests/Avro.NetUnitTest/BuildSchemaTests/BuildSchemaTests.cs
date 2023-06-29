using AvroNET.AvroObjectServices.BuildSchema;
using AvroNETUnitTest.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AvroNETUnitTest.BuildSchemaTests
{
    public class BuildSchemaTests
    {
        [Fact]
        public void BuildSchema_JsonSchemaContainsAvroAttributes_ResultIsEqualToInput()
        {
            //Arrange
            var schema = Schema.Create(typeof(AttributeClass));

            //Act
            var result = Schema.Create(schema.ToString());

            //Assert
            Assert.AreEqual(schema.ToString(), result.ToString());
        }
    }
}
