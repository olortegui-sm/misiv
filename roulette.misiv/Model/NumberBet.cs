using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Model
{
    public class NumberBet
    {
        public NumberBet()
        {
            betDetails = new List<BetDetail>();
        }
        public int Number { get; set; }
        public bool IsWinningNumber { get; set; }
        public List<BetDetail> betDetails { get; set; }
    }
}
