using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Library
{
    public static partial class Utils
    {
        /// <summary>
        /// Contains methods for converting string [] into string with delimiter and vice versa
        /// </summary>
        public static class Array
        {
            /// <summary>
            /// Converts a string into string [] using specified deliminator
            /// </summary>
            /// <param name="data">(string) delimited string</param>
            /// <param name="deliminator">(string) deliminator symbol</param>
            /// <returns>(string []) array of strings</returns>
            public static string[] FromString(string data, string deliminator)
            {
                return data.Split(deliminator.ToCharArray());
            }

            /// <summary>
            /// Converts a string [] into string using specified deliminator
            /// </summary>
            /// <param name="array">(string []) array of strings</param>
            /// <param name="deliminator">(string) deliminator symbol</param>
            /// <returns>(string) delimited string</returns>
            public static string ToString(string[] array, string deliminator)
            {
                return string.Join(deliminator, array);
            }
        }
    }
}