using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Core.Library;
using Core.Resources;

namespace Core.Helpers
{
    public static partial class DigitalFile
    {
        public class Posted
        {
            //////////
            // VARS //
            //////////       

            protected HttpPostedFileBase file = null;
            protected ModelStateDictionary model = null;
            protected string key = null;

            protected UrlHelper urlHelper = null;

            ////////////////
            // PROPERTIES //
            ////////////////

            public bool IsRequired { get; protected set; } //is file required
            public string Name { get; protected set; } //file name
            public bool HasFile
            {
                get
                {
                    return (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName));
                }
            }
            protected bool IsValidForUpload { get; set; }
            public string SavePath { get; protected set; } //save path       
            public string OriginalPath { get; set; } //original path for previously saved file
            public string SavedName { get; set; }

            public bool IsSaved { get; protected set; } //is file saved on the disk


            /////////////////
            // CONSTRUCTOR //
            /////////////////

            /// <summary>
            /// File Helper constructor
            /// </summary>
            /// <param name="postedFileBase">uploaded file</param>
            /// <param name="modelName">model name/id</param>
            /// <param name="modelState"></param>
            public Posted(HttpPostedFileBase postedFileBase, string modelName, ModelStateDictionary modelState)
            {
                file = postedFileBase;
                key = modelName;
                model = modelState;

                ResetProps();

                HttpContextWrapper contextWrapper = new HttpContextWrapper(HttpContext.Current);
                RequestContext reqContext = new RequestContext(contextWrapper, RouteTable.Routes.GetRouteData(contextWrapper));
                urlHelper = new UrlHelper(reqContext);
            }
            public Posted()
            {
            }
            /////////////
            // METHODS //
            /////////////

            /// <summary>
            /// Validate file extension
            /// override for different file types
            /// </summary>
            /// <returns></returns>
            protected virtual bool ValidateExtension()
            {
                return true;
            }



            /// <summary>
            /// Validate if file exists, file content length, file name and file extension
            /// </summary>
            /// <param name="required">Is file required</param>
            /// <returns></returns>
            public void ValidateForUpload(bool required)
            {
                IsRequired = required;

                bool bValid = true;
                bool bHasFile = false;
                #region check if file exists and set file name
                if (file != null)
                {
                    bHasFile = true;
                    //check for file name and set property "Name"
                    if (!String.IsNullOrEmpty(file.FileName))
                    {
                        Name = file.FileName;
                    }
                }
                else
                {
                    bHasFile = false;
                }
                #endregion

                #region check if file is required or not and validate content length, and file name
                if (IsRequired)
                {
                    //file is required
                    if (!bHasFile || file.ContentLength == 0 || string.IsNullOrEmpty(Name))
                    {
                        bValid = false;
                        if (file == null)
                        {
                            model.AddModelError(key, "File is missing");
                        }
                        else
                        {
                            if (file.ContentLength == 0)
                            {
                                model.AddModelError(key, "File has zero length");
                            }
                            if (string.IsNullOrEmpty(Name))
                            {
                                model.AddModelError(key, "File name is missing");
                            }
                        }
                    }
                }
                else
                {
                    //file is not required
                    if (bHasFile)
                    {
                        if (file.ContentLength == 0 || string.IsNullOrEmpty(Name))
                        {
                            bValid = false;
                            if (file.ContentLength == 0)
                            {
                                model.AddModelError(key, "File has zero length");
                            }
                            if (string.IsNullOrEmpty(Name))
                            {
                                model.AddModelError(key, "File name is missing");
                            }
                        }
                    }
                }
                #endregion

                #region if everything is valid, check for extension
                if (bValid && bHasFile)
                {
                    if (!ValidateExtension())
                    {
                        model.AddModelError(key, "Invalid file type");
                        bValid = false;
                    }
                }
                #endregion

                IsValidForUpload = bValid && bHasFile;

            }

            /// <summary>
            /// Save File
            /// </summary>
            /// <param name="fileName"> file name with extension to be saved</param>
            /// <returns>bool (IsSaved), read SavePath after successful saving to get DB value</returns>
            public bool Save(string fileName, string uniqueId = "")
            {
                if (IsValidForUpload)
                {
                    SavedName = ((!string.IsNullOrEmpty(uniqueId)) ? uniqueId : "") + Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName);
                    try
                    {
                        file.SaveAs(Reference.UploadPath(SavedName));
                        IsSaved = true;
                        SavePath = "Content/data/" + SavedName;
                    }
                    catch (Exception ex)
                    {
                        model.AddModelError(key, ex.Message);
                        IsSaved = false;
                    }
                }
                else
                {
                    IsSaved = false;
                }

                return IsSaved;
            }

            /// <summary>
            /// save image to a disk
            /// </summary>
            /// <param name="src"></param>
            /// <param name="fileName">including extension</param>
            /// <param name="quality"></param>
            /// <returns></returns>
            public bool Save(Bitmap src, string fileName, long quality, ImageFormat f, string uniqueId = "")
            {
                SavedName = ((!string.IsNullOrEmpty(uniqueId)) ? uniqueId : "") + Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName);

                EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
                ImageCodecInfo codec = Utils.Image.GetEncoderInfo(f);
                if (codec == null)
                {
                    IsSaved = false;
                    return IsSaved;
                }

                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                try
                {
                    src.Save(Reference.UploadPath(SavedName), codec, encoderParams);
                    IsSaved = true;
                    SavePath = "Content/data/" + SavedName;
                }
                catch (Exception ex)
                {
                    model.AddModelError(key, ex.Message);
                    IsSaved = false;
                }
                return IsSaved;
            }

            /// <summary>
            /// Clean up properties and files uploaded by not saved to the database
            /// </summary>
            public void Cleanup(bool bDeleteFile = false)
            {
                if (bDeleteFile)
                {
                    if (File.Exists(Reference.UploadPath(OriginalPath)))
                    {
                        try
                        {
                            //delete uploaded file because database is not updated
                            File.Delete(Reference.UploadPath(OriginalPath));
                        }
                        catch (Exception ex)
                        {
                            model.AddModelError(key, ex.Message);
                        }
                    }
                }

                //reset properties
                ResetProps();
            }

            /// <summary>
            /// Reset all properties to their default values
            /// </summary>
            protected void ResetProps()
            {
                IsRequired = true;
                Name = null;
                IsValidForUpload = false;
                SavePath = null;
                OriginalPath = null;
                SavedName = "";
                IsSaved = false;

            }

        }

        public class Compressed : Posted
        {
            public Compressed(HttpPostedFileBase file, string modelName, ModelStateDictionary modelState)
                : base(file, modelName, modelState)
            {
            }

            protected override bool ValidateExtension()
            {
                return Utils.File.Type.IsCompressedFile(Path.GetExtension(this.file.FileName));
            }
        }

        public class Image : Posted
        {
            private Bitmap img = null;
            ImageFormat format = null;

            public Image(HttpPostedFileBase file, string modelName, ModelStateDictionary modelState)
                : base(file, modelName, modelState)
            {
                if (file != null)
                {
                    img = new Bitmap(this.file.InputStream);
                    string ext = Path.GetExtension(file.FileName);
                    format = GetImageFormatFromExtension(ext);
                }
            }

            private string GetExtensionFromImageFormat(ImageFormat f)
            {
                string ext = "";
                if (f == ImageFormat.Jpeg)
                {
                    ext = ".jpg";
                }
                else if (f == ImageFormat.Gif)
                {
                    ext = ".gif";
                }
                else if (f == ImageFormat.Png)
                {
                    ext = ".png";
                }
                else if (f == ImageFormat.Tiff)
                {
                    ext = ".tiff";
                }
                return ext;
            }
            private ImageFormat GetImageFormatFromExtension(string ext)
            {
                ImageFormat f = null;
                switch (ext)
                {
                    case ".jpg":
                        f = ImageFormat.Jpeg;
                        break;
                    case ".jpeg":
                        f = ImageFormat.Jpeg;
                        break;
                    case ".gif":
                        f = ImageFormat.Gif;
                        break;
                    case ".png":
                        f = ImageFormat.Png;
                        break;
                    case ".tiff":
                        f = ImageFormat.Tiff;
                        break;
                }
                return f;
            }

            protected override bool ValidateExtension()
            {
                return Utils.File.Type.IsImageFile(Path.GetExtension(this.file.FileName));
            }

            public Size? GetDimensions()
            {
                if (img != null)
                {
                    return new Size(img.Width, img.Height);
                }
                return null;
            }


            public bool Save(string fileNameWithoutExtension, Size? dims, bool resizeAndCrop, string uniqueId = "")
            {
                bool? success = null;

                Size dimensions = new Size(0, 0);
                if (dims.HasValue)
                {
                    dimensions = (Size)dims;
                }

                if ((img.Width != dimensions.Width && dimensions.Width > 0) || (img.Height != dimensions.Height && dimensions.Height > 0))
                {
                    Bitmap resizedImage = resizeAndCrop ? Utils.Image.ResizeAndCrop(img, dimensions.Width, dimensions.Height) : Utils.Image.ResizeImage(img, dimensions.Width, dimensions.Height);
                    success = base.Save(resizedImage, fileNameWithoutExtension + GetExtensionFromImageFormat(format), 80, format, uniqueId);

                    resizedImage.Dispose();
                }

                if (!success.HasValue)
                {
                    return base.Save(fileNameWithoutExtension + GetExtensionFromImageFormat(format));
                }
                else
                {
                    return (bool)success;
                }
            }

        }

        public class Audio : Posted
        {
            public Audio(HttpPostedFileBase file, string modelName, ModelStateDictionary modelState)
                : base(file, modelName, modelState)
            {
            }

            protected override bool ValidateExtension()
            {
                return Utils.File.Type.IsAudioFile(Path.GetExtension(this.file.FileName));
            }
        }

        public class Video : Posted
        {
            public Video(HttpPostedFileBase file, string modelName, ModelStateDictionary modelState)
                : base(file, modelName, modelState)
            {

            }

            protected override bool ValidateExtension()
            {
                return Utils.File.Type.IsVideoFile(Path.GetExtension(this.file.FileName));
            }
        }

        public class Document : Posted
        {
            public Document(HttpPostedFileBase file, string modelName, ModelStateDictionary modelState)
                : base(file, modelName, modelState)
            {
            }

            protected override bool ValidateExtension()
            {
                return Utils.File.Type.IsDocumentFile(Path.GetExtension(this.file.FileName));
            }
        }

        public class Data : Posted
        {
            public Data(HttpPostedFileBase file, string modelName, ModelStateDictionary modelState)
                : base(file, modelName, modelState)
            {

            }

            protected override bool ValidateExtension()
            {
                return Utils.File.Type.IsDataFile(Path.GetExtension(this.file.FileName));
            }
        }
    }

}