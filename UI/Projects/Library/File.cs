using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Library
{
    public static partial class Utils
    {
        public static partial class File
        {
            public static string UniqueId { get { return DateTime.Now.Ticks.ToString() + new Random().Next(1, 100).ToString(); } }


        }
    }
}