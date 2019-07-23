using System;
using System.Collections.Generic;
using System.IO;
using AsposeLibrary.Enums;
using AsposeLibrary.Exceptions;
using AsposeLibrary.Interfaces;
using AsposeLibrary.Models;
using AsposeLibrary.Serializers;

namespace AsposeLibrary.Supporters
{
    public class CarCollectionSupporter
    {
        private static readonly Dictionary<CarFileFormat, Lazy<ICarCollectionSerializer>> FileFormats = new Dictionary<CarFileFormat, Lazy<ICarCollectionSerializer>>
        {
            { CarFileFormat.Binary, new Lazy<ICarCollectionSerializer>(() => new CarCollectionBinarySerializer()) },
            { CarFileFormat.Xml,    new Lazy<ICarCollectionSerializer>(() => new CarCollectionXMLSerializer()) }
        };

        /// <summary>
        /// The current loaded CarCollection
        /// </summary>
        public CarCollection CurrentCarCollection { get; protected set; }

        public static CarFileFormat AddFileFormat<T>() where T: ICarCollectionSerializer, new()
        {
            var fileFormat = (CarFileFormat) FileFormats.Count + 1;
            FileFormats.Add(fileFormat, new Lazy<ICarCollectionSerializer>(() => new T()));

            return fileFormat;
        }

        /// <summary>Gets the file format.</summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The determined file format.</returns>
        /// <remarks>
        /// The file format determined does not mean that the specified CarCollection may be loaded. Use one of the CanLoad method overloads to determine whether file may be loaded.
        /// </remarks>
        public CarFileFormat GetFileFormat(string filePath)
        {
            using (var fileStream = File.Open(filePath, FileMode.Open))
            {
                return GetFileFormat(fileStream);
            }
        }

        /// <summary>Gets the file format.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The determined file format.</returns>
        /// <remarks>
        /// The file format determined does not mean that the specified CarCollection may be loaded. Use one of the CanLoad method overloads to determine whether stream may be loaded.
        /// </remarks>
        public CarFileFormat GetFileFormat(Stream stream)
        {
            foreach (var format in FileFormats)
            {
                if (format.Value.Value.IsThisFileFormat(stream)) return format.Key;
            }

            return CarFileFormat.Unknown;
        }

        /// <summary>Loads a new CarCollection from the specified file.</summary>
        /// <param name="filePath">The file path to load CarCollection from.</param>
        /// <returns>The loaded CarCollection.</returns>
        public CarCollection Load(string filePath)
        {
            using (var fileStream = File.Open(filePath, FileMode.Open))
            {
                return Load(fileStream);
            }
        }

        /// <summary>Loads a new CarCollection from the specified stream.</summary>
        /// <param name="stream">The stream to load CarCollection from.</param>
        /// <returns>The loaded CarCollection.</returns>
        public CarCollection Load(Stream stream)
        {
            var fileFormat = GetFileFormat(stream);
            if (fileFormat == CarFileFormat.Unknown) throw new CarCollectionFormatException("Cannot load because file format was not determined");

            var serializer = FileFormats[fileFormat].Value;
            CurrentCarCollection = serializer.Read(stream);

            return CurrentCarCollection;
        }

        /// <summary>Saves the object's data to the specified stream.</summary>
        /// <param name="stream">The stream to save the object's data to.</param>
        /// <param name="fileFormat">Car file format to save</param>
        /// <param name="carCollection">Saves CurrentCarCollection by default</param>
        public void Save(Stream stream, CarFileFormat fileFormat, CarCollection carCollection = null)
        {
            carCollection = carCollection ?? CurrentCarCollection;
            if (!FileFormats.ContainsKey(fileFormat)) throw new CarCollectionFormatException("Cannot save because the file format is not supported");

            var serializer = FileFormats[fileFormat].Value;
            serializer.Write(stream, carCollection);
        }

        /// <summary>
        /// Saves the object's data to the specified file location.
        /// </summary>
        /// <param name="filePath">The file path to save the object's data to.</param>
        /// <param name="fileFormat">Car file format to save</param>
        /// <param name="carCollection">Saves CurrentCarCollection by default</param>
        /// <param name="overWrite">if set to <c>true</c> over write the file contents, otherwise append will occur.</param>
        public void Save(string filePath, CarFileFormat fileFormat, bool overWrite = false, CarCollection carCollection = null)
        {
            using (var fileStream = File.Open(filePath, overWrite ? FileMode.Create : FileMode.CreateNew))
            {
                Save(fileStream, fileFormat);
            }
        }

        /// <summary>
        /// Converts the object's data from the sourceFilePath to the destFilePath file location.
        /// </summary>
        /// <param name="sourceFilePath">The file path to load the object's data from.</param>
        /// <param name="destFilePath">The file path to save the object's data to.</param>
        /// <param name="destFileFormat">Car file format to convert to</param>
        /// <param name="overWrite">if set to <c>true</c> over write the file contents, otherwise append will occur.</param>
        public void Convert(string sourceFilePath, string destFilePath, CarFileFormat destFileFormat, bool overWrite = false)
        {
            Load(sourceFilePath);
            using (var fileStream = File.Open(destFilePath, overWrite ? FileMode.Create : FileMode.CreateNew))
            {
                Save(fileStream, destFileFormat);
            }
        }
    }
}
