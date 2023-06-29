using AvroNET.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.Schemas.AvroTypes
{
    internal class AvroFixed
    {
        protected readonly byte[] _value;
        private FixedSchema _schema;

        internal FixedSchema Schema
        {
            get => _schema;

            set
            {
                if (!(value is FixedSchema))
                    throw new AvroException("Schema " + value.Name + " in set is not FixedSchema");

                if ((value as FixedSchema).Size != _value.Length)
                    throw new AvroException("Schema " + value.Name + " Size " + (value as FixedSchema).Size + "is not equal to bytes length " + _value.Length);

                _schema = value;
            }
        }

        internal AvroFixed(FixedSchema schema)
        {
            _value = new byte[schema.Size];
            Schema = schema;
        }

        internal AvroFixed(FixedSchema schema, byte[] value)
        {
            _value = new byte[schema.Size];
            Schema = schema;
            Value = value;
        }

        protected AvroFixed(uint size)
        {
            _value = new byte[size];
        }

        internal byte[] Value
        {
            get => _value;
            set
            {
                if (value.Length == _value.Length)
                {
                    Array.Copy(value, _value, value.Length);
                    return;
                }
                throw new AvroException("Invalid length for fixed: " + value.Length + ", (" + Schema + ")");
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || !(obj is AvroFixed that)) return false;
            if (!that.Schema.Equals(Schema)) return false;
            return !_value.Where((t, i) => _value[i] != that._value[i]).Any();
        }

        public override int GetHashCode()
        {
            return Schema.GetHashCode() + _value.Sum(b => 23 * b);
        }
    }
}
