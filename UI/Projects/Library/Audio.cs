using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Library
{
    public static partial class Utils
    {
        public static class Audio
        {
            /// <summary>
            ///     Get array of accepted audio extensions
            /// </summary>
            public static string[] GetAcceptedAudioExtensions()
            {
                return new string[] { ".3gp", ".mp3", ".m4p", ".aac", ".wav", ".wma" };
            }

            /// <summary>
            ///     Check if file extension is in the accepted audio file extension list
            /// </summary>
            /// <param name="extension">File extension including the period (ie .mp3)</param>
            public static bool IsAudioFile(string extension)
            {
                string ext = extension.ToLower();
                return GetAcceptedAudioExtensions().Any(ex => ex == ext);
            }
        }
    }
}