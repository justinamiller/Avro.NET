using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.ComponentModel
{
    /// <summary>
    /// This attribute determines the size of the Avro fixed byte array.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    internal sealed class AvroFixedAttribute : Attribute
    {
        private readonly int size;
        private readonly string name;
        private readonly string @namespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroFixedAttribute"/> class. 
        /// </summary>
        /// <param name="size">
        /// The size of the byte array.
        /// </param>
        /// <param name="name">
        /// The name of the fixed schema.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the size is not larger than zero.
        /// </exception>
        internal AvroFixedAttribute(int size, string name)
            : this(size, name, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroFixedAttribute"/> class. 
        /// </summary>
        /// <param name="size">
        /// The size of the byte array.
        /// </param>
        /// <param name="name">
        /// The name of the fixed schema.
        /// </param>
        /// <param name="namespace">
        /// The name space.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the size is not larger than zero.
        /// </exception>
        internal AvroFixedAttribute(int size, string name, string @namespace)
        {
            if (size <= 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Size of fixed must be larger than zero"));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Name is not valid"));
            }
            this.size = size;
            this.name = name;
            this.@namespace = @namespace;
        }

        /// <summary>
        /// Gets the size of the fixed bytes.
        /// </summary>
        internal int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Gets the name of the fixed.
        /// </summary>
        /// <value>
        /// The name of the fixed.
        /// </value>
        internal string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the fixed name space.
        /// </summary>
        /// <value>
        /// The fixed name space.
        /// </value>
        internal string Namespace
        {
            get { return @namespace; }
        }
    }
}
