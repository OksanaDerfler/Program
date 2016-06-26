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
using Blog.Models;
using DAL;
using BLL;

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
            var tagDistinct = new Bll().GetDistinctTags().OrderBy(p=>p);
            ViewBag.tags = tagDistinct;
            var blogRecords = new DAL.Recordd().GetAll();
            var maxint = blogRecords.Max(p => p.Id);
            blogRecords = blogRecords.Where(p => p.Id > maxint - 7).ToList().OrderByDescending(p => p.Id);
            ViewBag.data = blogRecords;

            /*
            //Подключаемся к базе данных
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем последние блоги по записям
                var maxint = db.Records.Max(p => p.Id);
               // var blogRecords = db.Records.Where(p => p.Id > maxint - 3).ToList().OrderByDescending(p => p.Id);
                var blogRecords = db.Records.ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
                var tags = db.Records.Select(p => p.Tag).Distinct().ToList();
                ViewBag.tags = tags;
            }
            */
            //Забираем фотографии из хранилища
            //var photoPath = ControllerContext.HttpContext.Server.MapPath(@"~/Photo");
            //var photoFilenames = Directory.GetFiles(photoPath);
            //ViewBag.photo = photoFilenames;

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
            Entities.Record Rec = new Entities.Record();

            if (string.IsNullOrEmpty(record.textarea) || string.IsNullOrEmpty(record.title) || string.IsNullOrEmpty(record.tag))
            {
                return View("Blog/Error");
            }

            if (record.textarea.Length < 10 || record.title.Length < 10 || record.tag.Length < 1)
            {
                return View("Blog/ErrorLowLength");
            }


            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string login = HttpContext.User.Identity.Name;
                Rec.Nick = login;
            }
            else
            {
                Rec.Nick = "Anonimous";
            }


            if (string.IsNullOrEmpty(record.tag))
            { Rec.Tag = "No"; }
            else { Rec.Tag = record.tag; }


                if (!(record.uplfile == null))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        record.uplfile.InputStream.CopyTo(ms);
                        Rec.Picture = ms.ToArray();

                    }
                }



            Rec.Title=record.title;
            Rec.Text= record.textarea;
            Rec.DateStart = DateTime.Now;
            Rec.Like = 0;


            new DAL.Recordd().Create(Rec);

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

            if (sear.search == null)
            {
                ViewBag.data = null;
                return View("Blog/Search");
            }

            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.Where(p => p.Tag.Contains(sear.search)).ToList().OrderByDescending(p => p.Id);

            return View("Blog/Search");
        }

        [HttpPost]
        public ActionResult Edit(Models.EditRecord edrecord)
        {

            Entities.Record er = new Entities.Record();

            if (!(edrecord.uplfile == null))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    edrecord.uplfile.InputStream.CopyTo(ms);
                    er.Picture = ms.ToArray();
                }
            }

         
            er.Id = edrecord.idrec;
            er.Tag = edrecord.tag;
            er.Text = edrecord.textarea;
            er.Title = edrecord.title;


            var ans = new DAL.Recordd().Update(er);



            return View("Blog/NewEdit");
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
            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.ToList();

            return View("Blog/Records");
        }

        public ActionResult TagRedirect(string id)
        {
            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.Where(p => p.Tag == id).ToList();

            return View("Blog/Records");
        }

        public ActionResult NameRedirect(string id)
        {
            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.Where(p => p.Nick == id).ToList();

            return View("Blog/Records");
        }

        public ActionResult Like(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                new DAL.Recordd().Like(id);
            }

            var tagDistinct = new Bll().GetDistinctTags().OrderBy(p => p);
            ViewBag.tags = tagDistinct;
            var blogRecords = new DAL.Recordd().GetAll();
            var maxint = blogRecords.Max(p => p.Id);
            blogRecords = blogRecords.Where(p => p.Id > maxint - 7).ToList().OrderByDescending(p => p.Id);
            ViewBag.data = blogRecords;

            return View("Blog/Home");            
        }

        public ActionResult LikeSearch(int id)
        {
            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                //    //Забираем блоги
                var blogRecords = db.Records.Find(id);
                blogRecords.Like = blogRecords.Like + 1;
                db.SaveChanges();
            }

            //Подключаемся к базе данных
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем последние блоги по записям
                var maxint = db.Records.Max(p => p.Id);
                // var blogRecords = db.Records.Where(p => p.Id > maxint - 3).ToList().OrderByDescending(p => p.Id);
                var blogRecords = db.Records.ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
                var tags = db.Records.Select(p => p.Tag).Distinct().ToList();
                ViewBag.tags = tags;
            }
            */
            //Забираем фотографии из хранилища
            //var photoPath = ControllerContext.HttpContext.Server.MapPath(@"~/Photo");
            //var photoFilenames = Directory.GetFiles(photoPath);
            //ViewBag.photo = photoFilenames;

            return View("Blog/Home");
        }

        public ActionResult LikeRecords(int id)
        {
            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                //    //Забираем блоги
                var blogRecords = db.Records.Find(id);
                blogRecords.Like = blogRecords.Like + 1;
                db.SaveChanges();
            }

            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Id > 0).ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
            }
             * */
            return View("Blog/Records");
            
        }

        public ActionResult OpenProfile(string username)
        {
            return View("Blog/Profile");
        }


    }
}
