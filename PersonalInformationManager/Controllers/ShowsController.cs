using System;
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
    public class ShowsController : Controller
    {
        private IPersonalInformationManagerContext db;

        public ShowsController()
        {
            db = new PersonalInformationManagerContext();
        }
        public ShowsController(IPersonalInformationManagerContext context)
        {
            db = context;
        }
        public FileContentResult GetShowImg(int id)
        {

            byte[] byteArray = db.Shows.Find(id).Image;
            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }

        // GET: Sources
        public ActionResult IndexDataTable()
        {
            var shows = from s in db.Shows
                        .Include(s => s.Source)
                        select s;

            return View(shows.ToList());
        }

        // GET: Shows
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

            var shows = from s in db.Shows
                        .Include(s => s.Source)
                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                shows = shows.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
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
                    shows = shows.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    shows = shows.OrderByDescending(s => s.Title);
                    break;
                case "view_date_asc":
                    shows = shows.OrderBy(s => s.ViewedDate);
                    break;
                case "view_date_desc":
                    shows = shows.OrderByDescending(s => s.ViewedDate);
                    break;
                default:
                    shows = shows.OrderByDescending(s => s.ViewedDate);
                    break;

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(shows.ToPagedList(pageNumber, pageSize));
        }

        // GET: Shows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Shows.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // GET: Shows/Create
        public ActionResult Create()
        {
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name");

            return View();
        }

        // POST: Shows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShowId,Title,Season,SourceID,ReleaseDate,ViewedDate")] Show show, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    show.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                db.Shows.Add(show);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", show.Source.SourceID );

            return View(show);
        }

        // GET: Shows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Shows.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            if(show.Source != null)
            {
                ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", show.Source.SourceID);
            }
            else
            {
                ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name");
            }
            
            return View(show);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShowId,Title,Season,SourceID,ReleaseDate,ViewedDate,Image")] Show show, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    show.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                db.SetModified(show);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SourceID = new SelectList(db.Sources, "SourceID", "Name", show.Source.SourceID);
            return View(show);
        }

        // GET: Shows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Shows.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Show show = db.Shows.Find(id);
            db.Shows.Remove(show);
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
