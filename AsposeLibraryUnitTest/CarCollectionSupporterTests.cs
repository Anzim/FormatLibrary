using System.IO;
using AsposeLibrary.Enums;
using AsposeLibrary.Supporters;
using AsposeLibraryUnitTest.Helpers;

using NUnit.Framework;

namespace AsposeLibraryUnitTest
{
    public class CarCollectionSupporterTests
    {
        private const string BaseFilePath = @"..\..\..\..\Data\AsposeCars";
        private const string XmlExt = ".xml";
        private const string BinaryExt = ".dat";

        private CarCollectionSupporter _supporter;
        
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
        public void WhenConvertingAsposeCarsXmlFileToBinary_ThenValidDataStored()
        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                _supporter.Convert(BaseFilePath + XmlExt, tempFilePath, CarFileFormat.Binary, true);

                var expectedHash = CarTestHelper.ComputeHashString(tempFilePath);
                Assert.AreEqual(expectedHash, "1d56edb154f10056969880c90c07c493");
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }

        // this is a deep test
        [Test]
        public void WhenConvertingAsposeCarsBinaryFileToXml_ThenValidDataStored()
        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                _supporter.Convert(BaseFilePath + BinaryExt, tempFilePath, CarFileFormat.Xml, true);

                var expectedHash = CarTestHelper.ComputeHashString(tempFilePath);
                Assert.AreEqual(expectedHash, "68a2b9a39c661eec489995b607a50331");
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }

       // this is a deep test
        [Test]
        public void WhenSavingAsposeCarsToXml_ThenValidDataStored()
        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                var cars = CarTestHelper.CreateDefaultCarCollection();
                _supporter.Save(tempFilePath, CarFileFormat.Xml, true, cars);

                var expectedHash = CarTestHelper.ComputeHashString(tempFilePath);
                Assert.AreEqual(expectedHash, "b808279ce0ea89b2fb29647650649812");
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }

        [Test]
        public void WhenGetFileFormatForAsposeCarsBinaryFile_ThenValidFormatReturned()
        {
            var format = _supporter.GetFileFormat(BaseFilePath + BinaryExt);

            Assert.AreEqual(format, CarFileFormat.Binary);
        }

        [Test]
        public void WhenGetFileFormatForAsposeCarsXmlFile_ThenValidFormatReturned()
        {
            var format = _supporter.GetFileFormat(BaseFilePath + XmlExt);

            Assert.AreEqual(format, CarFileFormat.Xml);
        }
    }
}