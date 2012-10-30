using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;

namespace Core.Library
{
    public static partial class Utils
    {
        public static class Request
        {
            /// <summary>
            /// Content Type: application/x-form-urlencoded, multipart/form-data
            /// </summary>
            public enum ContentType
            {
                applicationxwwwformurlencoded,
                multipartformdata,
                applicationatomxml
            };
            /// <summary>
            /// Retrieves a form encoding type
            /// </summary>
            /// <param name="cType">(ContentType) form encoding type</param>
            /// <returns>(string) an actual string value of Content Type</returns>
            public static string GetContentType(ContentType cType)
            {
                ArrayList arl = new ArrayList();
                arl.Add("application/x-www-form-urlencoded");
                arl.Add("multipart/form-data");
                arl.Add("application/atom+xml; charset=UTF-8");

                return arl[cType.GetHashCode()].ToString();
            }

            /// <summary>
            /// Http POST Request
            /// </summary>
            /// <param name="url">(string) POST url</param>
            /// <param name="cType">(ContentType) form encoding type</param>
            /// <param name="enc">(Encoding) text endcoding</param>
            /// <param name="parameters">(string []) an array of parameters to be sent with POST request</param>
            /// <param name="values">(string []) an array of parameter values to be sent with POST request</param>
            /// <param name="requestHeaders">(ArrayList) array list of request header values (name: value) format</param>
            /// <returns>(HttpWebResponse) object</returns>
            public static HttpWebResponse Post(string url, ContentType cType, Encoding enc, string[] parameters, string[] values, ArrayList requestHeaders)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (requestHeaders != null)
                {
                    foreach (string header in requestHeaders)
                    {
                        request.Headers.Add(header);
                    }
                }

                string data = QS.AddParams(null, parameters, values);

                byte[] buffer = enc.GetBytes(data);

                request.ContentLength = buffer.Length;
                request.Method = "POST";
                request.ContentType = GetContentType(cType);

                Stream postStream = request.GetRequestStream();
                postStream.Write(buffer, 0, buffer.Length);
                postStream.Close();

                return (HttpWebResponse)request.GetResponse();

            }

            /// <summary>
            /// Http GET Request
            /// </summary>
            /// <param name="url">(string) GET url</param>
            /// <param name="enc">(Encoding) text encoding</param>
            /// <param name="parameters">(string []) an array of parameters to be sent with GET request</param>
            /// <param name="values">(string []) an array of parameter values to be send with GET request</param>
            /// <param name="requestHeaders">(ArrayList) array list of requets header values (name: value) format</param>
            /// <returns>(HttpWebResponse) object</returns>
            public static HttpWebResponse Get(string url, Encoding enc, string[] parameters, string[] values, ArrayList requestHeaders)
            {
                string data = QS.AddParams(null, parameters, values);


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + data);

                if (requestHeaders != null)
                {
                    foreach (string header in requestHeaders)
                    {
                        request.Headers.Add(header);
                    }
                }

                request.Method = "GET";

                return (HttpWebResponse)request.GetResponse();


            }

            /// <summary>
            /// Read response from HttpWebResponse object
            /// </summary>
            /// <param name="response">(HttpWebResponse) object</param>
            /// <param name="enc">(Encoding) text encoding</param>
            /// <returns>(string) response</returns>
            public static string ReadResponse(HttpWebResponse response, Encoding enc)
            {
                string r = "";
                StreamReader reader = new StreamReader(response.GetResponseStream(), enc);
                r = reader.ReadToEnd();
                response.Close();
                reader.Close();

                return r;
            }

        }
    }
}