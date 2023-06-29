using AvroNET.AvroObjectServices.Schemas;
using AvroNET.Infrastructure.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Read
{
    internal partial class Resolver
    {
        private readonly Dictionary<int, Dictionary<string, Func<object>>> readStepsDictionary = new Dictionary<int, Dictionary<string, Func<object>>>();
        private readonly Dictionary<int, TypeAccessor> accessorDictionary = new Dictionary<int, TypeAccessor>();

        protected virtual object ResolveRecord(RecordSchema writerSchema, RecordSchema readerSchema, IReader dec, Type type)
        {
            object result = FormatterServices.GetUninitializedObject(type);
            var typeHash = type.GetHashCode();

            TypeAccessor accessor;
            Dictionary<string, Func<object>> readSteps;

            if (!accessorDictionary.ContainsKey(typeHash))
            {
                accessor = TypeAccessor.Create(type, true);
                readSteps = new Dictionary<string, Func<object>>();

                foreach (RecordFieldSchema wf in writerSchema.Fields)
                {
                    if (readerSchema.TryGetField(wf.Name, out var rf))
                    {
                        string name = rf.Aliases.FirstOrDefault() ?? wf.Name;

                        var members = accessor.GetMembers();
                        var memberInfo = members.FirstOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                        if (memberInfo == null)
                        {
                            continue;
                        }

                        Func<object> func = () =>
                        {
                            object value = Resolve(wf.TypeSchema, rf.TypeSchema, dec, memberInfo.Type);
                            return value ?? FormatDefaultValue(wf.DefaultValue, memberInfo);
                        };

                        accessor[result, memberInfo.Name] = func.Invoke();

                        readSteps.Add(memberInfo.Name, func);

                    }
                    else
                        _skipper.Skip(wf.TypeSchema, dec);
                }

                readStepsDictionary.Add(typeHash, readSteps);
                accessorDictionary.Add(typeHash, accessor);
            }
            else
            {
                accessor = accessorDictionary[typeHash];
                readSteps = readStepsDictionary[typeHash];

                foreach (var readStep in readSteps)
                {
                    accessor[result, readStep.Key] = readStep.Value.Invoke();
                }
            }
            return result;
        }

        private static object FormatDefaultValue(object defaultValue, Member memberInfo)
        {
            if (defaultValue == null)
            {
                return defaultValue;
            }

            var t = memberInfo.Type;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            if (defaultValue.GetType() == t)
            {
                return defaultValue;
            }

            if (t.IsEnum)
            {
                return Enum.Parse(t, (string)defaultValue);
            }

            //TODO: Map and Record default values are represented as Dictionary<string,object>
            //https://avro.apache.org/docs/1.4.0/spec.html
            //It might be not supported at the moment

            return Convert.ChangeType(defaultValue, t);
        }
    }
}
