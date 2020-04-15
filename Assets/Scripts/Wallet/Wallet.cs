namespace Hermitage
{
    namespace Economy
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;

        public abstract class Wallet : IWallet
        {
            private List<ICoin> coins;

            protected abstract int Capacity { get; }

            public event EventHandler OnCoinAdded;
            public event EventHandler OnCoinRemoved;
            
            public bool IsFull => RemainingCapacity <= 0;
            public int Value => coins.Sum(c => c.Value);
            public int RemainingCapacity => Capacity - coins.Count;

            protected Wallet()
            {
                coins = new List<ICoin>();
            }

            public bool CanAfford(int cost) => cost <= Value;

            public ICoin[] Charge(int cost)
            {
                List<ICoin> payments = new List<ICoin>();

                foreach (ICoin coin in coins.OrderByDescending(c => c.Value).ToList())
                {
                    if (coin.Value > cost - payments.Sum(c => c.Value))
                    {
                        continue;
                    }

                    payments.Add(coin);
                }

                OnCoinRemoved?.Invoke(this, EventArgs.Empty);

                return payments.ToArray();
            }

            public void Pay(params ICoin[] payments)
            {
                payments = payments.OrderByDescending(coin => coin.Value).Take(RemainingCapacity).ToArray();

                coins.AddRange(payments);

                OnCoinAdded?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}