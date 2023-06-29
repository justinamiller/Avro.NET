using AvroNET.AvroObjectServices.Schemas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader
{
    internal class Header
    {
        private Dictionary<string, byte[]> MetaData { get; }

        internal byte[] SyncData { get; set; }

        internal TypeSchema Schema { get; set; }

        internal Header()
        {
            MetaData = new Dictionary<string, byte[]>();
            SyncData = new byte[16];
        }

        internal void AddMetadata(string key, byte[] value)
        {
            MetaData.Add(key, value);
        }

        internal void AddMetadata(string key, string value)
        {
            MetaData.Add(key, System.Text.Encoding.UTF8.GetBytes(value));
        }

        internal string GetMetadata(string key)
        {
            MetaData.TryGetValue(key, out var value);
            if (value == null)
            {
                return null;
            }
            var valueAsString = System.Text.Encoding.UTF8.GetString(value);
            return valueAsString;
        }

        internal byte[] GetRawMetadata(string key)
        {
            MetaData.TryGetValue(key, out var value);
            return value;
        }

        internal Dictionary<string, byte[]> GetRawMetadata()
        {
            return MetaData;
        }

        internal int GetMetadataSize()
        {
            return MetaData.Count;
        }
    }
}
