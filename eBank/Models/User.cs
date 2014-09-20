using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class User
    {
        [Key]
        [Editable(false)]
        [ForeignKey("UserProfile")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50)]
        public virtual string LastName { get; set; }
    }
}