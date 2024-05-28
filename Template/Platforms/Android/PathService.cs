using Java.IO;
using Template.Interfaces;

namespace Template.Platforms.Source;

public class PathService : IPathService
{
    public string GetDatabasePath()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        return Path.Combine(path, "Template.db3");

    }

    public string SaveImage(string fileName, MemoryStream stream)
    {
        try
        {
            Java.IO.File extFile = new(Android.App.Application.Context.GetExternalFilesDir(Environment.CurrentDirectory), fileName);
            Java.IO.File extDir = extFile.ParentFile;
            // Copy file to external storage to allow other apps to access ist
            if (System.IO.File.Exists(extFile.AbsolutePath))
            {
                System.IO.File.Delete(extFile.AbsolutePath);
            }

            //Write the stream into file.
            FileOutputStream outs = new(extFile);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();

            //String mime = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(extFile.AbsolutePath);
            // if copying was successful, start Intent for opening this file
            if (System.IO.File.Exists(extFile.AbsolutePath))
            {
                return extFile.AbsolutePath;
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(string.Format("Error SaveImage: {0}", ex.Message));
        }
        return "";
    }


    public byte[] GetImage(string fileName)
    {
        try
        {
            Java.IO.File extFile = new(Android.App.Application.Context.GetExternalFilesDir(Environment.CurrentDirectory), fileName);
            Java.IO.File extDir = extFile.ParentFile;
            // Copy file to external storage to allow other apps to access ist
            if (System.IO.File.Exists(extFile.AbsolutePath))
            {
                FileInputStream outs = new(extFile);

                byte[] bytes = new byte[extFile.Length()];
                outs.Read(bytes, 0, (int)extFile.Length());

                return bytes;

            }

            return null;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(string.Format("Error LoadingImage: {0}", ex.Message));
        }
        return null;
    }
}

