using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalInformationManager;
using PersonalInformationManager.Controllers;
using PersonalInformationManager.Models;
using PersonalInformationManager.ViewModels;
using Moq;
using FakeDbSet;

namespace PersonalInformationManager.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //private methods to load test case data, used in several tests in this class
        //broken out into individual methods to avoid repeating code in the tests.
        private Source GetTestSource()
        {
            return new Source { Name = "Online A", SourceID = 1 };
        }

        private Book GetTestBook(Source source)
        {
            return new Book { Source = source, Title = "Book A", Author = "Author A", ReleaseDate = DateTime.Parse("2016-01-01"), ViewedDate = DateTime.Parse("2016-01-01") };
        }
        private InMemoryDbSet<Book> GetTestBookSet(Source source)
        {
            return new InMemoryDbSet<Book>
            {
                GetTestBook(source)
            };
        }

        private Movie GetTestMovie(Source source)
        {
            return new Movie { Source = source, Title = "Movie A", ReleaseDate = DateTime.Parse("2016-01-01"), ViewedDate = DateTime.Parse("2016-01-01") };
        }

        private InMemoryDbSet<Movie> GetTestMovieSet(Source source)
        {
            return new InMemoryDbSet<Movie>
            {
                GetTestMovie(source)
            };
        }

        private Show GetTestShow(Source source)
        {
            return new Show { Source = source, SourceID = 1, Title = "TV Show A", Season = "1", ReleaseDate = DateTime.Parse("2016-01-01"), ViewedDate = DateTime.Parse("2016-01-01") };
        }
        private InMemoryDbSet<Show> GetTestShowSet(Source source)
        {
            return new InMemoryDbSet<Show>
            {
                GetTestShow(source)
            };
        }

        private const string BOOK_TITLE = "Book A";
        private const string MOVIE_TITLE = "Movie A";
        private const string SHOW_TITLE = "TV Show A";

        [TestMethod]
        public void Index()
        {
            // Arrange
            Source source = GetTestSource();
            var books = GetTestBookSet(source);
            var movies = GetTestMovieSet(source);
            var shows = GetTestShowSet(source);

            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Books).Returns(books);
            db.Setup(e => e.Movies).Returns(movies);
            db.Setup(e => e.Shows).Returns(shows);
            HomeController controller = new HomeController(db.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var model = result.Model as HomeDashboard;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(BOOK_TITLE, model.book.Title);
            Assert.AreEqual(MOVIE_TITLE, model.movie.Title);
            Assert.AreEqual(SHOW_TITLE, model.show.Title);

        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        
    }
}
