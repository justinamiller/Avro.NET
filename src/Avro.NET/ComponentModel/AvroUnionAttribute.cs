using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.ComponentModel
{
    /// <summary>
    /// Used to determine type alternatives for field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are not exposed.")]
    internal sealed class AvroUnionAttribute : Attribute
    {
        private readonly Type[] typeAlternatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroUnionAttribute"/> class.
        /// </summary>
        /// <param name="typeAlternatives">
        /// The type alternatives.
        /// </param>
        internal AvroUnionAttribute(params Type[] typeAlternatives)
        {
            this.typeAlternatives = typeAlternatives;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroUnionAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        internal AvroUnionAttribute(Type type) : this(new[] { type })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroUnionAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="type2">The type2.</param>
        internal AvroUnionAttribute(Type type, Type type2) : this(new[] { type, type2 })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroUnionAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="type2">The type2.</param>
        /// <param name="type3">The type3.</param>
        internal AvroUnionAttribute(Type type, Type type2, Type type3)
            : this(new[] { type, type2, type3 })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroUnionAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="type2">The type2.</param>
        /// <param name="type3">The type3.</param>
        /// <param name="type4">The type4.</param>
        internal AvroUnionAttribute(Type type, Type type2, Type type3, Type type4)
            : this(new[] { type, type2, type3, type4 })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroUnionAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="type2">The type2.</param>
        /// <param name="type3">The type3.</param>
        /// <param name="type4">The type4.</param>
        /// <param name="type5">The type5.</param>
        internal AvroUnionAttribute(Type type, Type type2, Type type3, Type type4, Type type5)
            : this(new[] { type, type2, type3, type4, type5 })
        {
        }

        /// <summary>
        /// Gets the type alternatives.
        /// </summary>
        /// <value>
        /// The type alternatives.
        /// </value>
        internal IEnumerable<Type> TypeAlternatives
        {
            get
            {
                return typeAlternatives;
            }
        }
    }
}
