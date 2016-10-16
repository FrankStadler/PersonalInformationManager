using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalInformationManager;
using PersonalInformationManager.Controllers;
using PersonalInformationManager.Models;
using Moq;
using FakeDbSet;

namespace PersonalInformationManager.Tests.Controllers
{
    [TestClass]
    public class SourcesControllerTest
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

        private const string SOURCE_NAME = "Online A";

        [TestMethod]
        public void Sources_Index()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            SourcesController controller = new SourcesController(db.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Sources_Details()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Sources.Find(It.IsAny<int>())).Returns(GetTestSource());
            SourcesController controller = new SourcesController(db.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Source;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SOURCE_NAME, model.Name);
        }


        [TestMethod]
        public void Sources_Create_Get()
        {
            // Arrange
            var db = new Mock<IPersonalInformationManagerContext>();
            SourcesController controller = new SourcesController(db.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Sources_Create_Post()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SaveChanges()).Returns(1);
            SourcesController controller = new SourcesController(db.Object);
            Source source = GetTestSource();

            // Act
            var result = (RedirectToRouteResult)controller.Create(source);
            
            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void Sources_Edit_Get()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Sources.Find(It.IsAny<int>())).Returns(GetTestSource());
            SourcesController controller = new SourcesController(db.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Source;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SOURCE_NAME, model.Name);
        }

        [TestMethod]
        public void Sources_Edit_Post()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.SaveChanges()).Returns(1);
            db.Setup(e => e.SetModified(It.IsAny<Source>()));
            SourcesController controller = new SourcesController(db.Object);
            Source source = GetTestSource();

            // Act
            var result = (RedirectToRouteResult)controller.Edit(source);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void Sources_Delete_Get()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Sources.Find(It.IsAny<int>())).Returns(GetTestSource());
            SourcesController controller = new SourcesController(db.Object);

            // Act
            ViewResult result = controller.Delete(1) as ViewResult;
            var model = result.Model as PersonalInformationManager.Models.Source;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SOURCE_NAME, model.Name);
        }

        [TestMethod]
        public void Sources_DeleteConfirmed()
        {
            // Arrange
            var sources = GetTestSourceSet();
            var db = new Mock<IPersonalInformationManagerContext>();
            db.Setup(e => e.Sources).Returns(sources);
            db.Setup(e => e.Sources.Find(It.IsAny<int>())).Returns(GetTestSource());
            SourcesController controller = new SourcesController(db.Object);

            // Act
            var result = (RedirectToRouteResult)controller.DeleteConfirmed(1);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
