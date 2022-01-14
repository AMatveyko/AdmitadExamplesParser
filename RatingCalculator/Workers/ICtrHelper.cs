using Common.Api;

namespace RatingCalculator.Workers
{
    internal interface ICtrHelper
    {
        int GetCtr(string itemId);
        void ChangeContext(BackgroundBaseContext context);
    }
}
