using ApiClient.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class RatingsCalculationRequest : BaseRequest<Context>
    {

        protected override string Controller => "Index";

        protected override string MethodName => "CalculateProductsRatings";

        public RatingsCalculationRequest( RequestSettings settings ) : base( settings, true ) { }
    }
}
