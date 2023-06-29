﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.BuildSchema
{
    /// <summary>
    /// Represents a name of the schema. For details, please see <a href="http://avro.apache.org/docs/current/spec.html#Names">the specification</a>.
    /// </summary>
    internal sealed class SchemaName
    {
        private static readonly Regex NamePattern = new Regex("^[A-Za-z_][A-Za-z0-9_]*$");
        private static readonly Regex NamespacePattern = new Regex("^([A-Za-z_][A-Za-z0-9_]*)?(?:\\.[A-Za-z_][A-Za-z0-9_]*)*$");

        private readonly string name;
        private readonly string @namespace;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SchemaName" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        internal SchemaName(string name) : this(name, string.Empty)
        {
        }

        internal SchemaName(string name, bool force)
        {
            if (force)
            {
                this.name = Regex.Replace(name, @"[^0-9a-zA-Z]+", "_"); //replace special characters
                return;
            }

            new SchemaName(name, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaName" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="namespace">The namespace.</param>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="name"/> is empty or null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Thrown when any argument is invalid.</exception>
        internal SchemaName(string name, string @namespace)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "Name is not allowed to be null or empty."), "name");
            }

            this.@namespace = @namespace ?? string.Empty;

            int lastDot = name.LastIndexOf('.');
            if (lastDot == name.Length - 1)
            {
                throw new SerializationException(
                    string.Format(CultureInfo.InvariantCulture, "Invalid name specified [{0}].", name));
            }

            if (lastDot != -1)
            {
                this.name = name.Substring(lastDot + 1, name.Length - lastDot - 1);
                this.@namespace = name.Substring(0, lastDot);
            }
            else
            {
                this.name = name;
            }

            this.CheckNameAndNamespace();
        }

        internal string Name
        {
            get { return this.name; }
        }

        internal string Namespace
        {
            get { return this.@namespace; }
        }

        internal string FullName
        {
            get { return string.IsNullOrEmpty(this.@namespace) ? this.name : this.@namespace + "." + this.name; }
        }


        private void CheckNameAndNamespace()
        {
            if (!NamePattern.IsMatch(this.name))
            {
                throw new SerializationException(
                    string.Format(CultureInfo.InvariantCulture, "Name [{0}] contains invalid characters.", this.name));
            }

            if (!NamespacePattern.IsMatch(this.@namespace))
            {
                throw new SerializationException(
                    string.Format(CultureInfo.InvariantCulture, "Namespace [{0}] contains invalid characters.", this.@namespace));
            }
        }
    }
}
