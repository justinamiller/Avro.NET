using AvroNET.AvroObjectServices.BuildSchema;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AvroNETUnitTest
{
    public class DeserializeLogicalDateTests
    {
        [Fact]
        public void GivenDateTimeProperty_WhenUsingSchemaWithTimeAsTimestampMicroseconds_ThenShouldWork()
        {
            //Arrange
            var toSerialize = new ClassWithDateTime { ArriveBy = DateTime.UtcNow };

            //Act
            var schema = Schema.Create(toSerialize);

            // Change schema logical type from timestamp-millis to timestamp-micros (a bit hacky)
            var microsecondsSchema = schema.ToString().Replace(LogicalTypeSchema.LogicalTypeEnum.TimestampMilliseconds, LogicalTypeSchema.LogicalTypeEnum.TimestampMicroseconds);

            var result = AvroConvert.SerializeHeadless(toSerialize, microsecondsSchema);

            var avro2Json = AvroConvert.Avro2Json(result, microsecondsSchema);
            var deserialized = JsonConvert.DeserializeObject<ClassWithDateTime>(avro2Json);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(toSerialize.ArriveBy.Millisecond, deserialized.ArriveBy.Millisecond);
            Assert.AreEqual(toSerialize.ArriveBy.Second, deserialized.ArriveBy.Second);
            Assert.AreEqual(toSerialize.ArriveBy.Minute, deserialized.ArriveBy.Minute);
            Assert.AreEqual(toSerialize.ArriveBy.Hour, deserialized.ArriveBy.Hour);
            Assert.AreEqual(toSerialize.ArriveBy.Day, deserialized.ArriveBy.Day);
            Assert.AreEqual(toSerialize.ArriveBy.Month, deserialized.ArriveBy.Month);
            Assert.AreEqual(toSerialize.ArriveBy.Year, deserialized.ArriveBy.Year);
        }

        public class ClassWithDateTime
        {
            public DateTime ArriveBy { get; set; }
        }
    }
}
