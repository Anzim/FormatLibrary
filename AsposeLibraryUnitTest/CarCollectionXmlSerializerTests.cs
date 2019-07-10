using System;
using System.IO;
using System.Linq;
using System.Text;
using AsposeLibrary.Models;
using AsposeLibrary.Serializers;
using NUnit.Framework;

namespace AsposeLibraryUnitTest
{
    public class CarCollectionXmlSerializerTests
    {
        private const string XmlData = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Car>
    <Date>10.10.2008</Date>
    <BrandName>Alpha Romeo Brera</BrandName>
    <Price>37000</Price>
  </Car>
</Document>";

        private CarCollectionXMLSerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _serializer = new CarCollectionXMLSerializer();
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenWritingDefaultCarCollection_ThenValidDefaultXmlDataStored()
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

            var data = new byte[285];
            var ms = new MemoryStream(data, true);
            _serializer.Write(ms, cars);

            var actualXmlData = Encoding.UTF8.GetString(data);
            // let's skip BOM
            int isEqual = string.CompareOrdinal(actualXmlData, 1, XmlData, 0, XmlData.Length);
            Assert.AreEqual(isEqual, 0);
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenReadingDefaultXmlData_ThenValidDefaultCarCollectionRetrieved()
        {
            var data = Encoding.ASCII.GetBytes(XmlData);
            var ms = new MemoryStream(data, false);
            var cars = _serializer.Read(ms);
            Assert.AreEqual(cars?.CarRecords?.Count, 1);
            var car = cars.CarRecords[0];
            Assert.AreEqual(car.BrandName, "Alpha Romeo Brera");
            Assert.AreEqual(car.Price, 37000);
            Assert.AreEqual(car.Date, "10.10.2008");
        }
    }
}