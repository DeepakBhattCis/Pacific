using PacificBBExtranet.Services.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.Controllers
{
    public class BaseController : Controller
    {

        public BaseController(){}

        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

    }
}