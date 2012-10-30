using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Library
{
    public static class Type
    {
        /// <summary>
        /// xPression: Empty, Alpha, Numeric, AlphaNumeric, NonAlphaNumeric, Mix
        /// </summary>
        public enum xPression { Empty, Alpha, Numeric, AlphaNumeric, SpecialCharacters, AlphaMix, NumericMix, Mix };

        /// <summary>
        /// Geography Name Type: FullName, Abbreviation
        /// for World Countries: United States, US
        /// for US States: Washington, WA
        /// </summary>
        public enum GeographyNameType { FullName, Abbreviation };

        /// <summary>
        /// Media Types
        /// </summary>
        public enum MediaType
        {
            Photo,
            Video,
            Audio,
            Document
        }
    }
}