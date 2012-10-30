using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Core.Library
{
    public static partial class Utils
    {
        /// <summary>
        ///     A collection of utilities for working with images
        /// </summary>
        public static class Image
        {
            /// <summary>
            ///     Get array of accepted image extensions
            /// </summary>
            public static string[] GetAcceptedImageExtensions()
            {
                return new string[] { ".jpg", ".jpeg", ".gif", ".png" };
            }

            /// <summary>
            ///     Check if file extension is in the accepted image extension list
            /// </summary>
            /// <param name="extension">File extension including the period (ie .png)</param>
            public static bool IsImage(string extension)
            {
                string ext = extension.ToLower();
                return GetAcceptedImageExtensions().Any(ex => ex == ext);
            }

            /// <summary>
            ///     Crop a bitmap source image, simply returns the top left piece of the image matching the specified dimensions
            /// </summary>
            /// <param name="src"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            public static Bitmap CropImage(Bitmap src, int width, int height)
            {
                Rectangle cropRect = new Rectangle(0, 0, width, height);

                return src.Clone(cropRect, src.PixelFormat);
            }

            /// <summary>
            ///     Crop a bitmap source image, gets the specified dimensions starting at the specified x,y coordinate
            /// </summary>
            /// <param name="src"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            public static Bitmap CropImage(Bitmap src, int x, int y, int width, int height)
            {
                Rectangle cropRect = new Rectangle(x, y, width, height);

                return src.Clone(cropRect, src.PixelFormat);
            }

            /// <summary>
            ///     Crop a bitmap source image, gets the middle of the image
            /// </summary>
            /// <param name="src"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static Bitmap CropImageMiddle(Bitmap src, int width, int height)
            {
                int startx = (src.Width - width) / 2;
                int starty = (src.Height - height) / 2;

                return CropImage(src, startx, starty, width, height);
            }

            /// <summary>
            ///     Resize a image bitmap source image to specified dimensions.  If width or height is zero then the image
            ///     will be resized by aspect ratio using the specified parameter.  If both width and height are zero then
            ///     the original image is returned.
            /// </summary>
            /// <param name="src"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            public static Bitmap ResizeImage(Bitmap src, int width, int height)
            {
                if (width <= 0 && height <= 0)
                {
                    return src;
                }

                int useWidth = width;
                int useHeight = height;

                if (useWidth <= 0)
                {
                    double aspectRatio = (double)src.Width / src.Height;
                    useWidth = (int)Math.Round(aspectRatio * height);
                }
                else if (useHeight <= 0)
                {
                    double aspectRatio = (double)src.Height / src.Width;
                    useHeight = (int)Math.Round(aspectRatio * width);
                }

                Bitmap target = new Bitmap(useWidth, useHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(src, 0, 0, useWidth, useHeight);
                }

                return target;
            }

            /// <summary>
            ///     Resizes (if necessary) and crops (if necessary) to the specified width and height, gets the middle of the image if it needs to crop
            /// </summary>
            /// <param name="src"></param>
            /// <param name="targetWidth"></param>
            /// <param name="targetHeight"></param>
            /// <returns></returns>
            public static Bitmap ResizeAndCrop(Bitmap src, int targetWidth, int targetHeight)
            {
                if (targetWidth == src.Width && targetHeight == src.Height)
                    return src;

                if (targetWidth > src.Width && targetHeight > src.Height)
                    return CropImageMiddle(src, targetWidth, targetHeight);

                double targetRatio = (double)targetWidth / targetHeight;
                double srcRatio = (double)src.Width / src.Height;

                if (srcRatio == targetRatio)
                {
                    return ResizeImage(src, targetWidth, targetHeight);
                }
                else if (srcRatio > targetRatio)
                {
                    return CropImageMiddle(ResizeImage(src, 0, targetHeight), targetWidth, targetHeight);
                }
                else
                    return CropImageMiddle(ResizeImage(src, targetWidth, 0), targetWidth, targetHeight);
            }

            /// <summary>
            ///     Find an encoder for a specified image format
            /// </summary>
            /// <param name="format">Desired format</param>
            public static ImageCodecInfo GetEncoderInfo(ImageFormat format)
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

                foreach (ImageCodecInfo codecInfo in codecs)
                {
                    if (codecInfo.FormatID == format.Guid)
                        return codecInfo;
                }

                return null;
            }

            /// <summary>
            ///     Save bitmap to disk as a JPEG
            /// </summary>
            /// <param name="src"></param>
            /// <param name="path">Server file system path</param>
            /// <param name="quality"></param>
            /// <remarks>
            ///     The quality of the built-in JPEG encoder is not terribly good so this should probably only be used for thumbnails.
            ///     Another solution should be found for better quality.
            /// </remarks>
            public static bool SaveJpeg(Bitmap src, string path, long quality)
            {
                EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
                ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);

                if (jpegCodec == null)
                    return false;

                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                src.Save(path, jpegCodec, encoderParams);

                return true;
            }
        }
    }
}