namespace Hermitage
{
    namespace Economy
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using UnityEngine;

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

                    coins.Remove(coin);
                    payments.Add(coin);
                }

                int remainingCost = cost - payments.Sum(c => c.Value);
                ICoin payment = coins.OrderBy(c => c.Value).FirstOrDefault(c => c.Value >= remainingCost);
                coins.Remove(payment);
                payments.Add(payment);

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