using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PersonalInformationManager.Models;
using PersonalInformationManager.ViewModels;
using System.Configuration;

namespace PersonalInformationManager.Controllers
{
    public class HomeController : Controller
    {
        private IPersonalInformationManagerContext db;

        public HomeController()
        {
            db = new PersonalInformationManagerContext();
        }
        public HomeController(IPersonalInformationManagerContext context)
        {
            db = context;
        }
        public ActionResult Index()
        {
            //load the most recently viewed Book, Movie, and TV Show
            //and display them on the home page
            HomeDashboard dashboard = new HomeDashboard();
            Book book = (from b in db.Books
                        .Include(b => b.Source)
                        where b.ViewedDate != null
                        orderby b.ViewedDate descending
                        select b).FirstOrDefault();

            Movie movie = (from m in db.Movies
                        .Include(m => m.Source)
                           where m.ViewedDate != null
                           orderby m.ViewedDate descending
                           select m).FirstOrDefault();

            Show show = (from s in db.Shows
                        .Include(s => s.Source)
                         where s.ViewedDate != null
                         orderby s.ViewedDate descending
                         select s).FirstOrDefault();

            dashboard.movie = movie;
            dashboard.book = book;
            dashboard.show = show;

            return View(dashboard);
        }

        public ActionResult About()
        {
            ViewBag.Message = "FS Personal Information Manager application description page.";

            return View();
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