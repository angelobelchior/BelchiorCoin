using System;

namespace BelchiorCoin.Playground
{
    [Serializable]
    public class Coin
    {
        public float Amount { get; set; }

        public Coin() { }
        public Coin(float amount) 
            => this.Amount = amount;
    }
}
