using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.IO;

namespace Core.Helpers
{
    public static partial class DigitalFile
    {
        public class UploadBinder : IModelBinder
        {
            public static void RegisterTypes()
            {
                UploadBinder binder = new UploadBinder();
                ModelBinders.Binders[typeof(DigitalFile.Posted)] = binder;
                ModelBinders.Binders[typeof(DigitalFile.Compressed)] = binder;
                ModelBinders.Binders[typeof(DigitalFile.Image)] = binder;
                ModelBinders.Binders[typeof(DigitalFile.Audio)] = binder;
                ModelBinders.Binders[typeof(DigitalFile.Video)] = binder;
                ModelBinders.Binders[typeof(DigitalFile.Document)] = binder;
                ModelBinders.Binders[typeof(DigitalFile.Data)] = binder;
            }

            public object BindModel(ControllerContext controllerCtx, ModelBindingContext bindingCtx)
            {
                if (controllerCtx == null)
                {
                    throw new ArgumentNullException("controllerCtx");
                }
                if (bindingCtx == null)
                {
                    throw new ArgumentNullException("bindingCtx");
                }

                Type uploadingFileType = typeof(DigitalFile.Posted);
                Type modelType = bindingCtx.ModelType;

                if (modelType.Equals(uploadingFileType) || modelType.IsSubclassOf(uploadingFileType))
                {
                    HttpPostedFileBase file = controllerCtx.HttpContext.Request.Files[bindingCtx.ModelName];

                    return ChooseFileOrNull(file, bindingCtx);
                }

                return null;

            }

            internal static object ChooseFileOrNull(HttpPostedFileBase rawFile, ModelBindingContext bindingCtx)
            {
                HttpPostedFileBase file = rawFile;
                if (rawFile == null)
                {
                    file = null;
                }
                else if (rawFile.ContentLength == 0 && String.IsNullOrEmpty(rawFile.FileName))
                {
                    file = null;
                }

                ConstructorInfo constructor = bindingCtx.ModelType.GetConstructor(new Type[] { typeof(HttpPostedFileBase), typeof(string), typeof(ModelStateDictionary) });
                return constructor.Invoke(new object[] { file, bindingCtx.ModelName, bindingCtx.ModelState });
            }
        }
    }
}