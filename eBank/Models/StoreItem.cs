using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class StoreItem
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public virtual int StoreItemID { get; set; }

        public virtual string StoreItemName { get; set; }

        [ForeignKey("Store")]
        public virtual int StoreID { get; set; }

        public virtual Store Store { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        public virtual decimal ItemPrice { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual int HappinessChange { get; set; }

        public virtual int HealthChange { get; set; }

        public virtual int HungerChange { get; set; }

        public virtual int SocialChange { get; set; }

        public virtual int EnergyChange { get; set; }
    }
}