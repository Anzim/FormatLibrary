using System.IO;
using AsposeLibrary.Models;
using AsposeLibrary.Serializers;
using AsposeLibraryUnitTest.Helpers;
using NUnit.Framework;

namespace AsposeLibraryUnitTest
{
    public class CarCollectionBinarySerializerTests
    {
        private CarCollectionBinarySerializer _serializer;
        private CarCollection _cars;

        [SetUp]
        public void Setup()
        {
            _serializer = new CarCollectionBinarySerializer();
            _cars = CarTestHelper.CreateDefaultCarCollection();
        }

        

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenWritingDefaultCarCollection_ThenValidDefaultBinaryDataStored()
        {
            var data = new byte[123];
            var ms = new MemoryStream(data, true);
            _serializer.Write(ms, _cars);

            var expectedData = CarTestHelper.ByteArrayToHexString(data);;
            Assert.AreEqual(expectedData, CarTestHelper.DefaultBinaryData);
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenReadingDefaultBinaryData_ThenValidDefaultCarCollectionRetrieved()
        {
            var data = CarTestHelper.HexStringToByteArray(CarTestHelper.DefaultBinaryData);
            var ms = new MemoryStream(data, false);
            var cars = _serializer.Read(ms);
            var isEqual = CarTestHelper.IsCarCollectionsEqual(cars, _cars);
            Assert.IsTrue(isEqual);
        }
    }
}