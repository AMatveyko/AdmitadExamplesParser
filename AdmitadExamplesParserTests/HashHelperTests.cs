// a.snegovoy@gmail.com

using AdmitadCommon.Helpers;



using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public sealed class HashHelperTests
    {
        [ Test ]
        public void OfferIdTest()
        {
            const string shopName = "lamoda.ru";
            const string offerId = "AR035LWMSX03NS00";
            const string actual = "b4d019aa313dc77af2b7cf7dd0cb4d48";
            var id = HashHelper.GetMd5Hash( shopName, offerId );
            Assert.AreEqual( id, actual );
        }
    }
}