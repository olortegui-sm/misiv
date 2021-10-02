using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Model
{
    public class Roulette
    {
        public string Id { get; set; }

        public bool IsOpen { get; set; } = false;

        public DateTime? Open { get; set; }

        public DateTime? Close { get; set; }
        public List<NumberBet> numberBets { get; set; }

        public Roulette()
        {
            this.Start();
        }
        private void Start()
        {
            numberBets = new List<NumberBet>();
            for (int i = 0; i < 37; i++)
            {
                numberBets.Add(new NumberBet()
                {
                    Number = i
                });
            }
        }
    }
}
