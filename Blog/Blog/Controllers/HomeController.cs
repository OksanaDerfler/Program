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
        [HttpGet]    
        public ActionResult Index()
        {
            //Подключаемся к базе данных
            using (Models.dbblog db = new Models.dbblog())
            {
            //Забираем последние блоги по записям
            var maxint = db.Records.Max(p => p.Id);
            var blogRecords = db.Records.Where(p => p.Id > maxint-3).ToList().OrderByDescending(p => p.Id);
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

        [HttpPost]
        public ActionResult Index(Models.NewRecord record)
        {
            if (string.IsNullOrEmpty(record.textarea) || string.IsNullOrEmpty(record.title))
            {
                return View("Blog/Error");
            }

            if (record.textarea.Length < 10 || record.title.Length < 10)
            {
                return View("Blog/ErrorLowLength");
            }

            //Если данные прошли проверку, необходимо записать их в базу

            Models.Record mr = new Models.Record();
            mr.Title = record.title;
            mr.Text = record.textarea;

            if (string.IsNullOrEmpty(record.nick))
            { mr.Nick = "Anonimous"; }
            else { mr.Nick = record.nick;}
            if (string.IsNullOrEmpty(record.tag))
            { mr.Tag = "No"; }
            else { mr.Tag = record.tag; }
                        
            mr.Like = 0;
            mr.Dislike = 0;
            mr.DateStart = DateTime.Now;

            using (Models.dbblog db = new Models.dbblog())
            {
                db.Records.Add(mr);
                db.SaveChanges();
            }

            return View("Blog/AddNotification");  
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
            return View("Blog/Rules");
        }
        public ActionResult Records()
        {
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Id > 0).ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
            }
            return View("Blog/Records");
        }

    }
}
