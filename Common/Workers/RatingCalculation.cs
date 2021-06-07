// a.snegovoy@gmail.com

namespace Common.Workers
{
    public sealed class RatingCalculation
    {
        private readonly int _shopWeight;

        public RatingCalculation( int shopWeight ) =>
            _shopWeight = shopWeight;

        public int Calculate() => _shopWeight;
    }
}