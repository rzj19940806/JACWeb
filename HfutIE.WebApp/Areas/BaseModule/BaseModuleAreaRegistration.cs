﻿using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.CommonModule
{
    public class BaseModuleeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BaseModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "HfutIE.WebApp.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
