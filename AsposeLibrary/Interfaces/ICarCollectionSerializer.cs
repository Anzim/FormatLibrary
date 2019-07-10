using System.IO;
using AsposeLibrary.Models;

namespace AsposeLibrary.Interfaces
{
    public interface ICarCollectionSerializer
    {
        void Write(Stream destinationStream, CarCollection cars);
        CarCollection Read(Stream sourceStream);
    }
}