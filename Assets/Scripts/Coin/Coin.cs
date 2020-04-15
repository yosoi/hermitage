namespace Hermitage
{
    namespace Economy
    {
        public abstract class Coin : ICoin
        {
            public abstract int Value { get; }
        }
    }
}