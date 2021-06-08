// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiClient.Responces
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
            text.AppendLine( $"Статус: { ( IsError ? "Ошибка" : "Без ошибок" ) }" );
            text.AppendLine( $"Скачали фидо: { DownloadedShops }{( DownloadedShops != TotalShops ? $" ({TotalShops}!)" : String.Empty )}" );
            if( ClosedStores.Any() ) {
                text.AppendLine( $"Закрылись: { string.Join( ",", ClosedStores ) }" );
            }

            var totalNew = TotalAfter - TotalBefore;
            var soldOutNew = SoldOutAfter - SoldOutBefore;
            
            text.AppendLine( $"Всего: {TotalAfter}, новых: { totalNew }, всего распроданных: { SoldOutAfter }, распродали: { soldOutNew }" );
            text.AppendLine( $"Разница: { totalNew - soldOutNew }" );
            text.AppendLine( $"Подходящих для пометки 'soldout': { ProductsForDisable }" );
            if( WorkErrors > 0 ) {
                text.AppendLine( $"Количество ошибок во время работы: {WorkErrors}" );
            }

            var t = TimeSpan.FromMilliseconds( Time );
            var time = $"{t.Hours:D2}ч:{t.Minutes:D2}м:{t.Seconds:D2}с:{t.Milliseconds:D3}мс";
            text.AppendLine( $"Общее время работы: {time}" );
            
            return text.ToString();
        }
    }
}