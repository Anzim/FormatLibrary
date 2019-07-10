using System;
using System.Globalization;

namespace AsposeLibrary.Models
{
    public class CarRecord
    {
        public const string DateFormat = "dd.MM.yyyy";

        internal DateTime LocalDate;

        public string Date
        {
            get => LocalDate.ToString(DateFormat);
            set => LocalDate = DateTime.ParseExact(value, DateFormat, provider: CultureInfo.CurrentCulture.DateTimeFormat);
        }

        public string BrandName { get; set; }

        public uint Price { get; set; }
    }
}
