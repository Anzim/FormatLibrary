using System.IO;
using AsposeLibrary.Models;

namespace AsposeLibrary.Interfaces
{
    public interface ICarCollectionSerializer
    {
        bool IsThisFileFormat(Stream stream);
        void Write(Stream destinationStream, CarCollection cars);
        CarCollection Read(Stream sourceStream);
    }
}