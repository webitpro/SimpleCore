using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Text;

namespace Core.Library
{
    public static partial class Utils
    {
        /// <summary>
        /// Contains methods to work with Query String object
        /// </summary>
        public static class QS
        {
            /// <summary>
            /// Generates query string or adds new parameter to existing query string
            /// </summary>
            /// <param name="queryString">(NameValueCollection) existing query string name-value collection or null if query string doesn't exist</param>
            /// <param name="param">(string) new parameter to add</param>
            /// <param name="value">(string) new parameter value to add</param>
            /// <returns>(string) Query String with new parameter</returns>
            public static string AddParam(NameValueCollection queryString, string param, string value)
            {
                NameValueCollection queryDict = (queryString != null) ? new NameValueCollection(queryString) : new NameValueCollection();
                queryDict[param] = value;

                StringBuilder querystr = new StringBuilder("?");
                foreach (string key in queryDict)
                {
                    querystr.AppendFormat("{0}={1}&", key, queryDict[key]);
                }
                querystr.Remove(querystr.Length - 1, 1);

                return querystr.ToString();
            }

            /// <summary>
            /// Generates query string or adds new multiple parameters to existing query string
            /// </summary>
            /// <param name="queryString">(NameValueCollection) existing query string name-value collection of null if query string doesn't exist</param>
            /// <param name="paramArray">(string []) an array of parameters to add</param>
            /// <param name="valueArray">(string []) an array of parameter values to add</param>
            /// <returns></returns>
            public static string AddParams(NameValueCollection queryString, string[] paramArray, string[] valueArray)
            {
                NameValueCollection queryDict = (queryString != null) ? new NameValueCollection(queryString) : new NameValueCollection();
                for (int i = 0; i < Math.Min(paramArray.Length, valueArray.Length); i++)
                {
                    string param = paramArray[i];
                    string value = valueArray[i];

                    if (!System.String.IsNullOrEmpty(param))
                        queryDict[param] = value;
                }

                StringBuilder querystr = new StringBuilder("?");
                foreach (string key in queryDict)
                {
                    querystr.AppendFormat("{0}={1}&", key, queryDict[key]);
                }
                querystr.Remove(querystr.Length - 1, 1);

                return querystr.ToString();
            }

            /// <summary>
            /// Removes a parameter from existing query string
            /// </summary>
            /// <param name="queryString">(NameValueCollection) existing query string name-value collection</param>
            /// <param name="param">(string) parameter to be removed</param>
            /// <returns>(string) Query String</returns>
            public static string RemoveParam(NameValueCollection queryString, string param)
            {
                NameValueCollection queryDict = new NameValueCollection(queryString);

                if (!System.String.IsNullOrEmpty(param))
                    queryDict.Remove(param);

                StringBuilder querystr = new StringBuilder("?");
                foreach (string key in queryDict)
                {
                    querystr.AppendFormat("{0}={1}&", key, queryDict[key]);
                }
                querystr.Remove(querystr.Length - 1, 1);

                return querystr.ToString();
            }

            /// <summary>
            /// Removes multiple parameters from existing query string
            /// </summary>
            /// <param name="queryString">(NameValueCollection) existing query string name-value collection</param>
            /// <param name="paramArray">(string) an array of parameters to be removed</param>
            /// <returns>(string) Query String</returns>
            public static string RemoveParams(NameValueCollection queryString, string[] paramArray)
            {
                NameValueCollection queryDict = new NameValueCollection(queryString);

                foreach (string param in paramArray)
                {
                    if (!System.String.IsNullOrEmpty(param))
                        queryDict.Remove(param);
                }

                StringBuilder querystr = new StringBuilder("?");
                foreach (string key in queryDict)
                {
                    querystr.AppendFormat("{0}={1}&", key, queryDict[key]);
                }
                querystr.Remove(querystr.Length - 1, 1);

                return querystr.ToString();
            }
        }
    }
}