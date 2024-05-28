using Template.Interfaces;

namespace Template.Platforms.Source;

public class PathService : IPathService
{
    public string GetDatabasePath()
    {
        string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

        if (!Directory.Exists(libFolder))
        {
            Directory.CreateDirectory(libFolder);
        }
        var sqliteFilename = "Template.db3";
        return Path.Combine(libFolder, sqliteFilename);
    }

    public string SaveImage(string filename, MemoryStream stream)
    {
        try
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, filename);

            //Create a file and write the stream into it.
            FileStream fileStream = File.Open(filePath, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(fileStream);

            fileStream.Flush();
            fileStream.Close();
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("Error SaveImage: {0}", ex.Message));
        }
        return "";
    }

    public byte[] GetImage(string filename)
    {
        try
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, filename);


            byte[] bytes = File.ReadAllBytes(filePath);

            return bytes;


        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("Error GetImage: {0}", ex.Message));
        }
        return null;
    }

}

