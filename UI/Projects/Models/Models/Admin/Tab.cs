using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.Models
{
    [Table("Tab")]
    public class Tab
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

        [Required]
        [MaxLength(100)]
        public string UrlTitle { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public int Position { get; set; }

        public virtual ICollection<Section> Sections { get; set; }

        public int? PageId { get; set; }

        public virtual Page Page { get; set; }

    }
}
