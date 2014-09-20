using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int PurchaseID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        public virtual int StudentID { get; set; }

        //[ForeignKey("StoreItemID")]
        //public virtual StoreItem StoreItem { get; set; }

        //public virtual int StoreItemID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public virtual DateTime PurchaseDate { get; set; }

        public virtual string StoreItemName { get; set; }

        public virtual string StoreName { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        public virtual decimal ItemPrice { get; set; }
    }
}