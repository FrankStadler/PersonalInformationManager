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
    public class BooksController : Controller
    {
        private IPersonalInformationManagerContext db;

        public BooksController()
        {
            db = new PersonalInformationManagerContext();
        }
        public BooksController(IPersonalInformationManagerContext context)
        {
            db = context;
        }
        public FileContentResult GetBookImg(int id)
        {

            byte[] byteArray = db.Books.Find(id).Image;
            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }

        // GET: Books
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

            var books = from b in db.Books
                        .Include(b => b.Source)
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.ToUpper().Contains(searchString.ToUpper()));
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
                    books = books.OrderBy(b => b.Title);
                    break;
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "view_date_asc":
                    books = books.OrderBy(b => b.ViewedDate);
                    break;
                case "view_date_desc":
                    books = books.OrderByDescending(b => b.ViewedDate);
                    break;
                default:
                    books = books.OrderByDescending(b => b.ViewedDate);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,Title,Author,ReleaseDate,ViewedDate,Image,SourceID")] Book book, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    book.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", book.SourceID);

            return View(book);


        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            if (book.Source != null)
            {
                ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", book.SourceID);
            }
            else
            {
                ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name");
            }

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,Author,ReleaseDate,ViewedDate,Image,SourceID")] Book book, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    book.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                db.SetModified(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", book.SourceID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
