using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
        {
        /// <summary>
        /// Метод отвечает за запуск главной страницы
        /// </summary>
        public ActionResult Index()
        {
           // ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View("Blog/Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
