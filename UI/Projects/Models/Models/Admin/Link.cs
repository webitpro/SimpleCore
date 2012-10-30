using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.Models
{
    [Table("Link")]
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Title length cannot exceed 50 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Alpha, xOperator = true, ErrorMessage = "Title must be alphabetic characters only")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Url Title is required")]
        [MaxLength(100, ErrorMessage = "Url Title cannot exceed 100 characters")]
        public string UrlTitle { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public int Position { get; set; }

        [Required(ErrorMessage = "Section relationship is required")]
        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        [Required(ErrorMessage = "Page relationship is required")]
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }
    }
}
