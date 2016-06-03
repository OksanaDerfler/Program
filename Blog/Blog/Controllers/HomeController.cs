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
using System.IO;

namespace Blog.Controllers
{
    public class HomeController : Controller
        {
        /// <summary>
        /// Метод отвечает за запуск главной страницы
        /// </summary>
        public ActionResult Index()
        {
            //Подключаемся к базе данных
            using (Models.dbblog db = new Models.dbblog())
            {
            //Забираем блог записи
            var blogRecords = db.Records.Where(p => p.Id < 4).ToList();
            ViewBag.data = blogRecords;
            var tags = db.Records.Select(p => p.Tag).Distinct().ToList();
            ViewBag.tags = tags;
            }

            //Забираем фотографии из хранилища
            var photoPath = ControllerContext.HttpContext.Server.MapPath(@"~/Photo");
            var photoFilenames = Directory.GetFiles(photoPath);            
            ViewBag.photo = photoFilenames;

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
