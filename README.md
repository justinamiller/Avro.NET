# Avro.NET
Fast Avro de/serializer for .NET

[![NuGet Badge](https://buildstats.info/nuget/Avro.NET)](https://www.nuget.org/packages/Avro.NET/)

## Benefits
Introducing Avro to the projects brings three main benefits:
 * Reduction of data size and storage cost
 * Decrease of the communication time and the network traffic between microservices
 * Increased security - the data is not visible in plain text format

## Features
|                                                               | AvroConvert                                | Apache.Avro | Newtonsoft.Json |
|---------------------------------------------------------------|:------------------------------------------:|:-----------:|:---------------:|
| Rapid serialization                                            |                      ✔                     |      ✔      |        ✔        |
| Easy to use                                                   |                      ✔                     |      X      |        ✔        |
| Built-in compression                                          |                      ✔                     |      ✔      |        X        |
| Low memory allocation                                         |                      ✔                     |      ✔      |        ✔        |
| Support for C# data structures: Dictionary, List, DateTime... |                      ✔                     |      X      |        ✔        |
| Support for compression codecs                                | Deflate &  GZip |   Deflate   |        X        |
| Readable schema of data structure                                      |                      ✔                     |      ✔      |        ✔        |
| Data encryption                                       |                      ✔                     |      ✔      |        X        |

## Benchmark

Results of BenchmarkDotNet:

|Converter     | Request Time [ms] | Allocated Memory [MB] | Compressed Size [kB] |
|------------- |------------------:|----------------------:|---------------------:|
| Json         |       642.1       |          151.45        |         6044         |
| Avro         |       254.3       |          71.27        |         2623         |
| Json_Gzip    |       194.7       |          85.45        |          514         |
| Avro_Gzip    |       131.5       |          64.15        |          104         |

## Docs
**Avro format combines readability of JSON and data compression of binary serialization.**
[Apache Wiki](https://cwiki.apache.org/confluence/display/AVRO/Index)
[Apache Avro format documentation](http://avro.apache.org/)

## Code samples

* Serialization
```csharp
 byte[] avroObject = AvroConvert.Serialize(object yourObject);
```
<br/>

* Deserialization
```csharp
CustomClass deserializedObject = AvroConvert.Deserialize<CustomClass>(byte[] avroObject);
```
<br/>

* Read schema from Avro object

```csharp
string schemaInJsonFormat = AvroConvert.GetSchema(byte[] avroObject)
```
<br/>

* Deserialization of large collection of Avro objects one by one

```csharp
using (var reader = AvroConvert.OpenDeserializer<CustomClass>(new MemoryStream(avroObject)))
{
    while (reader.HasNext())
    {
        var item = reader.ReadNext();
        // process item
    }
}
```

* Generation of C# models from Avro file or schema

```csharp
  string resultModel = AvroConvert.GenerateModel(avroObject);
```

* Conversion of Avro to JSON directly

```csharp
  var resultJson = AvroConvert.Avro2Json(avroObject);
```
