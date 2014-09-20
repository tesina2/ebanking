using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class StudentExpense
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int StudentExpenseID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Company { get; set; }
        public virtual string ExpenseAccountNumber { get; set; }
        public virtual decimal CurrentAmountOwed { get; set; }

        public virtual bool? bIsRecurring { get; set; }
        //TODO: Design system to add in recurring payments

        //Times Missed?
        public virtual bool bMissedPayment { get; set; }

        /** Navigation Variables **/
        [ForeignKey("Student")]
        public virtual int StudentID { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Expense")]
        public virtual int ExpenseID { get; set; }
        public virtual Expense Expense { get; set; }

    }
}