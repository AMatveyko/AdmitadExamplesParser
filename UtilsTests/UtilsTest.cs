using System;

using NUnit.Framework;

namespace UtilsTests
{
    public class UtilsTest
    {
        [ Test ]
        public void CompareStrings()
        {
            var string1 = "https://assets.adidas.com/images/w_1080,h_1080,f_auto,q_auto:sensitive,fl_lossy/"; //work
            var string2 = "https://assets.adidas.com/images/w_1080,h_1080,f_auto,q_auto:sensitive,fl_lossy/"; //--
            Assert.AreEqual( string1, string2 );
        }
    }
}