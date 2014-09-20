using eBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBank.ViewModels
{
    public class ManageStudentViewModel
    {
        public int StudentID { get; set; }

        public SelectList ListOfClassrooms { get; set; }
        public int SelectedClassroomId { get; set; }
    }
}