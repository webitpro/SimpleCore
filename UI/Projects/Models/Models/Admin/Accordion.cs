using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Accordion")]
    public class Accordion
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [MaxLength(250, ErrorMessage = "Question cannot exceed 250 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Numeric, xOperator = false, ErrorMessage = "Question cannot be numeric value only")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Answer is required")]  
        [ExpressionType(xType = Core.Library.Type.xPression.Numeric, xOperator = false, ErrorMessage = "Answer cannot be numeric value only")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [ExpressionType(xType = Core.Library.Type.xPression.Numeric, xOperator = true, ErrorMessage = "Position must be numeric value")]
        public int Position { get; set; }

        [Required(ErrorMessage = "Page relationship is required")]
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }

    }
}
