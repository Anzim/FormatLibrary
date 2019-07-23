using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AsposeLibrary.Models;

namespace AsposeLibraryUnitTest.Helpers
{
    public class CarTestHelper
    {
        internal const string DefaultBinaryData = "262503000000303230373230313914004272616e64206e616d652028d0b1d180d18dd0bdd0b4292031a9aaaaaa303330373230313914004272616e64206e616d652028d0b1d180d18dd0bdd0b429203254555555303430373230313914004272616e64206e616d652028d0b1d180d18dd0bdd0b4292033ffffffff";
        internal const string DefaultBrandName = "Brand name (брэнд) ";
        internal const int DefaultRecords = 3;

        public static CarCollection CreateDefaultCarCollection()
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
        
        public static string ByteArrayToHexString(byte[] data)
        {
            return string.Concat(data.Select(b => b.ToString("x2")));
        }
                
        public static byte[] HexStringToByteArray(string s)
        {
            return Enumerable.Range(0, s.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                .ToArray();
        }

        public static bool IsCarCollectionsEqual(CarCollection c1, CarCollection c2)
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

        public static byte[] ComputeHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public static string ComputeHashString(string filename)
        {
            return ByteArrayToHexString(ComputeHash(filename));
        }
    }
}
