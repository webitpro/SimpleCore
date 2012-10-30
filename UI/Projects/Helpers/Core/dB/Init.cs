﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Core.Models;
using System.Configuration;

namespace Core.Helpers
{
    public static partial class DB
    {
        /* DB SEEDER / DB INITIALIZER */
        public class Init : DropCreateDatabaseIfModelChanges<DB.Context>
        {
            protected override void Seed(DB.Context context)
            {

                //add account table with default value/account
                List<Account> accounts = new List<Account>();
                accounts.Add(new Account
                {
                    FirstName = "Dragan",
                    LastName = "Rusnov",
                    Email = "drusnov@magnersanborn.com",
                    Password = Security.Password.GenerateHash("drusnov@magnersanborn.com", "Ru$n0v"),
                    Registered = DateTime.Now,
                    SecurityRole = Security.Role.Administrator.ToString(),
                    Status = Enums.Status.Account.Active.ToString()
                });
                /*accounts.Add(new Account
                {
                    FirstName = "Ethan",
                    LastName = "Craft",
                    Email = "ecraft@magnersanborn.com",
                    Password = Security.Password.GenerateHash("ecraft@magnersanborn.com", "masaDev2012"),
                    Registered = DateTime.Now,
                    SecurityRole = Security.Role.Administrator.ToString(),
                    Status = Enums.Status.Account.Active.ToString()
                });
                accounts.Add(new Account
                {
                    FirstName = "Marshall",
                    LastName = "Moore",
                    Email = "mmoore@magnersanborn.com",
                    Password = Security.Password.GenerateHash("mmoore@magnersanborn.com", "masaDev2012"),
                    Registered = DateTime.Now,
                    SecurityRole = Security.Role.Administrator.ToString(),
                    Status = Enums.Status.Account.Active.ToString()
                });*/
                accounts.Add(new Account
                {
                    FirstName = "Jeremy",
                    LastName = "Caldwell",
                    Email = "jcaldwell@magnersanborn.com",
                    Password = Security.Password.GenerateHash("jcaldwell@magnersanborn.com", "masaDev2012"),
                    Registered = DateTime.Now,
                    SecurityRole = Security.Role.Administrator.ToString(),
                    Status = Enums.Status.Account.Active.ToString()
                });
                /*accounts.Add(new Account
                {
                    FirstName = "Areli",
                    LastName = "Nathanson",
                    Email = "anathanson@magnersanborn.com",
                    Password = Security.Password.GenerateHash("anathanson@magnersanborn.com", "masaDev2012"),
                    Registered = DateTime.Now,
                    SecurityRole = Security.Role.Administrator.ToString(),
                    Status = Enums.Status.Account.Active.ToString()
                });
                accounts.Add(new Account
                {
                    FirstName = "Alexa",
                    LastName = "Lohmeyer",
                    Email = "alohmeyer@magnersanborn.com",
                    Password = Security.Password.GenerateHash("alohmeyer@magnersanborn.com", "masaDev2012"),
                    Registered = DateTime.Now,
                    SecurityRole = Security.Role.Administrator.ToString(),
                    Status = Enums.Status.Account.Active.ToString()
                });*/


                foreach (Account acc in accounts)
                {
                    context.Accounts.Add(acc);
                    context.SaveChanges();
                }



            }
        }
    }
}
