using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

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
            Models.dbblog db = new Models.dbblog();
            var blogRecords = db.Records.FirstOrDefault();
            ViewBag.text = blogRecords.Text;
           
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

        public ActionResult Rules()
        {
           // ViewBag.Message = "Your contact page.";

            return View("Blog/Rules");
        }
    }
}
