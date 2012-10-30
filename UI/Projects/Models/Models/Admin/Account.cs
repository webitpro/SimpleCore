using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Core;
using Core.Library;
using Core.Models;

namespace Core.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "First Name must be under 50 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Numeric,
            xOperator = false,
            ErrorMessage = "First Name cannot be numeric value")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Last Name must be under 50 characters")]
        [ExpressionType(xType = Core.Library.Type.xPression.Numeric,
            xOperator = false,
            ErrorMessage = "Last Name cannot be numeric value")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(250)]
        [StringLength(250, ErrorMessage = "Email must be under 250 characters")]
        [EmailFormat(ErrorMessage = "Email format is not valid")]
        public string Email { get; set; }
        
        [MaxLength(100)]        
        public string Password { get; set; }

        public DateTime Registered { get; set; }
        
        [Required(ErrorMessage = "Security Role is required")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Security Role must be under 50 characters")]
        public string SecurityRole { get; set; }

        [Required]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Status must be under 50 characters")]
        public string Status { get; set; }
   

    }
}