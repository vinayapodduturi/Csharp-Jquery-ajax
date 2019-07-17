using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace YoutubeDashboard
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
  
}
