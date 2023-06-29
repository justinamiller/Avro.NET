using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Schemas;
using AvroNETUnitTest.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AvroNETUnitTest.BuildSchemaTests
{
    public class AttributesTests
    {
        [Fact]
        public void BuildSchema_RecordWithAttributes_AttributesAreAppliedToSchema()
        {
            //Arrange
            var builder = new ReflectionSchemaBuilder();


            //Act
            TypeSchema schema = builder.BuildSchema(typeof(AttributeClass));


            //Assert
            Assert.IsInstanceOfType(schema, typeof(RecordSchema));
            var resultSchema = (RecordSchema)schema;

            Assert.IsNotNull(resultSchema);
            Assert.AreEqual("This is Doc of User Class", resultSchema.Doc);
            Assert.AreEqual("User", resultSchema.Name);
            Assert.AreEqual("userspace", resultSchema.Namespace);

            var stringField = resultSchema.Fields.SingleOrDefault(f => f.Name == "name");
            Assert.IsNotNull(stringField);
            Assert.AreEqual("This is Doc of record field", stringField.Doc);

            var intField = resultSchema.Fields.SingleOrDefault(f => f.Name == "favorite_number");
            Assert.IsNotNull(intField);
            Assert.IsTrue(intField.HasDefaultValue);
            Assert.AreEqual(2137, intField.DefaultValue);


            var decimalField = resultSchema.Fields.SingleOrDefault(f => f.Name == nameof(AttributeClass.AvroDecimal));
            Assert.IsInstanceOfType(decimalField.TypeSchema, typeof(DecimalSchema));
            var decimalSchema = (DecimalSchema)decimalField.TypeSchema;
            Assert.AreEqual(2, decimalSchema.Scale);
            Assert.AreEqual(4, decimalSchema.Precision);
        }
    }
}
