using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class Expense
    {
        //TODO: Reword data model design between student users and their expenses
        //IDEA: Create a second table where student expenses are creaed and they need to match up to the seed ones.
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int ExpenseID { get; set; }
        public virtual bool? bIsActive { get; set; }
        public virtual string ExpenseAccountNumber { get; set; }
        public virtual string Company { get; set; }

        //The min and max values that the monthly charge of this will be
        public virtual decimal MinAmount { get; set; }
        public virtual decimal MaxAmout { get; set; }
        public virtual decimal LateFee { get; set; }

    }
}