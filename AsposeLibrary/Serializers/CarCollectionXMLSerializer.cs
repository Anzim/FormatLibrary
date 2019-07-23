using System.IO;
using System.Xml.Serialization;
using AsposeLibrary.Interfaces;
using AsposeLibrary.Models;

namespace AsposeLibrary.Serializers
{
    public class CarCollectionXMLSerializer : ICarCollectionSerializer
    {
        private readonly XmlSerializer _serializer;

        public CarCollectionXMLSerializer()
        {
            _serializer = new XmlSerializer(typeof(CarCollection));
        }

        public void Write(Stream destinationStream, CarCollection cars)
        {
            using (var writer = new StreamWriter(destinationStream, System.Text.Encoding.UTF8))
            {
                _serializer.Serialize(writer, cars);
            }
        }

        public CarCollection Read(Stream sourceStream)
        {
            using(var strReader = new StreamReader(sourceStream, System.Text.Encoding.UTF8))
            {
                return (CarCollection)_serializer.Deserialize(strReader);
            }

        }
    }
}
