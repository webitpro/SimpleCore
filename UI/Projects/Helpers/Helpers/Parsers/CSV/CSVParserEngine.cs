using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;

namespace Core.Helpers
{
    public static partial class Parser
    {
        public static partial class Engine
        {
            /// <summary>
            /// Summary description for CSVParserEngine
            /// <typeparam name="T">Class with properties matching the order of columns in CSV file</typeparam>
            /// </summary>
            public class CSV<T> where T : new()
            {
                public bool RemoveHeaders { get; set; }
                public CSV(bool removeHeaders)
                {
                    RemoveHeaders = removeHeaders;
                }

                public List<T> ParseFile(string path)
                {
                    StreamReader reader = new StreamReader(path);
                    string data = reader.ReadToEnd();
                    reader.Close();

                    return Parse(data);

                }

                public List<T> ParseFile(Stream inputStream)
                {
                    StreamReader reader = new StreamReader(inputStream);
                    string data = reader.ReadToEnd();
                    reader.Close();

                    return Parse(data);
                }

                public List<T> Parse(string data)
                {
                    List<T> list = new List<T>();
                    StringBuilder colValue = new StringBuilder();
                    ArrayList arl = new ArrayList();


                    //parse string
                    char[] chars = data.ToCharArray();
                    bool bStart = false;
                    bool bEnd = false;
                    StringBuilder sValue = new StringBuilder();
                    foreach (char c in chars)
                    {
                        if (c != ',')
                        {
                            //check for starting/ending quotes
                            if (c == '"')
                            {
                                if (bStart)
                                {
                                    bEnd = true;
                                }
                                else
                                {
                                    bStart = true;
                                }
                            }

                            if (bStart)
                            {
                                //value between quotes
                                sValue.Append(c);
                                if (bEnd)
                                {
                                    sValue = sValue.Remove(0, 1).Remove(sValue.Length - 1, 1);//remove quotes
                                    colValue = sValue;//set colValue = sValue
                                    sValue = new StringBuilder();//reset to empty						
                                    bStart = false;//reset start
                                    bEnd = false;//reset end						
                                }
                            }
                            else
                            {
                                //value not between quotes
                                if (c == '\n' || c == '\r')
                                {
                                    if (c == '\r')
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        arl.Add(colValue.ToString().Trim());//add last value before completing record
                                        colValue = new StringBuilder();//reset colValue

                                        //new line / record
                                        //create new instance of the type
                                        T o = new T();
                                        System.Type t = typeof(T);

                                        //use reflection to populate properties of the object/class
                                        PropertyInfo[] properties = t.GetProperties();

                                        //loop through array of strings
                                        int index = 0;
                                        foreach (string s in arl)
                                        {
                                            properties[index].SetValue(o, arl[index], null);
                                            index++;
                                        }

                                        //add row to the list
                                        list.Add(o);

                                        //reset arr values
                                        arl.Clear();
                                        colValue = new StringBuilder();//reset colValue							
                                    }

                                }
                                else
                                {
                                    //add char to column Value
                                    colValue.Append(c);
                                }
                            }
                        }
                        else
                        {
                            if (bStart)
                            {
                                //add to sValue
                                sValue.Append(c);
                            }
                            else
                            {
                                arl.Add(colValue.ToString().Trim());//add to array
                                colValue = new StringBuilder();//reset colValue					
                            }
                        }
                    }


                    //remove headers
                    if (RemoveHeaders)
                    {
                        list.RemoveAt(0);
                    }

                    return list;

                }

            }
        }
    }
}
