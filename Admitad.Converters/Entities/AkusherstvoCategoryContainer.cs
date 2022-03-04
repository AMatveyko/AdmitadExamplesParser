// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class AkusherstvoCategoryContainer : BaseCategoryPropertiesContainer
    {
	    private static Dictionary<string, ShopCategoryProperties> _properties = new() {
		    { "6", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "9", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "27", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "28", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "40", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "91", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "102", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "109", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "132", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "133", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "150", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "171", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "174", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "180", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "181", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "190", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "193", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "210", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "222", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "234", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "238", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "253", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "280", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "295", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "329", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "334", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "346", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "367", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "368", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "369", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "385", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "541", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "547", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "553", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "557", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "559", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "565", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "572", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "577", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "597", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "620", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "626", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "632", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "633", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "635", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "637", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "640", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "642", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) },
		    { "643", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "648", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) },
		    { "653", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) }

	    };

	    public AkusherstvoCategoryContainer()
		    : base( _properties ) { }
    }
}