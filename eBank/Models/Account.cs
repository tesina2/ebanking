using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBank.Models
{
    public class Account
    {
        [Key]
        [Editable(false)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int AccountID { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50)]
        public virtual string AccountName { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        public virtual decimal AccountTotal { get; set; }

        [ForeignKey("Student")]
        public virtual int StudentID { get; set; }

        public virtual Student Student { get; set; }

    }

    public class AccountManager
    {
        public void TransferMoney(Account srcAccount, Account destAccount, decimal transferAmount)
        {
            if (srcAccount.AccountTotal >= transferAmount && transferAmount >= 0)
            {
                srcAccount.AccountTotal -= transferAmount;
                destAccount.AccountTotal += transferAmount;
            }
        }

        //TODO: Add functionality to handle the eSTATEMENT
        public void PayExpense(Account srcAccount, StudentExpense studentExpense, Student stud, bool bIsLate)
        {
            if (srcAccount.AccountTotal >= studentExpense.CurrentAmountOwed && (studentExpense.CurrentAmountOwed > 0))
            {
                if (bIsLate == true)
                {
                    srcAccount.AccountTotal -= studentExpense.CurrentAmountOwed;
                    srcAccount.AccountTotal -= studentExpense.Expense.LateFee;
                    studentExpense.CurrentAmountOwed = 0;
                    stud.bBillsPaid = true;
                }
                else
                {
                    srcAccount.AccountTotal -= studentExpense.CurrentAmountOwed;
                    studentExpense.CurrentAmountOwed = 0;
                    stud.bBillsPaid = true;
                }
            }
        }
    }
}
