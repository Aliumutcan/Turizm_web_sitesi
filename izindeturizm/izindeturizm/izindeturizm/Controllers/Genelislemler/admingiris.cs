using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace izindeturizm.Controllers.Genelislemler
{
    public class admingiris: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["admin"] != null)
            {
                
            }
            else
            {
                filterContext.Result = new RedirectResult("/admin-giris");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}