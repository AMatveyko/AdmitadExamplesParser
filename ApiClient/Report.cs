// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiClient
{
    internal sealed class Report
    {
        public bool IsError { get; set; }
        public int TotalShops { get; set; }
        public int DownloadedShops { get; set; }
        public long TotalBefore { get; set; }
        public long TotalAfter { get; set; }
        public long SoldOutBefore { get; set; }
        public long SoldOutAfter { get; set; }
        public long ProductsForDisable { get; set; }
        public List<string> ClosedStores { get; set; } = new();
        public int WorkErrors { get; set; }
        public long Time { get; set; }

        public override string ToString()
        {
            var text = new StringBuilder();
            text.AppendLine( $"Status: { ( IsError ? "Error" : "Ok" ) }" );
            text.AppendLine( $"Downloaded: { DownloadedShops }{( DownloadedShops != TotalShops ? $" ({TotalShops}!)" : String.Empty )}" );
            if( ClosedStores.Any() ) {
                text.AppendLine( $"Closed: { string.Join( ",", ClosedStores ) }" );
            }

            var totalNew = TotalAfter - TotalBefore;
            var soldOutNew = SoldOutAfter - SoldOutBefore;
            
            text.AppendLine( $"Total: {TotalAfter}, new: { totalNew }, soldout total: { SoldOutAfter }, soldout new: { soldOutNew }" );
            text.AppendLine( $"Number of new versus sold out: { totalNew - soldOutNew }" );
            text.AppendLine( $"Suitable for disable: 'Неуспел реализовать'" );
            if( WorkErrors > 0 ) {
                text.AppendLine( $"The number of errors during work: {WorkErrors}" );
            }

            var t = TimeSpan.FromMilliseconds( Time );
            var time = $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
            text.AppendLine( $"Time: {time}" );
            
            return text.ToString();
        }
    }
}