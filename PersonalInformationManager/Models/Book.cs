using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalInformationManager.Models
{
    public class Book
    {
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "View Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ViewedDate { get; set; }

        [DataType(DataType.Upload)]
        public byte[] Image { get; set; }
        [Required]
        [ForeignKey("Source")]
        public int SourceID { get; set; }
        public virtual Source Source { get; set; }
    }
}