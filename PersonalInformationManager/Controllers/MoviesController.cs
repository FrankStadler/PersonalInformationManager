using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalInformationManager.Models;
using PagedList;
using System.Configuration;

namespace PersonalInformationManager.Controllers
{
    public class MoviesController : Controller
    {
        private IPersonalInformationManagerContext db;

        public MoviesController()
        {
            db = new PersonalInformationManagerContext();
        }
        public MoviesController(IPersonalInformationManagerContext context)
        {
            db = context;
        }
        public FileContentResult GetMovieImg(int id)
        {

            byte[] byteArray = db.Movies.Find(id).Image;
            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = sortOrder == "title_asc" ? "title_desc" : "title_asc";
            ViewBag.ViewDateSortParm = sortOrder == "view_date_asc" ? "view_date_desc" : "view_date_asc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var movies = from m in db.Movies
                        .Include(m => m.Source)
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()));
                ViewBag.CurrentFilter = searchString;
            }
            else
            {
                ViewBag.CurrentFilter = null;
            }
            if (!String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = sortOrder.ToLower().Trim();
            }

            switch (sortOrder)
            {
                case "title_asc":
                    movies = movies.OrderBy(m => m.Title);
                    break;
                case "title_desc":
                    movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "view_date_asc":
                    movies = movies.OrderBy(m => m.ViewedDate);
                    break;
                case "view_date_desc":
                    movies = movies.OrderByDescending(m => m.ViewedDate);
                    break;
                default:
                    movies = movies.OrderByDescending(m => m.ViewedDate);
                    break;

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(movies.ToPagedList(pageNumber, pageSize));
        }



        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieID,Title,ReleaseDate,ViewedDate,Image,SourceID")] Movie movie, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    movie.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", movie.SourceID);

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            if (movie.Source != null)
            {
                ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", movie.SourceID);
            }
            else
            {
                ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name");
            }

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,Title,ReleaseDate,ViewedDate,Image,SourceID")] Movie movie, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    movie.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                db.SetModified(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", movie.SourceID);
            return View(movie);

        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            PersonalInformationManager.Utilities.Logging.WriteLog(ConfigurationManager.AppSettings["ErrorLogLocation"], filterContext.Exception.ToString());

            // Output a nice error page
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                this.View("Error").ExecuteResult(this.ControllerContext);
            }
        }
    }
}
