using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HelpDeskData;


namespace HelpDesk
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public class HttpDataContextDataStore : IDataRepositoryStore
        {
            public object this[string key]
            {
                get { return HttpContext.Current.Items[key]; }
                set { HttpContext.Current.Items[key] = value; }
            }
        }
        protected void Application_Start()
        {
            DataRepositoryStore.CurrentDataStore = new HttpDataContextDataStore();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {

            HelpDeskData.CurrentDataContext.CloseCurrentRepository();

        }
    }
}
