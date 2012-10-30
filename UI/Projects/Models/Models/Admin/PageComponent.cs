using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("PageComponent")]
    public class PageComponent
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Component Type is required")]
        public string ComponentType { get; set; }

        [Required(ErrorMessage = "Component Id is required")]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = "Page relationship is required")]
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }
    }
}
