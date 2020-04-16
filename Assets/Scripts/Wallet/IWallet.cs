namespace Hermitage
{
    namespace Economy
    {
        using System;
        using System.Collections.Generic;
        
        public interface IWallet
        {
            event EventHandler OnCoinAdded;
            event EventHandler OnCoinRemoved;
            bool IsFull { get; }
            int Value { get; }
            int RemainingCapacity { get; }
            bool CanAfford(int cost);
            ICoin[] Charge(int cost);
            void Pay(params ICoin[] payments);
        }
    }
}