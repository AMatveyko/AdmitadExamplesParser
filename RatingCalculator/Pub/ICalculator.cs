using Common.Api;
using Common.Entities.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingCalculator.Pub
{
    public interface ICalculator
    {
        ProductRating GetRating(ProductRatingData data);
        List<ProductRating> GetRatings(List<ProductRatingData> datas);
        void ChangeContext(BackgroundBaseContext context);
    }
}
