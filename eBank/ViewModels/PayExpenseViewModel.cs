using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBank.ViewModels
{
    public class PayExpenseViewModel
    {
        public SelectList ListOfAccounts { get; set; }
        public int SelectedAccountId { get; set; }
        public int StudentExpenseId { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal ExpenseAmount { get; set; }
    }
}