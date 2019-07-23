using System.IO;
using AsposeLibrary.Interfaces;
using AsposeLibrary.Models;
using Newtonsoft.Json;

namespace JsonSerializerPlugin.Serializers
{
    public class CarCollectionJsonSerializer : ICarCollectionSerializer
    {
        private readonly JsonSerializer _serializer;

        public CarCollectionJsonSerializer()
        {
            _serializer = new JsonSerializer();
        }

        public bool IsThisFileFormat(Stream stream)
        {
            var position = stream.Position;
            var buffer = stream.ReadByte();
            if (stream.CanSeek) stream.Seek(position, SeekOrigin.Begin);

            return buffer == '{';
        }

        public void Write(Stream destinationStream, CarCollection cars)
        {
            using (var writer = new StreamWriter(destinationStream))
            {
                _serializer.Serialize(writer, cars);
            }
        }

        public CarCollection Read(Stream sourceStream)
        {
            using (var sr = new StreamReader(sourceStream))
            using (var reader = new JsonTextReader(sr))
            {
                return _serializer.Deserialize<CarCollection>(reader);
            }
        }
    }
}
