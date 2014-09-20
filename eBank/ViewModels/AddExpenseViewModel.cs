using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBank.ViewModels
{
    public class AddExpenseViewModel
    {
        public string Name { get; set; }
        public string ExpenseAccountNumber { get; set; }
        //TODO: Add recurring payment functionality here
    }
}