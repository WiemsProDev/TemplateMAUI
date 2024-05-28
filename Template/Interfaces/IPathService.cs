using System.IO;

namespace Template.Interfaces
{
    public interface IPathService
    {
        string GetDatabasePath();
        string SaveImage(string fileName, MemoryStream stream);
        byte[] GetImage(string fileName);
    }
}
