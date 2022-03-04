// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.Entities
{
    [ Serializable ]
    [ XmlRoot( "categories" ) ]
    public sealed class ShopCategories
    {
        [ XmlElement( "category" ) ]
        public List<ShopCategory> Categories { get; set; }
    }
}