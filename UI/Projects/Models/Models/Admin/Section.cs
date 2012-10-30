using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.Models
{
    [Table("Section")]
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Alpha, xOperator = true, ErrorMessage = "Title must be alphabetic characters only")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Url Title is required")]
        [MaxLength(100, ErrorMessage = "Url Title cannot exceed 100 characters")]
        public string UrlTitle { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public int Position { get; set; }

        [Required(ErrorMessage = "Tab relationship is required")]
        public int TabId { get; set; }

        [ForeignKey("TabId")]
        public virtual Tab Tab { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public int? PageId { get; set; }

        public virtual Page Page { get; set; }
    }
}
