// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public interface IExtendedOffer
    {
        List<Param> Params { get; set; }
        Gender Gender { get; set; }
        Age Age { get; set; }
        int CategoryId { get; set; }
        int BrandId { get; set; }
        string VendorNameClearly { get; set; }
        int CountryId { get; set; }

        void AddParamIfNeed(
            RawParam raw );
    }
}