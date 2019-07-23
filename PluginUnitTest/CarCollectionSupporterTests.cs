using System.IO;
using AsposeLibrary.Enums;
using AsposeLibrary.Supporters;
using AsposeLibraryUnitTest.Helpers;
using JsonSerializerPlugin.Serializers;
using NUnit.Framework;

namespace PluginUnitTest
{
    public class CarCollectionSupporterPluginTests
    {
        private const string BaseFilePath = @"..\..\..\..\Data\AsposeCars";
        private const string XmlExt = ".xml";
        private const string BinaryExt = ".dat";
        private const string JsonExt = ".json";

        private static CarFileFormat _jsonFileFormat;
        private CarCollectionSupporter _supporter;
        
        [OneTimeSetUp]
        public static void BeforeTests()
        {
            _jsonFileFormat = CarCollectionSupporter.AddFileFormat<CarCollectionJsonSerializer>();
        }

        [SetUp]
        public void BeforeTest()
        {
            _supporter = new CarCollectionSupporter();
        }

        [TearDown]
        public void AfterTest()
        {
        }

        // this is a deep test
        [Test]
        public void WhenConvertingAsposeCarsBinaryFileToJson_ThenValidDataStored()
        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                _supporter.Convert(BaseFilePath + BinaryExt, tempFilePath, _jsonFileFormat, true);

                var expectedHash = CarTestHelper.ComputeHashString(tempFilePath);
                Assert.AreEqual(expectedHash, "f851e486c27e363c129009705d29bb96");
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }

        // this is a deep test
        [Test]
        public void WhenConvertingAsposeCarsJsonFileToXml_ThenValidDataStored()
        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                _supporter.Convert(BaseFilePath + JsonExt, tempFilePath, CarFileFormat.Xml, true);

                var expectedHash = CarTestHelper.ComputeHashString(tempFilePath);
                Assert.AreEqual(expectedHash, "3f1e0b3cddeb1b365b6bdf31d3200791");
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }


        [Test]
        public void WhenGetFileFormatForAsposeCarsJsonFile_ThenValidFormatReturned()
        {
            var format = _supporter.GetFileFormat(BaseFilePath + JsonExt);

            Assert.AreEqual(format, _jsonFileFormat);
        }

    }
}