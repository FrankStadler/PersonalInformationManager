using System.Data.Entity;

namespace PersonalInformationManager.Models
{
    public class PersonalInformationManagerContext : DbContext, IPersonalInformationManagerContext
    {
        public PersonalInformationManagerContext() : base("PersonalInformationManagerContext") { }
        public IDbSet<Show> Shows { get; set; }
        public IDbSet<Book> Books { get; set; }
        public IDbSet<Movie> Movies { get; set; }
        public IDbSet<Source> Sources { get; set; }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}