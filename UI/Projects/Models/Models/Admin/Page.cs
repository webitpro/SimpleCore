using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Page")]
    public class Page
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Template is required")]
        public string Template { get; set; }

        [Required]
        public bool IsHomePage { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public virtual ICollection<StaticPage> StaticPages { get; set; }

        public virtual ICollection<CustomPage> CustomPages { get; set; }

        public virtual ICollection<Accordion> Accordions { get; set; }


       
    }
}
