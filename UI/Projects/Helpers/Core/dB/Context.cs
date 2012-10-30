using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Core;
using System.Data.Entity;
using Core.Models;


namespace Core.Helpers
{
    public static partial class DB
    {
        public partial class Context : DbContext
        {
            public Context()
                : base(Config.ActiveConfiguration.Database)
            {

            }

            public DbSet<Account> Accounts { get; set; }
            public DbSet<Tab> Tabs { get; set; }
            public DbSet<Section> Sections { get; set; }
            public DbSet<Link> Links { get; set; }
            public DbSet<Page> Pages { get; set; }
            public DbSet<PageComponent> PageComponents { get; set; }

            //templates
            public DbSet<StaticPage> StaticPages { get; set; }
            public DbSet<CustomPage> CustomPages { get; set; }
            public DbSet<Accordion> Accordions { get; set; }

            //page components
            public DbSet<Intro> Intros { get; set; }
        }
    }
}

   