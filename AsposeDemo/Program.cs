using System;
using System.IO;
using AsposeLibrary.Serializers;
using AsposeLibrary.Interfaces;
using AsposeLibrary.Models;

namespace AsposeDemo
{
    class Program
    {
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
                CreateCarsFile(demoCars, new CarCollectionBinarySerializer(), BinaryDemoFileName);
                
                Console.WriteLine("Creating default Car Collection XML file " + XmlDemoFileName);
                CreateCarsFile(demoCars, new CarCollectionXMLSerializer(), XmlDemoFileName);

                Console.WriteLine("Reading Car Collection file " + XMLFileName);
                var cars = ReadCarCollection(new CarCollectionXMLSerializer(), XMLFileName);

                Console.WriteLine("Writing (converting) Car Collection to binary file " + BinaryFileName);
                CreateCarsFile(cars, new CarCollectionBinarySerializer(), BinaryFileName);
                
                Console.WriteLine("Adding a new record to the default Car Collection file");
                demoCars.CarRecords.Add(cars.CarRecords[0]);

                Console.WriteLine("Updating the first record in the default Car Collection file (price = 99999)");
                demoCars.CarRecords[0].Price = 99999;

                Console.WriteLine("Deleting the second record in the default Car Collection file");
                demoCars.CarRecords.RemoveAt(1);

                Console.WriteLine("Creating modified Car Collection binary file " + ModifiedBinaryFileName);
                CreateCarsFile(demoCars, new CarCollectionBinarySerializer(), ModifiedBinaryFileName);
                
                Console.WriteLine("Creating modified Car Collection XML file " + ModifiedXMLFileName);
                CreateCarsFile(demoCars, new CarCollectionJsonSerializer(), ModifiedXMLFileName);

                Console.WriteLine("Creating modified Car Collection Json file " + ModifiedJsonFileName);
                // Json serializer is custom user format
                CreateCarsFile(demoCars, new CarCollectionJsonSerializer(), ModifiedJsonFileName);
            }
            catch (IOException e)
            {
                Console.WriteLine("Cannot perform operation because of this error: " + e.Message);
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void CreateCarsFile(CarCollection cars, ICarCollectionSerializer carsSerializer, string fileName) {
            var file = File.Open(fileName, FileMode.Create);
            carsSerializer.Write(file, cars);
            file.Close();
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

        public static CarCollection ReadCarCollection(ICarCollectionSerializer carCollectionSerializer, string fileName)
        {
            var file = File.Open(fileName, FileMode.Open);
            return carCollectionSerializer.Read(file);
        }
    }
}

