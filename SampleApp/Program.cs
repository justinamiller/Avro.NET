// See https://aka.ms/new-console-template for more information
using AvroNET;

Console.WriteLine("Hello, World!");
var obj = new MyObject
{
    Name = "john",
    Age = 13,
    Address = "somewhere"
};

  
System.Threading.Thread.Sleep(5000);
for(var i = 0; i < 100; i++)
{
    var result = AvroNET.AvroConvert.Serialize(obj);
    string resultModel = AvroConvert.GenerateModel(result);
    string schemaInJsonFormat = AvroConvert.GetSchema(result);
    var deserializedObject = AvroConvert.Deserialize<MyObject>(result);
    var resultJson = AvroConvert.Avro2Json(result);
    result.ToArray();
}



public class MyObject
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}

