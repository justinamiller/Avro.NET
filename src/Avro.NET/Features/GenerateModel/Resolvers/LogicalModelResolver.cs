using AvroNET;
using AvroNET.AvroObjectServices.Schemas.Abstract;
using AvroNET.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Features.GenerateModel.Resolvers
{
    internal class LogicalModelResolver
    {
        internal string ResolveLogical(JObject typeObj)
        {
            string logicalType = typeObj["logicalType"].ToString();

            switch (logicalType)
            {
                case LogicalTypeSchema.LogicalTypeEnum.Date:
                case LogicalTypeSchema.LogicalTypeEnum.TimestampMicroseconds:
                case LogicalTypeSchema.LogicalTypeEnum.TimestampMilliseconds:
                    return "DateTime";
                case LogicalTypeSchema.LogicalTypeEnum.Decimal:
                    return "decimal";
                case LogicalTypeSchema.LogicalTypeEnum.Duration:
                case LogicalTypeSchema.LogicalTypeEnum.TimeMicrosecond:
                case LogicalTypeSchema.LogicalTypeEnum.TimeMilliseconds:
                    return "TimeSpan";
                case LogicalTypeSchema.LogicalTypeEnum.Uuid:
                    return "Guid";
                default:
                    throw new InvalidAvroObjectException($"Unidentified logicalType {logicalType}");
            }
        }
    }
}
