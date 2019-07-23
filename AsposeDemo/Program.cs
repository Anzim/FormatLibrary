using System;
using System.IO;
using AsposeLibrary.Enums;
using AsposeLibrary.Interfaces;
using AsposeLibrary.Models;
using AsposeLibrary.Supporters;
using JsonSerializerPlugin.Serializers;

namespace AsposeDemo
{
    class Program
    {
        private static CarCollectionSupporter _carSupporter = new CarCollectionSupporter();
        const string BinExt = ".dat";
        const string XmlExt = ".xml";
        const string JsonExt = ".json";
        const string DemoFileName = @"..\..\..\Data\AsposeDemo";
        const string FileName = @"..\..\..\Data\AsposeCars";
        const string BinaryDemoFileName = DemoFileName + BinExt;
        const string XmlDemoFileName = DemoFileName + XmlExt;
        const string XMLFileName = FileName + XmlExt;
        const string BinaryFileName = FileName + BinExt;
        const string ModifiedXMLFileName = FileName + "Modified" + XmlExt;
        const string ModifiedBinaryFileName = FileName + "Modified" + BinExt;
        const string ModifiedJsonFileName = FileName + "Modified" + JsonExt;

        static void Main(string[] args)
        {
            try
            {
                var demoCars = CreateDefaultCarCollection();
                Console.WriteLine("Creating default Car Collection binary file " + BinaryDemoFileName);
                _carSupporter.Save(BinaryDemoFileName, CarFileFormat.Binary, true, demoCars);
                
                Console.WriteLine("Creating default Car Collection XML file " + XmlDemoFileName);
                _carSupporter.Save(XmlDemoFileName, CarFileFormat.Xml, true, demoCars);

                Console.WriteLine("Converting XML Car Collection file to a binary file " + BinaryFileName);
                _carSupporter.Convert(XMLFileName, BinaryFileName, CarFileFormat.Binary);

                Console.WriteLine("Reading Car Collection file " + XMLFileName);
                var cars = _carSupporter.Load(XMLFileName);
                
                Console.WriteLine("Adding a new record to the default Car Collection file");
                demoCars.CarRecords.Add(cars.CarRecords[0]);

                Console.WriteLine("Creating modified Car Collection binary file " + ModifiedBinaryFileName);
                _carSupporter.Save(ModifiedBinaryFileName, CarFileFormat.Binary, true, demoCars);

                Console.WriteLine("Updating the first record in the default Car Collection file (price = 99999)");
                demoCars.CarRecords[0].Price = 99999;

                Console.WriteLine("Creating modified Car Collection XML file " + ModifiedXMLFileName);
                _carSupporter.Save(ModifiedXMLFileName, CarFileFormat.Xml, true, demoCars);

                Console.WriteLine("Deleting the second record in the default Car Collection file");
                demoCars.CarRecords.RemoveAt(1);

                var jsonFormat = CarCollectionSupporter.AddFileFormat<CarCollectionJsonSerializer>();

                Console.WriteLine("Creating modified Car Collection JSON file " + ModifiedJsonFileName);
                _carSupporter.Save(ModifiedJsonFileName, jsonFormat, true, demoCars);
                
            }
            catch (IOException e)
            {
                Console.WriteLine("Cannot perform operation because of this error: " + e.Message);
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static CarCollection CreateDefaultCarCollection()
        {
            const string brandName = "Brand name (брэнд) ";
            const int records = 3;
            var cars = new CarCollection();
            var date = new DateTime(2019, 07, 01);
            var price = uint.MaxValue - 1;
            for (var i = 1; i <= records; i++)
            {
                var car = new CarRecord
                {
                    Date = date.AddDays(i).ToString(CarRecord.DateFormat),
                    BrandName = brandName + i.ToString(),
                    Price = price - (uint)(i * uint.MaxValue / records)
                };
                cars.CarRecords.Add(car);
            }

            return cars;
        }

        //public static CarCollection ReadCarCollection(ICarCollectionSerializer carCollectionSerializer, string fileName)
        //{
        //    var file = File.Open(fileName, FileMode.Open);
        //    return carCollectionSerializer.Read(file);
        //}
    }
}

