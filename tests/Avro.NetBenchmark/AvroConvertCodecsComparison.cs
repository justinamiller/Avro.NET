using AutoFixture;
using AvroNET;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNETBenchmark
{
    [MemoryDiagnoser]
    [Config(typeof(Config))]
    public class AvroConvertCodecsComparison
    {
        private class Config : ManualConfig
        {
            public Config() => AddColumn(new FileSizeColumn());
        }

        private const int N = 100;
        private readonly Pet[] data;

        public AvroConvertCodecsComparison()
        {
            Fixture fixture = new Fixture();
            data = fixture.CreateMany<Pet>(N).ToArray();
        }

        [Benchmark]
        public void Avro_Default()
        {
            var serialized = AvroConvert.Serialize(data);
            AvroConvert.Deserialize<List<Pet>>(serialized);

            var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}disk - size.{nameof(Avro_Default).ToLower()}.txt";
            File.WriteAllText(path, ConstructSizeLog(serialized.Length));
        }

        [Benchmark]
        public void Avro_Gzip()
        {
            var serialized = AvroConvert.Serialize(data, CodecType.GZip);
            AvroConvert.Deserialize<List<Pet>>(serialized);

            var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}disk-size.{nameof(Avro_Gzip).ToLower()}.txt";
            File.WriteAllText(path, ConstructSizeLog(serialized.Length));
        }

        [Benchmark]
        public void Avro_Deflate()
        {
            var serialized = AvroConvert.Serialize(data, CodecType.Deflate);
            AvroConvert.Deserialize<List<Pet>>(serialized);

            var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}disk-size.{nameof(Avro_Deflate).ToLower()}.txt";
            File.WriteAllText(path, ConstructSizeLog(serialized.Length));
        }

        private string ConstructSizeLog(int size)
        {
            return $"{size / 1024} kB";
        }
    }
}
