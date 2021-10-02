using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Model
{
    public class BetDetail
    {
        public string Player { get; set; }
        public double? AmountBet { get; set; }
        public double? TotalWon { get; set; }
        public bool IsColor { get; set; }
    }
}
