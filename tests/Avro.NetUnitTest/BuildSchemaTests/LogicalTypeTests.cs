using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.AvroObjectServices.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AvroNETUnitTest.BuildSchemaTests
{
    public class LogicalTypeTests
    {
        [Theory]
        [InlineData(typeof(decimal), 29, 14)]
        public void BuildDecimalSchema(Type type, int precision, int scale)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(DecimalSchema));
            var decimalSchema = (DecimalSchema)schema;

            Assert.IsNotNull(schema);
            Assert.AreEqual(precision, decimalSchema.Precision);
            Assert.AreEqual(scale, decimalSchema.Scale);
        }

        [Theory]
        [InlineData(typeof(decimal?), 29, 14)]
        public void BuildNullableDecimalSchema(Type type, int precision, int scale)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(UnionSchema));
            var unionSchema = (UnionSchema)schema;

            Assert.IsInstanceOfType(unionSchema.Schemas[0], typeof(NullSchema));
            Assert.IsInstanceOfType(unionSchema.Schemas[1], typeof(DecimalSchema));

            var decimalSchema = (DecimalSchema)unionSchema.Schemas[1];

            Assert.IsNotNull(schema);
            Assert.AreEqual(precision, decimalSchema.Precision);
            Assert.AreEqual(scale, decimalSchema.Scale);
        }

        [Theory]
        [InlineData(typeof(Guid))]
        public void BuildGuidSchema(Type type)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(UuidSchema));
            var uuidSchema = (UuidSchema)schema;

            Assert.IsNotNull(uuidSchema);
        }

        [Theory]
        [InlineData(typeof(Guid?))]
        public void BuildNullableGuidSchema(Type type)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(UnionSchema));
            var unionSchema = (UnionSchema)schema;

            Assert.IsInstanceOfType(unionSchema.Schemas[0], typeof(NullSchema));
            Assert.IsInstanceOfType(unionSchema.Schemas[1], typeof(UuidSchema));

            var uuidSchema = (UuidSchema)unionSchema.Schemas[1];

            Assert.IsNotNull(uuidSchema);
        }

        [Theory]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DateTimeOffset))]
        public void BuildDateTimeSchema(Type type)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(TimestampMillisecondsSchema));
            var resolvedSchema = (TimestampMillisecondsSchema)schema;

            Assert.IsNotNull(resolvedSchema);
        }

        [Theory]
        [InlineData(typeof(DateTime?))]
        [InlineData(typeof(DateTimeOffset?))]
        public void BuildNullableDateTimeSchema(Type type)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(UnionSchema));
            var unionSchema = (UnionSchema)schema;

            Assert.IsInstanceOfType(unionSchema.Schemas[0], typeof(NullSchema));
            Assert.IsInstanceOfType(unionSchema.Schemas[1], typeof(TimestampMillisecondsSchema));

            var resolvedSchema = (TimestampMillisecondsSchema)unionSchema.Schemas[1];

            Assert.IsNotNull(resolvedSchema);
        }

        [Theory]
        [InlineData(typeof(TimeSpan))]
        public void BuildTimespanSchema(Type type)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(DurationSchema));
            var resolvedSchema = (DurationSchema)schema;

            Assert.IsNotNull(resolvedSchema);
        }

        [Theory]
        [InlineData(typeof(TimeSpan?))]
        public void BuildTimeSpanSchema(Type type)
        {
            //Act
            var builder = new ReflectionSchemaBuilder();
            TypeSchema schema = builder.BuildSchema(type);


            //Assert
            Assert.IsInstanceOfType(schema, typeof(UnionSchema));
            var unionSchema = (UnionSchema)schema;

            Assert.IsInstanceOfType(unionSchema.Schemas[0], typeof(NullSchema));
            Assert.IsInstanceOfType(unionSchema.Schemas[1], typeof(DurationSchema));

            var resolvedSchema = (DurationSchema)unionSchema.Schemas[1];

            Assert.IsNotNull(resolvedSchema);
        }
    }
}
