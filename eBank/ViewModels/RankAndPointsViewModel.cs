using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBank.ViewModels
{
    public class RankAndPointsViewModel
    {
        public virtual int Rank { get; set; }
        public virtual int StudentTotal { get; set; }
        public virtual int Points { get; set; }
    }
}