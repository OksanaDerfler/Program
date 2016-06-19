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


            if (string.IsNullOrEmpty(record.textarea) || string.IsNullOrEmpty(record.title) || string.IsNullOrEmpty(record.tag))
            {
                return View("Blog/Error");
            }

            if (record.textarea.Length < 10 || record.title.Length < 10 || record.tag.Length < 1)
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
                mr.Nick = "Anonimous";
            }


            if (string.IsNullOrEmpty(record.tag))
            { mr.Tag = "No"; }
            else { mr.Tag = record.tag; }

            mr.Like = 0;
            mr.Dislike = 0;
            mr.DateStart = DateTime.Now;







            Entities.Record Rec = new Entities.Record();


                if (!(record.uplfile == null))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        record.uplfile.InputStream.CopyTo(ms);
                        mr.Picture = ms.ToArray();

                    }
                }



            Rec.Title=record.title;
            Rec.Text= record.textarea;
            Rec.DateStart = DateTime.Now;
            Rec.Like = 0;
            Rec.Nick=mr.Nick;
            Rec.Tag=mr.Tag;
            Rec.Picture=mr.Picture;


            new DAL.Recordd().Create(Rec);

            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                // newRecord = db.Records.Max(p => p.Id);
                if (!(record.uplfile == null))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        record.uplfile.InputStream.CopyTo(ms);
                        mr.Picture = ms.ToArray();

                    }

                    db.Records.Add(mr);
                    db.SaveChanges();
                }
                else
                {
                    db.Records.Add(mr);
                    db.SaveChanges();
                }
            }
                */

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
            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Tag.Contains(sear.search)).ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
            }
            */
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
            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                //Здесь передадутся данные из базы в форму изменения записи
                //Заменяем данные
                Record rec = db.Records.Find(edrecord.idrec);
                rec.Title = edrecord.title;
                rec.Text = edrecord.textarea;
                rec.Tag = edrecord.tag;

                //Если есть картинка, то тоже заменяем
                if (!(edrecord.uplfile == null))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        edrecord.uplfile.InputStream.CopyTo(ms);
                        rec.Picture = ms.ToArray();
                    }
                }

                db.SaveChanges();
            }
             */

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
            //er.Picture = edrecord.uplfile;
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
            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Id > 0).ToList().OrderByDescending(p => p.Id);
                ViewBag.data = blogRecords;
            }
            */

            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.ToList();

            return View("Blog/Records");
        }

        public ActionResult TagRedirect(string id)
        {
           // using (Models.dbblog db = new Models.dbblog())
           // {
                //Забираем блоги
                //var blogRecords = db.Records.Where(p => p.Tag == id).ToList();
                //var blogRecords = db.Records.Where(p => p.Id > 0).ToList().OrderByDescending(p => p.Id);
           //     ViewBag.data = blogRecords;
            //}

            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.Where(p => p.Tag == id).ToList();

            return View("Blog/Records");
        }

        public ActionResult NameRedirect(string id)
        {
            /*
            using (Models.dbblog db = new Models.dbblog())
            {
                //Забираем блоги
                var blogRecords = db.Records.Where(p => p.Nick == id).ToList();
                ViewBag.data = blogRecords;
            }
            */

            var blogRecords = new DAL.Recordd().GetAll();
            ViewBag.data = blogRecords.Where(p => p.Nick == id).ToList();

            return View("Blog/Records");
        }

        public ActionResult Like(int id)
        {
            using (Models.dbblog db = new Models.dbblog())
            {
            //    //Забираем блоги
                var blogRecords = db.Records.Find(id);
                blogRecords.Like = blogRecords.Like + 1;
                db.SaveChanges();
            }



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
            return View("Blog/Home");
        }

        public ActionResult LikeSearch(int id)
        {
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

            //Забираем фотографии из хранилища
            //var photoPath = ControllerContext.HttpContext.Server.MapPath(@"~/Photo");
            //var photoFilenames = Directory.GetFiles(photoPath);
            //ViewBag.photo = photoFilenames;

            return View("Blog/Home");
        }

        public ActionResult LikeRecords(int id)
        {
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
            return View("Blog/Records");

        }

        public ActionResult OpenProfile(string username)
        {
            return View("Blog/Profile");
        }


    }
}
