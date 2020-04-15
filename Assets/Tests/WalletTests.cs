namespace Tests
{
    namespace Economy
    {
        using Hermitage.Economy;
        using NUnit.Framework;
        using System.Collections;
        using System.Collections.Generic;
        using System.Linq;
        using UnityEngine;
        using UnityEngine.TestTools;

        public class WalletTests
        {
            [Test]
            public void PayCoin()
            {
                IWallet wallet = new RegularWallet();
                ICoin coin = new CopperCoin();
                wallet.Pay(coin);
                Assert.AreEqual(wallet.Value, coin.Value);
            }

            [Test]
            public void PayCoins()
            {
                IWallet wallet = new RegularWallet();
                ICoin[] coins = 
                {
                    new GoldCoin(),
                    new CopperCoin(),
                    new SilverCoin()
                };
                wallet.Pay(coins);
                Assert.AreEqual(wallet.Value, coins.Sum(coin => coin.Value));
            }

            [Test]
            public void Overpay()
            {
                IWallet wallet = new RegularWallet();
                int startingCapacity = wallet.RemainingCapacity;
                List<ICoin> coins = new List<ICoin>
                {
                    new CopperCoin(),
                    new SilverCoin()
                };
                for (int i = 0; i < startingCapacity; i++)
                {
                    coins.Add(new GoldCoin());
                }
                wallet.Pay(coins.ToArray());
                Assert.AreEqual(wallet.Value, startingCapacity * new GoldCoin().Value);
            }

            // [Test]
            // public void ChargeCoin()
            // {
            //     IWallet wallet = new RegularWallet();

            // }

            // [Test]
            // public void ChargeCoins()
            // {

            // }

            // [Test]
            // public void Overcharge()
            // {

            // }
        }
    }
}
