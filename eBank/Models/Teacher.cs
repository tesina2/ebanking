using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class Teacher : User
    {
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}