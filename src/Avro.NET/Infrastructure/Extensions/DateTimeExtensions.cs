using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Extensions
{
    internal static class DateTimeExtensions
    {
        internal readonly static DateTime UnixEpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal readonly static DateOnly UnixEpochDate = new DateOnly(1970, 1, 1);
        internal readonly static TimeOnly UnixEpochTime = new TimeOnly(0, 0, 0);
    }
}
