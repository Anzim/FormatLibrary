using System.Collections.Generic;
using System.Xml.Serialization;

namespace AsposeLibrary.Models
{
    [XmlRoot("Document")]
    public class CarCollection
    {
        public CarCollection()
        {
            CarRecords = new List<CarRecord>();
        }

        [XmlElement(typeof(CarRecord), ElementName="Car")]
        public List<CarRecord> CarRecords { get; private set; }
    }
}
