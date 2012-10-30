using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Helpers
{
    public static partial class Api
    {
        public partial class Response
        {
            public bool Success { get; set; }
            public List<Error.Message> Errors { get; set; }
            public object Result { get; set; }

            public Response()
            {
                Success = false;
                Result = null;
                Errors = new List<Error.Message>() { };
            }

            public void setErrors(ICollection<ModelState> dic)
            {
                List<Error.Message> errorList = new List<Error.Message>();
                foreach (ModelState item in dic)
                {
                    if (item.Errors.Count() > 0)
                    {
                        errorList.Add(new Error.Message { Id = "", Text = item.Errors.First().ErrorMessage });
                    }
                }
                Errors = errorList;
            }


        }
    }
}