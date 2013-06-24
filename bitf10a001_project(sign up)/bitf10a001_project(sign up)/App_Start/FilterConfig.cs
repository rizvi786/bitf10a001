using System.Web;
using System.Web.Mvc;

namespace bitf10a001_project_sign_up_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}