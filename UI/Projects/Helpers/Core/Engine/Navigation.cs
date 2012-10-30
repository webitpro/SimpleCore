using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers
{
    public static partial class Engine
    {
        public static class Navigation
        {
            public static class Tab
            {
                public static Core.Models.Tab Info(int id)
                {
                    IRepository<Core.Models.Tab> repo = new DB.Repository<Core.Models.Tab>(new DB.Context());
                    return repo.RetrieveById(id);

                }

                public static string IdBySection(int id)
                {
                    IRepository<Core.Models.Section> repo = new DB.Repository<Core.Models.Section>(new DB.Context());
                    return repo.RetrieveById(id).TabId.ToString();
                }
            }

            public static class Section
            {
                public static Core.Models.Section Info(int id)
                {
                    IRepository<Core.Models.Section> repo = new DB.Repository<Core.Models.Section>(new DB.Context());
                    return repo.RetrieveById(id);
                }
            }
        }
    }
}
