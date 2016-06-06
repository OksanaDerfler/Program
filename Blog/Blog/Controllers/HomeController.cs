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
        /// Метод отвечает за запуск главной страницы и возвращающий данные из БД
        /// </summary>
        [HttpGet]    
        public ActionResult Index()
        {

                //Подключаемся к базе данных
                using (Models.dbblog db = new Models.dbblog())
                {
                    //Забираем последние блоги по записям
                    var maxint = db.Records.Max(p => p.Id);
                    var blogRecords = db.Records.Where(p => p.Id > maxint - 3).ToList().OrderByDescending(p => p.Id);
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
                    

        /// <summary>
        /// Метод принимающий данные от пользователя и обрабатывающий их
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Models.NewRecord record)
        {


            if (string.IsNullOrEmpty(record.textarea) || string.IsNullOrEmpty(record.title) || string.IsNullOrEmpty(record.tag))
            {
                return View("Blog/Error");
            }

            if (record.textarea.Length < 10 || record.title.Length < 10 || record.tag.Length<1)
            {
                return View("Blog/ErrorLowLength");
            }

            //Если данные прошли проверку, необходимо записать их в базу

            Models.Record mr = new Models.Record();
            mr.Title = record.title;
            mr.Text = record.textarea;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string login = HttpContext.User.Identity.Name;
                mr.Nick = login;
            }
            else
            {
            mr.Nick = "Anonimous"; }     
            

            if (string.IsNullOrEmpty(record.tag))
            { mr.Tag = "No"; }
            else { mr.Tag = record.tag; }

            mr.Like = 0;
            mr.Dislike = 0;
            mr.DateStart = DateTime.Now;

            int newRecord = 0;
            using (Models.dbblog db = new Models.dbblog())
            {
                newRecord = db.Records.Max(p => p.Id);
                db.Records.Add(mr);
                db.SaveChanges();
            }
            newRecord = newRecord + 1;

            //Сохраняем картинку, если она есть:
            if (!(record.uplfile==null))
            {
               var dirName = Server.MapPath("~");
               record.uplfile.SaveAs(dirName + @"Photo\" + newRecord + ".jpg");
            }

            return View("Blog/AddNotification");
        }

        /// <summary>
        /// Метод на поиск записей по тегам
        /// </summary>
        /// <param name="sear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(Models.Search sear)
        {
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Tag.Contains(sear.search)).ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
            }
            return View("Blog/Search");
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

        /// <summary>
        /// Метод забирает из БД все записи по блогам
        /// </summary>
        /// <returns></returns>
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

        public ActionResult TagRedirect(string id)
        {
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Tag == id).ToList();
                //var blogRecords = db.Records.Where(p => p.Id > 0).ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
            }
            return View("Blog/Records");
        }

        public ActionResult NameRedirect(string id)
        {
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Nick == id).ToList();
                ViewBag.data = blogRecords;
            }
            return View("Blog/Records");
        }

    }
}
