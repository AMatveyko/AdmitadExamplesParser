// a.snegovoy@gmail.com

using System;
using System.Linq;

using AdmitadExamplesParser.Entities;

namespace AdmitadExamplesParser.Workers
{
    internal static class ExtendedStatistics {

        // public static void AddStatisticsForParsing(
        //     ShopData shopData,
        //     Action<string> adding )
        // {
        //     adding( "Extended statistics" );
        //     adding( $"Is group id matches if url is the same: { CheckGroupId( shopData ) }" );
        //     var isGroupIdDifferentProduct = IsOneGroupIdDifferentProduct(
        //         shopData,
        //         out var allIds,
        //         out var uniqueIds,
        //         out var isGroupIdZero );
        //     adding( $"Does one groupId belong to different product: { isGroupIdDifferentProduct }" );
        //     if( isGroupIdDifferentProduct ) {
        //         var line = isGroupIdZero
        //             ? "GroupId in file not found"
        //             : $"All groupId count: {allIds}, unique groupId count: {uniqueIds}";
        //         adding( line );
        //     }
        // }

        // private static bool IsOneGroupIdDifferentProduct(
        //     ShopData shopData,
        //     out int allIdsCount,
        //     out int uniquIdsCount,
        //     out bool isGroupIdZero ) {
        //     var allIds = shopData.Offers.Select( p => p.OfferId ).ToList();
        //     allIdsCount = allIds.Count;
        //     uniquIdsCount = allIds.Distinct().Count();
        //     // var isDifferent = groupIds.Count != distinctIds.Count;
        //     // if( isDifferent ) {
        //     //     // int groupId = 0;
        //     //     // foreach( var id in groupIds ) {
        //     //     //     if( groupIds.Count( i => i == id ) > 1 ) {
        //     //     //         groupId = id;
        //     //     //         break;
        //     //     //     }
        //     //     // }
        //     //     //
        //     //     // var pair = shopData.Offers
        //     //     //      .Where( p => p.Value.First().GroupId == groupId ).ToList();
        //     // }
        //     isGroupIdZero = uniquIdsCount == 1 && allIds.First() == 0;
        //     return allIdsCount != uniquIdsCount;
        // }
        
        // private static bool CheckGroupId(
        //     ShopData shopData )
        // {
        //     foreach( var shopDataOffer in shopData.Offers ) {
        //         var groupIds = shopDataOffer.Value.Select( o => o.GroupId ).Distinct();
        //         if( groupIds.Count() != 1 ) {
        //             return false;
        //         }
        //     }
        //
        //     return true;
        // }
        
    }
}