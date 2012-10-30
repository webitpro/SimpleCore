using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Core.Helpers
{
    public static partial class Action
    {
        public static partial class Result
        {
            /// <summary>
            ///     An action result to simplify exporting CSV files.
            /// </summary>
            /// <typeparam name="T">Type of the data being passed in and processed</typeparam>
            public class CSV<T> : ActionResult where T : class
            {
                private IEnumerable<T> p_ObjectList;
                private PropertyInfo[] p_Properties;
                private string[] p_PropertyHeaders;
                private string p_FileName;

                /// <summary>
                ///     Constructor to create action result
                /// </summary>
                /// <param name="enumerable">An enumerable of the actual data</param>
                /// <param name="file_name">Default name of the downloaded file (user can change in the browser during download)</param>
                /// <param name="properties">An array of property names on the type T...resulting file will have columns in the same order as this array</param>
                /// <param name="columnHeaders">An array of the column names...resulting file will have headers in the same order as this array</param>
                /// <example>
                ///     Example class:
                ///     <code>
                ///         class Example
                ///         {
                ///             public int ID { get; set; }
                ///             public string FirstName { get; set; }
                ///             public string LastName { get; set; }
                ///         }
                ///     </code>
                ///     
                ///     Generate CsvActionResult with the Example class:
                ///     <code>
                ///         return new CsvActionResult&gt;Example&lt;(exampleList, "example.csv", new string[] { "ID", "FirstName", "LastName" }, new string[] { "Identifier", "First Name", "Last Name" });
                ///     </code>
                /// </example>
                public CSV(IEnumerable<T> enumerable, string file_name, string[] properties, string[] columnHeaders)
                {
                    p_ObjectList = enumerable;
                    p_PropertyHeaders = columnHeaders;
                    p_FileName = file_name;

                    p_Properties = new PropertyInfo[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        p_Properties[i] = typeof(T).GetProperty(properties[i]);
                    }
                }

                public override void ExecuteResult(ControllerContext context)
                {
                    var response = context.HttpContext.Response;

                    response.ContentType = "text/csv";
                    response.AddHeader("content-disposition", "attachment; filename=" + p_FileName);

                    response.Write(String.Join(",", p_PropertyHeaders.Select(s => P_ProcessColumnValue(s)).ToArray()) + Environment.NewLine);

                    foreach (T item in p_ObjectList)
                    {
                        string[] values = new string[p_Properties.Length];

                        for (int i = 0; i < p_Properties.Length; i++)
                        {
                            object obj = p_Properties[i].GetValue(item, null);
                            values[i] = obj != null ? obj.ToString() : null;
                        }

                        response.Write(String.Join(",", values.Select(s => P_ProcessColumnValue(s)).ToArray()) + Environment.NewLine);
                    }
                }

                private string P_ProcessColumnValue(string val)
                {
                    return String.Format("\"{0}\"", val != null ? val.Replace("\"", "\"\"") : "");
                }
            }
        }
    }
}