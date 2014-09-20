using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class Store
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public virtual int StoreID { get; set; }

        public virtual string StoreName { get; set; }

        public virtual int EnergyCost { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        public virtual decimal TravelCost { get; set; }

        public virtual ICollection<StoreItem> StoreItems { get; set; }
    }
}