using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

using HelpDeskData;

namespace HelpDeskData
{
    public partial class User
    {
        public static User GetByName(string name)
        {
            return CurrentDataContext.CurrentContext.Users.FirstOrDefault(x =>x.Nama == name);
        }

        public static User GetByEmail(string Email)
        {
            return CurrentDataContext.CurrentContext.Users.FirstOrDefault(x => x.Email == Email);
        }

        public static IQueryable<User> GetByRole(int Role)
        {
            return CurrentDataContext.CurrentContext.Users.Where(x => x.Role == Role && !x.IsDelete);
        }
        public static User GetByID(string ID)
        {
            return CurrentDataContext.CurrentContext.Users.FirstOrDefault(x => x.ID == ID);
        }

        public void Insert(string by)
        {
            ExtentionTransaction.Inserted(this);
        }
        public void Update(string by)
        {
            ExtentionTransaction.Updated(this);
        }

        public static IQueryable<User> GetAll()
        {
            return CurrentDataContext.CurrentContext.Users.Where(x => !x.IsDelete);
        }


    }
   
}
