using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.AvroObjectServices.FileHeader
{
    internal class DataFileConstants
    {
        internal const string SyncMetadataKey = "avro.sync";
        internal const string CodecMetadataKey = "avro.codec";
        internal const string SchemaMetadataKey = "avro.schema";
        internal const string MetaDataReserved = "avro";

        internal static byte[] AvroHeader = { (byte)'O',
                                            (byte)'b',
                                            (byte)'j',
                                            (byte) 1 };

        internal const int SyncSize = 16;
        internal const int DefaultSyncInterval = 4000 * SyncSize;
    }
}
