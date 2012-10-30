using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("CustomPage")]
    public class CustomPage
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Controller is required")]
        [MaxLength(50, ErrorMessage = "Controller name cannot exceed 50 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Alpha, xOperator = true, ErrorMessage = "Controller must be alphabetic characters only")]
        public string Controller { get; set; }

        [Required(ErrorMessage = "Action is required")]
        [MaxLength(50, ErrorMessage = "Action name cannot exceed 50 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Alpha, xOperator = true, ErrorMessage = "Action must be alphabetic characters only")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Page relationship is required")]
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }

    }
}
