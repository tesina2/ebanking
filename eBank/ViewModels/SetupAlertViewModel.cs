using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBank.ViewModels
{
    public class SetupAlertViewModel
    {
        public SelectList ListOfAccounts { get; set; }
        public int SelectedAccountId { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal AmountThreshold { get; set; }
    }
}