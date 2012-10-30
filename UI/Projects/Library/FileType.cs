using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Library
{
    public static partial class Utils
    {
        public static partial class File
        {

            public static class Type
            {
                public static List<Category> Categories
                {
                    get
                    {
                        return getCategories();
                    }
                }

                #region Compressed File
                public static string[] CompressedFileExtensions()
                {
                    return new string[] { ".7z", ".gz", ".rar", ".zip", ".zipx" };
                }
                public static bool IsCompressedFile(string extension)
                {
                    string ext = extension.ToLower();
                    return CompressedFileExtensions().Any(x => x == ext);
                }
                #endregion

                #region Image File

                public static string[] ImageFileExtensions()
                {
                    return new string[] { ".bmp", ".eps", ".gif", ".jpg", ".jpeg", ".png", ".tif" };
                }

                public static bool IsImageFile(string extension)
                {
                    string ext = extension.ToLower();
                    return ImageFileExtensions().Any(x => x == ext);
                }
                #endregion

                #region Audio File

                public static string[] AudioFileExtensions()
                {
                    return new string[] { ".aif", ".m4a", ".mp3", ".mpa", ".ra", ".wav", ".wma" };
                }

                public static bool IsAudioFile(string extension)
                {
                    string ext = extension.ToLower();
                    return AudioFileExtensions().Any(x => x == ext);
                }
                #endregion

                #region Video File
                public static string[] VideoFileExtensions()
                {
                    return new string[] { ".avi", ".flv", ".mpg", ".rm", ".swf", ".vob", ".wmv" };
                }
                public static bool IsVideoFile(string extension)
                {
                    string ext = extension.ToLower();
                    return VideoFileExtensions().Any(x => x == ext);
                }
                #endregion

                #region Document File
                public static string[] DocumentFileExtensions()
                {
                    return new string[] { ".pdf", ".doc", ".docx", ".txt", ".rtf" };
                }
                public static bool IsDocumentFile(string extension)
                {
                    string ext = extension.ToLower();
                    return DocumentFileExtensions().Any(x => x == ext);
                }
                #endregion

                #region Data File
                public static string[] DataFileExtensions()
                {
                    return new string[] { ".csv", ".pps", ".ppt", ".pptx", ".xls", ".xlsx", ".xml" };
                }
                public static bool IsDataFile(string extension)
                {
                    string ext = extension.ToLower();
                    return DataFileExtensions().Any(x => x == ext);
                }
                #endregion

                private static List<Category> getCategories()
                {
                    string[] cat = new string[] { "Compressed", "Data", "Document", "Image", "Audio", "Video" };
                    List<Category> categories = new List<Category>();
                    foreach (string c in cat)
                    {
                        categories.Add(new Category()
                        {
                            Name = c,
                            Extensions = getExtensions(c)
                        });
                    }

                    return categories;
                }
                private static string[] getExtensions(string c)
                {
                    switch (c)
                    {
                        case "Compressed":
                            return CompressedFileExtensions();
                        case "Data":
                            return DataFileExtensions();
                        case "Document":
                            return DocumentFileExtensions();
                        case "Image":
                            return ImageFileExtensions();
                        case "Audio":
                            return AudioFileExtensions();
                        case "Video":
                            return VideoFileExtensions();
                        default:
                            return new string[] { };
                    }
                }
                public class Category
                {
                    public string Name { get; set; }
                    public string[] Extensions { get; set; }
                }
            }
        }
    }
}