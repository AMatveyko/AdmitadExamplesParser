// a.snegovoy@gmail.com

using System;
using System.Security.Cryptography;
using System.Text;

namespace AdmitadCommon.Helpers
{
    public static class HashHelper
    {
        
        
        
        public static string GetMd5Hash( params string[] inputs )
        {
            const string format = "x2";
            var input = string.Join( string.Empty, inputs );
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var sBuilder = new StringBuilder();
            for ( var i = 0; i < data.Length; i++ ) {
                sBuilder.Append( data[i].ToString( format ) );
            }
            return sBuilder.ToString();
        }
    }
}