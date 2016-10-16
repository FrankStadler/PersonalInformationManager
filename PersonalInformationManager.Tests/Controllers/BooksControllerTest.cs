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
    public class BooksControllerTest
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

        private Book GetTestBook(Source source)
        {
            return new Book { Source = source, Title = "Book A", Author = "Author A", ReleaseDate = DateTime.Parse("2016-01-01") };
        }

        private InMemoryDbSet<Book> GetTestBookSet(Source source)
        {
            return new InMemoryDbSet<Book>
            {
                GetTestBook(source)
            };
        }

        private const string BOOK_TITLE = "Book A";

        [TestMethod]
        public void Books_Index()
        {
            // Arrange
            Source source = GetTestSource();
            var books = GetTestBookSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Books).Returns(books);
            BooksController controller = new BooksController(db.Object);

            // Act
            ViewResult result = controller.Index("", "", "", null) as ViewResult;
            var model = result.Model as PagedList.IPagedList<PersonalInformationManager.Models.Book>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Count());
        }

        [TestMethod]
        public void Books_Details()
        {
            // Arrange
            Source source = GetTestSource();
            var books = GetTestBookSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Books).Returns(books);
            db.Setup(e => e.Books.Find(It.IsAny<int>())).Returns(GetTestBook(source));
            BooksController controller = new BooksController(db.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(BOOK_TITLE, model.Title);
        }

        [TestMethod]
        public void Books_Create_Get()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            BooksController controller = new BooksController(db.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Books_Create_Post()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var books = new InMemoryDbSet<Book> { };
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SaveChanges()).Returns(1);
            db.Setup(e => e.Books).Returns(books);
            BooksController controller = new BooksController(db.Object);
            Source source = GetTestSource();
            Book book = GetTestBook(source);

            // Act
            var result = (RedirectToRouteResult)controller.Create(book, null);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Shows_Edit_Get()
        {
            // Arrange
            Source source = GetTestSource();
            var sources = GetTestSourceSet();
            var books = new InMemoryDbSet<Book> { };
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Books).Returns(books);
            db.Setup(e => e.Books.Find(It.IsAny<int>())).Returns(GetTestBook(source));
            BooksController controller = new BooksController(db.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(BOOK_TITLE, model.Title);
        }

        [TestMethod]
        public void Books_Edit_Post()
        {
            // Arrange
            Source source = GetTestSource();
            var sources = GetTestSourceSet();
            Book book = GetTestBook(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SetModified(It.IsAny<Book>()));
            db.Setup(e => e.SaveChanges()).Returns(1);
            BooksController controller = new BooksController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.Edit(book, null);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }


        [TestMethod]
        public void Books_Delete_Get()
        {
            // Arrange
            Source source = GetTestSource();
            var books = GetTestBookSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Books).Returns(books);
            db.Setup(e => e.Books.Find(It.IsAny<int>())).Returns(GetTestBook(source));
            BooksController controller = new BooksController(db.Object);

            // Act
            ViewResult result = controller.Delete(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(BOOK_TITLE, model.Title);
        }

        [TestMethod]
        public void Books_DeleteConfirmed()
        {
            // Arrange
            Source source = GetTestSource();
            var books = GetTestBookSet(source);
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Books).Returns(books);
            db.Setup(e => e.Books.Find(It.IsAny<int>())).Returns(GetTestBook(source));
            db.Setup(e => e.SaveChanges()).Returns(1);
            BooksController controller = new BooksController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.DeleteConfirmed(1);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }
    }
}
