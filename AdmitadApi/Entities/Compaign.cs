// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

namespace AdmitadApi.Entities
{
    public sealed class Compaign
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string NameAliases { get; set; }
        public string Currency { get; set; }
        public decimal Rating { get; set; }
        public decimal ECPC { get; set; }
        public decimal EPC { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<AdmitadFeedInfo> Feeds { get; set; }
        public bool IsConnected { get; set; }
    }
}