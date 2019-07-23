using System;
using System.IO;
using AsposeLibrary.Exceptions;
using AsposeLibrary.Interfaces;
using AsposeLibrary.Models;

namespace AsposeLibrary.Serializers
{
    public class CarCollectionBinarySerializer : ICarCollectionSerializer
    {
        private const ushort BinaryHeader = 0x2526;

        public bool IsThisFileFormat(Stream stream)
        {
            var buffer = new byte[2];
            var position = stream.Position;
            stream.Read(buffer, 0, 2);
            if (stream.CanSeek) stream.Seek(position, SeekOrigin.Begin);

            return BinaryHeader == buffer[0] + buffer[1] * 0x100;
        }

        public void Write(Stream destinationStream, CarCollection cars)
        {
            using (var writer = new BinaryWriter(destinationStream))
            {
                InternalWrite(cars, writer);
            }
        }
       
        public CarCollection Read(Stream sourceStream)
        {
            var cars = new CarCollection();

            using (var reader = new BinaryReader(sourceStream))
            {
                InternalRead(reader, cars);
            }

            return cars;
        }
        
        private static void InternalWrite(CarCollection cars, BinaryWriter writer)
        {
            var records = cars.CarRecords.Count;
            writer.Write(BinaryHeader);
            writer.Write(records);
            foreach (var record in cars.CarRecords)
            {
                var date = record.LocalDate;

                // use this if DDMMYYYY means int values
                //writer.Write((ushort)date.Day);
                //writer.Write((ushort)date.Month);
                //writer.Write((uint)date.Year);

                // use this if DDMMYYYY means char values
                writer.Write(date.ToString("ddMMyyyy").ToCharArray());

                var brandName = record.BrandName;
                writer.Write((ushort) brandName.Length);
                writer.Write(brandName.ToCharArray());
                writer.Write(record.Price);
            }
        }

        private static void InternalRead(BinaryReader reader, CarCollection cars)
        {
            var header = reader.ReadUInt16();
            if (header != BinaryHeader) throw new CarCollectionFormatException("Mismatch data format: signature is wrong");

            var records = reader.ReadUInt32();
            for (var i = 0; i < records; i++)
            {
                // use this if DDMMYYYY means int values
                //var day = reader.ReadInt16();
                //var month = reader.ReadInt16();
                //var year = reader.ReadInt32();

                // use this if DDMMYYYY means char values
                var day = reader.ReadChars(2);
                var month = reader.ReadChars(2);
                var year = reader.ReadChars(4);
                var brandLength = reader.ReadUInt16();
                var brand = reader.ReadChars(brandLength);
                var price = reader.ReadUInt32();
                var yearValue = int.Parse(new string(year));
                var monthValue = int.Parse(new string(month));
                var dayValue = int.Parse(new string(day));
                var dateStr = new DateTime(yearValue, monthValue, dayValue).ToString(CarRecord.DateFormat);
                var car = new CarRecord
                {
                    Date = dateStr,
                    BrandName = new string(brand),
                    Price = price
                };
                cars.CarRecords.Add(car);
            }
        }
    }
}
