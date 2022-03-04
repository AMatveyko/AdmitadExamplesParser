// a.snegovoy@gmail.com

namespace Common.Entities.Rating
{
    public sealed class ItemCtrInfo
    {

        public ItemCtrInfo(
            int id,
            string productId,
            int views,
            int clicks ) =>
            ( Id, ProductId, Views, Clicks ) = ( id, productId, views, clicks );
        
        public string ProductId { get; }
        public int Views { get; }
        public int Clicks { get; }
        public int Id { get; }
    }
}