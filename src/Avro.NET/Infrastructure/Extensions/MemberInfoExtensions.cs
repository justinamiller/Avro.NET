﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvroNET.Infrastructure.Extensions
{
    internal static class MemberInfoExtensions
    {
        private static readonly byte[] EmptyFlags = { 0 };

        internal static bool IsNullableReferenceType(this MemberInfo member)
        {
            var nullableFlags = GetNullableFlags(member);

            return nullableFlags.Length > 0 && nullableFlags[0] == 2;
        }

        private static byte[] GetNullableFlags(MemberInfo member)
        {
            var nullableAttribute = member.GetCustomAttributes(true)
                .OfType<Attribute>()
                .FirstOrDefault(a => a.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");

            if (nullableAttribute != null)
            {
                return (byte[])nullableAttribute.GetType()
                    .GetRuntimeField("NullableFlags")?
                    .GetValue(nullableAttribute) ?? EmptyFlags;
            }

            var nullableContextAttribute = member.DeclaringType?
                .GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType().FullName == "System.Runtime.CompilerServices.NullableContextAttribute");

            if (nullableContextAttribute != null)
            {
                var value = nullableContextAttribute.GetType()
                    .GetRuntimeField("Flag")?
                    .GetValue(nullableContextAttribute) ?? 0;

                return new[] { (byte)value };
            }

            return EmptyFlags;
        }
    }
}
