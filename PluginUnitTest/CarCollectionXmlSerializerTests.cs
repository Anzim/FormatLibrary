using System.IO;
using System.Text;
using AsposeLibrary.Models;
using JsonSerializerPlugin.Serializers;
using NUnit.Framework;

namespace PluginUnitTest
{
    public class CarCollectionJsonSerializerTests
    {
        private const string JsonData = @"{""CarRecords"":[{""Date"":""02.07.2019"",""BrandName"":""Brand name (брэнд) 1"",""Price"":99999},{""Date"":""04.07.2019"",""BrandName"":""Brand name (брэнд) 3"",""Price"":4294967295},{""Date"":""10.10.2008"",""BrandName"":""Alpha Romeo Brera"",""Price"":37000}]}";

        private CarCollectionJsonSerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _serializer = new CarCollectionJsonSerializer();
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenWritingDefaultCarCollection_ThenValidDefaultJsonDataStored()
        {
            var cars = new CarCollection
            {
                CarRecords =
                {
                    new CarRecord
                    {
                        Date = "10.10.2008",
                        BrandName = "Alpha Romeo Brera",
                        Price = 37000
                    }
                }
            };

            var data = new byte[84];
            var ms = new MemoryStream(data, true);
            _serializer.Write(ms, cars);

            var actualJsonData = Encoding.UTF8.GetString(data);
            var expectedJsonData = @"{""CarRecords"":[{""Date"":""10.10.2008"",""BrandName"":""Alpha Romeo Brera"",""Price"":37000}]}";
            Assert.AreEqual(expectedJsonData, actualJsonData);
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenReadingDefaultJsonData_ThenValidDefaultCarCollectionRetrieved()
        {
            var data = Encoding.ASCII.GetBytes(JsonData);
            var ms = new MemoryStream(data, false);
            var cars = _serializer.Read(ms);
            Assert.AreEqual(cars?.CarRecords?.Count, 3);
            var car = cars.CarRecords[2];
            Assert.AreEqual(car.BrandName, "Alpha Romeo Brera");
            Assert.AreEqual(car.Price, 37000);
            Assert.AreEqual(car.Date, "10.10.2008");
        }
    }
}