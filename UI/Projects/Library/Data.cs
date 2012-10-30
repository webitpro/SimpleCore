using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;


namespace Core.Library
{
    public static partial class Utils
    {
        
        
        /// <summary>
        /// Contains methods to retrieve a list of World Countries, US States
        /// Required to have "countries.xml" and "states.xml" files within the application
        /// </summary>
        public static class Data
        {
            /// <summary>
            /// Creates a string [] of World Countries from XML file (Must be named "countries.xml")
            /// </summary>
            /// <param name="root">string)( root directory for xml file</param>
            /// <param name="type">(GeographyNameType) return value type for country name</param>
            /// <returns>(string[]) string array of world countries</returns>
            public static string[] WorldCountries(string root, Type.GeographyNameType type)
            {
                List<string> countryList = new List<string>();

                string strPath = root + @"countries.xml";

                XDocument doc = XDocument.Load(strPath);
                var query = from xml in doc.Descendants("country")
                            select ((Type.GeographyNameType.FullName == type) ? xml.Attribute("name").Value : xml.Attribute("code").Value);

                foreach (var item in query)
                {
                    countryList.Add(item);
                }

                return countryList.ToArray();
            }

            /// <summary>
            /// Creates a string [] of US States from XML file. Must be named ("states.xml")
            /// </summary>
            /// <param name="root">(string) root directory for xml file</param>
            /// <param name="type">(GeographyNameType) return value type for US States</param>
            /// <returns>(string[]) string array of US States</returns>
            public static string[] USStates(string strRootPath, Type.GeographyNameType type)
            {
                List<string> stateList = new List<string>();

                string strPath = strRootPath + @"states.xml";

                XDocument doc = XDocument.Load(strPath);
                var query = from xml in doc.Descendants("item")
                            select ((Type.GeographyNameType.FullName == type) ? xml.Attribute("name").Value : xml.Value);
                foreach (var item in query)
                {
                    stateList.Add(item);
                }

                return stateList.ToArray();
            }
        }
    }
}