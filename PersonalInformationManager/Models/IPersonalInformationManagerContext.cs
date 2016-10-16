using System;
using System.Data.Entity;

namespace PersonalInformationManager.Models
{
    public interface IPersonalInformationManagerContext : IDisposable
    {
        IDbSet<Show> Shows { get; set; }
        IDbSet<Book> Books { get; set; }
        IDbSet<Movie> Movies { get; set; }
        IDbSet<Source> Sources { get; set; }
        int SaveChanges();
        void SetModified(object entity);
    }
}
