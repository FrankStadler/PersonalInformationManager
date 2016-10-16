namespace PersonalInformationManager.Migrations
{
    using PersonalInformationManager.Models;
    using System.Collections.Generic;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<PersonalInformationManager.Models.PersonalInformationManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "PersonalInformationManager.Models.ShowDb";
        }

        protected override void Seed(PersonalInformationManager.Models.PersonalInformationManagerContext context)
        {
            
            var sources = new List<PersonalInformationManager.Models.Source>
            {
               new PersonalInformationManager.Models.Source { Name="Online Source A" },
               new PersonalInformationManager.Models.Source { Name="Online Source B" },
               new PersonalInformationManager.Models.Source { Name="Online Source C" },
               new PersonalInformationManager.Models.Source { Name="Movie Theater" }
            };
            sources.ForEach(s => context.Sources.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var movies = new List<PersonalInformationManager.Models.Movie>
            {
                new Movie { Title = "Movie A", SourceID = sources.Single(s => s.Name == "Movie Theater").SourceID , ReleaseDate = DateTime.Parse("2017-01-01"), ViewedDate = DateTime.Parse("2017-01-01"), Image = LoadImageFromFile("MovieA.jpg") },
                new Movie { Title = "Movie B", SourceID = sources.Single(s => s.Name == "Movie Theater").SourceID , ReleaseDate = DateTime.Parse("2017-02-01"), ViewedDate = null, Image = LoadImageFromFile("MovieB.jpg") },
                new Movie { Title = "Movie C", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID , ReleaseDate = DateTime.Parse("2017-03-01"), ViewedDate = null, Image = LoadImageFromFile("MovieC.jpg") },
                new Movie { Title = "Movie D", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID , ReleaseDate = DateTime.Parse("2017-04-01"), ViewedDate = null, Image = LoadImageFromFile("MovieD.jpg") },
                new Movie { Title = "Movie E", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID , ReleaseDate = DateTime.Parse("2017-05-01"), ViewedDate = null, Image = LoadImageFromFile("MovieE.jpg") },
                new Movie { Title = "Movie F", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID , ReleaseDate = DateTime.Parse("2017-06-01"), ViewedDate = null, Image = LoadImageFromFile("MovieF.jpg") }
            };
            movies.ForEach(m => context.Movies.AddOrUpdate(p => p.Title, m));
            context.SaveChanges();

            var books = new List<PersonalInformationManager.Models.Book>
            {
                new Book { Title="Book A", Author = "Author A", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID, ReleaseDate = DateTime.Parse("2017-01-01"), ViewedDate = DateTime.Parse("2017-01-01"), Image = LoadImageFromFile("BookA.jpg") },
                new Book { Title="Book B", Author = "Author B", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID, ReleaseDate = DateTime.Parse("2017-02-01"), ViewedDate = null, Image = LoadImageFromFile("BookB.jpg") },
                new Book { Title="Book C", Author = "Author C", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID, ReleaseDate = DateTime.Parse("2017-03-01"), ViewedDate = null, Image = LoadImageFromFile("BookC.jpg") },
                new Book { Title="Book D", Author = "Author D", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID, ReleaseDate = DateTime.Parse("2017-04-01"), ViewedDate = null, Image = LoadImageFromFile("BookD.jpg") },
                new Book { Title="Book E", Author = "Author E", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID, ReleaseDate = DateTime.Parse("2017-05-01"), ViewedDate = null, Image = LoadImageFromFile("BookE.jpg") },
                new Book { Title="Book F", Author = "Author F", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID, ReleaseDate = DateTime.Parse("2017-06-01"), ViewedDate = null, Image = LoadImageFromFile("BookF.jpg") }
            };
            books.ForEach(b => context.Books.AddOrUpdate(p => p.Title, b));
            context.SaveChanges();

            var shows = new List<PersonalInformationManager.Models.Show>
            {
                new Show { Title = "Show A", Season = "1", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID, ReleaseDate = DateTime.Parse("2017-01-01"), ViewedDate = DateTime.Parse("2017-01-01"), Image = LoadImageFromFile("ShowA.jpg") },
                new Show { Title = "Show B", Season = "1", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID, ReleaseDate = DateTime.Parse("2017-02-01"), ViewedDate = null, Image = LoadImageFromFile("ShowB.jpg") },
                new Show { Title = "Show C", Season = "2", SourceID = sources.Single(s => s.Name == "Online Source A").SourceID, ReleaseDate = DateTime.Parse("2017-03-01"), ViewedDate = null, Image = LoadImageFromFile("ShowC.jpg") },
                new Show { Title = "Show D", Season = "1", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID, ReleaseDate = DateTime.Parse("2017-04-01"), ViewedDate = null, Image = LoadImageFromFile("ShowD.jpg") },
                new Show { Title = "Show E", Season = "1", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID, ReleaseDate = DateTime.Parse("2017-05-01"), ViewedDate = null, Image = LoadImageFromFile("ShowE.jpg") },
                new Show { Title = "Show F", Season = "3", SourceID = sources.Single(s => s.Name == "Online Source B").SourceID, ReleaseDate = DateTime.Parse("2017-06-01"), ViewedDate = null, Image = LoadImageFromFile("ShowF.jpg") }
            };
            shows.ForEach(s => context.Shows.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

        }

        private byte[] LoadImageFromFile(string fileName)
        {
            string SeedDataFolder = ConfigurationManager.AppSettings["SeedDataFolder"];
            string FullFilePath = SeedDataFolder + fileName;
            
            if(System.IO.File.Exists(FullFilePath))
            {
                return System.IO.File.ReadAllBytes(FullFilePath);
            } 
            else
            {
                return null;
            }
        }
    }
}
