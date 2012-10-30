using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Library;
using Core.Models;

namespace Core.Helpers
{
    public static partial class Security
    {
        public class AuthenticatedUser
        {
            public enum Info
            {
                ID,
                Name,
                Role
            }
            //PROPERTIES
            public int ID { get; set; }
            public string Name { get; set; }
            public Security.Role Role { get; set; }

            #region constructors
            private AuthenticatedUser()
            {
                Role = Security.Role.Administrator;
            }

            private AuthenticatedUser(int personID, string name, Security.Role secRole)
            {
                ID = personID;
                Name = name;
                Role = secRole;
            }
            #endregion

            //METHODS
            public override string ToString()
            {
                return String.Format("{0}\n{1}\n{2}", ID, Name, Role.ToString());
            }

            public static AuthenticatedUser FromString(string data)
            {
                string[] arr = data.Split('\n');
                AuthenticatedUser user = new AuthenticatedUser();
                user.ID = Convert.ToInt32(arr[0]);
                user.Name = arr[1];
                user.Role = GetSecurityRoleById(user.ID);

                return user;
            }

            public static AuthenticatedUser FindUser(string email, string hashedPassword)
            {
                DB.Context db = new DB.Context();
                string activeStatus = Enums.Status.Account.Active.ToString();
                Account acc = db.Accounts.SingleOrDefault(x => x.Email.Equals(email) && x.Password == hashedPassword && x.Status.Equals(activeStatus));
                if (acc != null)
                {
                    return new AuthenticatedUser(acc.Id, String.Format("{0} {1}", acc.FirstName, acc.LastName), (Security.Role)Enum.Parse(typeof(Security.Role), acc.SecurityRole));
                }
                return null;

            }

            public static Security.Role GetSecurityRoleById(int id)
            {
                DB.Context db = new DB.Context();
                Account acc = db.Accounts.SingleOrDefault(x => x.Id == id);
                if (acc != null)
                {
                    return (Security.Role)Enum.Parse(typeof(Security.Role), acc.SecurityRole);
                }

                return Security.Role.User;
            }

        }
    }
}