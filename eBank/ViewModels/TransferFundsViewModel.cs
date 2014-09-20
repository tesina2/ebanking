using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBank.ViewModels
{
    public class TransferFundsViewModel
    {
        public SelectList ListOfSourceAccounts { get; set; }
        public int SelectedSourceAccountId { get; set; }

        public SelectList ListOfDestinationAccounts { get; set; }
        public int SelectedDestinationAccountId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TransferAmount { get; set; }
    }
}