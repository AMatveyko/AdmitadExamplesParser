// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public interface IBaseOffer
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Url { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public string Model { get; set; }
        public string TypePrefix { get; set; }
        public string MarketCategory { get; set; }
        public string CategoryPath { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public Currency Currency { get; set; }
        //public bool Enable { get; set; }
        int ShopId { get; set; }
        bool Delivery { get; set; }
        //List<string> AllParams { get; set; }
        string SalesNotes { get; set; }
    }
}