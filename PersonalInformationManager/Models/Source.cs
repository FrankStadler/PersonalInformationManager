using System.ComponentModel.DataAnnotations;

namespace PersonalInformationManager.Models
{
    public class Source
    {
        [Key]
        public int SourceID { get; set; }

        [StringLength(50)]
        [Display(Name ="Name")]
        public string Name { get; set; }
    }
}