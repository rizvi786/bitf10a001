using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace bitf10a001_project_sign_up_.Models
{
    public class Accounts: IAccounts
    {
        public void save(acc acc)
        {
            WebSecurity.CreateUserAndAccount(acc.UserName, acc.pass, new { name = acc.name, mobile = acc.mobile });
        }
    }
}