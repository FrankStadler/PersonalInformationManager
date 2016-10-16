using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalInformationManager.Controllers;
using PersonalInformationManager.Models;
using Moq;
using FakeDbSet;


namespace PersonalInformationManager.Tests.Controllers
{
    [TestClass]
    public class ShowsControllerTest
    {
        //private methods to load test case data, used in several tests in this class
        //broken out into individual methods to avoid repeating code in the tests.
        private Source GetTestSource()
        {
            return new Source { Name = "Online A", SourceID = 1 };
        }

        private InMemoryDbSet<Source> GetTestSourceSet()
        {
            return new InMemoryDbSet<Source>
            {
                new Source { Name = "Online A", SourceID=1 },
                new Source { Name = "Online B", SourceID=2 },
                new Source { Name = "Online C", SourceID=3 }
            };
        }

        private Show GetTestShow(Source source)
        {
            return new Show { Source = source, SourceID = 1, Title = "TV Show A", Season = "1", ReleaseDate = DateTime.Parse("2016-01-01") };
        }

        private InMemoryDbSet<Show> GetTestShowSet(Source source)
        {
            return new InMemoryDbSet<Show>
            {
                GetTestShow(source)
            };
        }

        private const string SHOW_TITLE = "TV Show A";


        [TestMethod]
        public void Shows_IndexDataTable()
        {
            // Arrange
            Source source = GetTestSource();
            var shows = GetTestShowSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Shows).Returns(shows);

            ShowsController controller = new ShowsController(db.Object);

            // Act
            ViewResult result = controller.IndexDataTable() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Shows_Index()
        {
            // Arrange
            Source source = GetTestSource();
            var shows = GetTestShowSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Shows).Returns(shows);
            ShowsController controller = new ShowsController(db.Object);

            // Act
            ViewResult result = controller.Index("","","", null) as ViewResult;
            var model = result.Model as PagedList.IPagedList<PersonalInformationManager.Models.Show>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void Shows_Details()
        {
            // Arrange
            Source source = GetTestSource();
            var shows = GetTestShowSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Shows).Returns(shows);
            db.Setup(e => e.Shows.Find(It.IsAny<int>())).Returns(GetTestShow(source));
            ShowsController controller = new ShowsController(db.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Show;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SHOW_TITLE, model.Title);
        }

        [TestMethod]
        public void Shows_Create_Get()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            ShowsController controller = new ShowsController(db.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Shows_Create_Post()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var shows = new InMemoryDbSet<Show> { };
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SaveChanges()).Returns(1);
            db.Setup(e => e.Shows).Returns(shows);
            ShowsController controller = new ShowsController(db.Object);
            Source source = GetTestSource();
            Show show = GetTestShow(source);

            // Act
            var result = (RedirectToRouteResult)controller.Create(show, null);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Shows_Edit_Get()
        {
            // Arrange
            Source source = GetTestSource();
            var sources = GetTestSourceSet();
            var shows = new InMemoryDbSet<Show> { };
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Shows).Returns(shows);
            db.Setup(e => e.Shows.Find(It.IsAny<int>())).Returns(GetTestShow(source));
            ShowsController controller = new ShowsController(db.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Show;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SHOW_TITLE, model.Title);
        }

        [TestMethod]
        public void Shows_Edit_Post()
        {
            // Arrange
            Source source = GetTestSource();
            var sources = GetTestSourceSet();
            Show show = GetTestShow(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SetModified(It.IsAny<Show>()));
            db.Setup(e => e.SaveChanges()).Returns(1);
            ShowsController controller = new ShowsController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.Edit(show, null);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void Shows_Delete_Get()
        {
            // Arrange
            Source source = GetTestSource();
            var shows = GetTestShowSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Shows).Returns(shows);
            db.Setup(e => e.Shows.Find(It.IsAny<int>())).Returns(GetTestShow(source));
            ShowsController controller = new ShowsController(db.Object);

            // Act
            ViewResult result = controller.Delete(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Show;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SHOW_TITLE, model.Title);
        }

        [TestMethod]
        public void Shows_DeleteConfirmed()
        {
            // Arrange
            Source source = GetTestSource();
            var shows = GetTestShowSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Shows).Returns(shows);
            db.Setup(e => e.Shows.Find(It.IsAny<int>())).Returns(GetTestShow(source));
            db.Setup(e => e.SaveChanges()).Returns(1);
            ShowsController controller = new ShowsController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.DeleteConfirmed(1);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

    }
}
