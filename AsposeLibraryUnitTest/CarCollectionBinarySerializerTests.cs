using System;
using System.IO;
using System.Linq;
using AsposeLibrary.Models;
using AsposeLibrary.Serializers;
using NUnit.Framework;

namespace AsposeLibraryUnitTest
{
    public class CarCollectionBinarySerializerTests
    {
        private const string DefaultBinaryData = "262503000000303230373230313914004272616e64206e616d652028d0b1d180d18dd0bdd0b4292031a9aaaaaa303330373230313914004272616e64206e616d652028d0b1d180d18dd0bdd0b429203254555555303430373230313914004272616e64206e616d652028d0b1d180d18dd0bdd0b4292033ffffffff";
        const string DefaultBrandName = "Brand name (брэнд) ";
        const int DefaultRecords = 3;
        
        private CarCollectionBinarySerializer _serializer;
        private CarCollection _cars;

        [SetUp]
        public void Setup()
        {
            _serializer = new CarCollectionBinarySerializer();
            _cars = CreateDefaultCarCollection();
        }

        private static CarCollection CreateDefaultCarCollection()
        {
            var cars = new CarCollection();
            var date = new DateTime(2019, 07, 01);
            var price = uint.MaxValue - 1;
            for (var i = 1; i <= DefaultRecords; i++)
            {
                var car = new CarRecord
                {
                    Date = date.AddDays(i).ToString(CarRecord.DateFormat),
                    BrandName = DefaultBrandName + i.ToString(),
                    Price = price - (uint)(i * uint.MaxValue / DefaultRecords)
                };
                cars.CarRecords.Add(car);
            }

            return cars;
        }
        
        private static string ByteArrayToHexString(byte[] data)
        {
            return string.Concat(data.Select(b => b.ToString("x2")));
        }
                
        private static byte[] HexStringToByteArray(string s)
        {
            return Enumerable.Range(0, s.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                .ToArray();
        }

        private static bool IsCarCollectionsEqual(CarCollection c1, CarCollection c2)
        {
            if (c1 == c2) return true;
            if (c1.CarRecords == c2.CarRecords) return true;
            if (c1.CarRecords.Count != c2.CarRecords.Count) return false;
            for (int i = 0; i < c1.CarRecords.Count; i++)
            {
                var car1 = c1.CarRecords[i];
                var car2 = c2.CarRecords[i];
                if (car1.Price != car2.Price || car1.BrandName != car2.BrandName || car1.Date != car2.Date) return false;
            }

            return true;
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenWritingDefaultCarCollection_ThenValidDefaultBinaryDataStored()
        {
            var data = new byte[123];
            var ms = new MemoryStream(data, true);
            _serializer.Write(ms, _cars);

            var expectedData = ByteArrayToHexString(data);;
            Assert.AreEqual(expectedData, DefaultBinaryData);
        }

        // this is a deep test, some consider it integration test
        [Test]
        public void WhenReadingDefaultBinaryData_ThenValidDefaultCarCollectionRetrieved()
        {
            var data = HexStringToByteArray(DefaultBinaryData);
            var ms = new MemoryStream(data, false);
            var cars = _serializer.Read(ms);
            var isEqual = IsCarCollectionsEqual(cars, _cars);
            Assert.IsTrue(isEqual);
        }
    }
}