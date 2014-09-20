using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class Classroom
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int ClassroomID { get; set; }

        [Display(Name = "Classroom Name")]
        [MaxLength(50)]
        public virtual string ClassroomName { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        [Column("TeacherID")]
        [ForeignKey("Teacher")]
        public virtual int TeacherID { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}