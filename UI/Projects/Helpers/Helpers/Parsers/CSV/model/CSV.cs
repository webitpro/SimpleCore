using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers
{
    public static partial class Parser
    {
        public static partial class Model
        {
            public class CSV
            {
                public string Name { get; set; }
                public string Street { get; set; }
                public string City { get; set; }
                public string State { get; set; }
                public string ZipCode { get; set; }
                public string Latitude { get; set; }
                public string Longitude { get; set; }

                public CSV()
                {
                }
            }
        }
    }
}
