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
    public class MoviesControllerTest
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

        private Movie GetTestMovie(Source source)
        {
            return new Movie { Source = source, Title = "Movie A", ReleaseDate = DateTime.Parse("2016-01-01") };
        }

        private InMemoryDbSet<Movie> GetTestMovieSet(Source source)
        {
            return new InMemoryDbSet<Movie>
            {
                GetTestMovie(source)
            };
        }

        private const string MOVIE_TITLE = "Movie A";

        [TestMethod]
        public void Movies_Index()
        {
            // Arrange
            Source source = GetTestSource();
            var movies = GetTestMovieSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Movies).Returns(movies);
            MoviesController controller = new MoviesController(db.Object);

            // Act
            ViewResult result = controller.Index("", "", "", null) as ViewResult;
            var model = result.Model as PagedList.IPagedList<PersonalInformationManager.Models.Movie>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Count());
        }

        [TestMethod]
        public void Movies_Details()
        {
            // Arrange
            Source source = GetTestSource();
            var movies = GetTestMovieSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Movies).Returns(movies);
            db.Setup(e => e.Movies.Find(It.IsAny<int>())).Returns(GetTestMovie(source));
            MoviesController controller = new MoviesController(db.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Movie;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MOVIE_TITLE, model.Title);
        }

        [TestMethod]
        public void Movies_Create_Get()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            MoviesController controller = new MoviesController(db.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Movies_Create_Post()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var movies = new InMemoryDbSet<Movie> { };
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SaveChanges()).Returns(1);
            db.Setup(e => e.Movies).Returns(movies);
            MoviesController controller = new MoviesController(db.Object);
            Source source = GetTestSource();
            Movie movie = GetTestMovie(source);

            // Act
            var result = (RedirectToRouteResult)controller.Create(movie, null);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Movies_Edit_Get()
        {
            // Arrange
            Source source = GetTestSource();
            var sources = GetTestSourceSet();
            var movies = new InMemoryDbSet<Movie> { };
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Movies).Returns(movies);
            db.Setup(e => e.Movies.Find(It.IsAny<int>())).Returns(GetTestMovie(source));
            MoviesController controller = new MoviesController(db.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Movie;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MOVIE_TITLE, model.Title);
        }

        [TestMethod]
        public void Movies_Edit_Post()
        {
            // Arrange
            Source source = GetTestSource();
            var sources = GetTestSourceSet();
            Movie movie = GetTestMovie(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SetModified(It.IsAny<Movie>()));
            db.Setup(e => e.SaveChanges()).Returns(1);
            MoviesController controller = new MoviesController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.Edit(movie, null);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }


        [TestMethod]
        public void Movies_Delete_Get()
        {
            // Arrange
            Source source = GetTestSource();
            var movies = GetTestMovieSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Movies).Returns(movies);
            db.Setup(e => e.Movies.Find(It.IsAny<int>())).Returns(GetTestMovie(source));
            MoviesController controller = new MoviesController(db.Object);

            // Act
            ViewResult result = controller.Delete(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Movie;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(MOVIE_TITLE, model.Title);
        }

        [TestMethod]
        public void Books_DeleteConfirmed()
        {
            // Arrange
            Source source = GetTestSource();
            var movies = GetTestMovieSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Movies).Returns(movies);
            db.Setup(e => e.Movies.Find(It.IsAny<int>())).Returns(GetTestMovie(source));
            db.Setup(e => e.SaveChanges()).Returns(1);
            MoviesController controller = new MoviesController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.DeleteConfirmed(1);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }
    }
}
