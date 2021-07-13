// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class LassieCategoryContainer : BaseCategoryPropertiesContainer
    {
        public LassieCategoryContainer()
            : base( _properties ) { }

        private static Dictionary<string, ShopCategoryProperties> _properties = new() {
            { "18", new ShopCategoryProperties( ProductType.Clothing ) }, // Одежда из флиса
            { "351", new ShopCategoryProperties( ProductType.Clothing, Gender.Man, Age.Child ) }, // Для мальчиков
            { "352", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Child ) }, // Для девочек
            { "353", new ShopCategoryProperties( ProductType.Clothing, Gender.Unisex, Age.Baby ) }, // Новорожденные
            { "354", new ShopCategoryProperties( ProductType.Footwear ) }, // Обувь
            { "420", new ShopCategoryProperties( ProductType.Clothing ) }, // Верхняя одежда
            { "425", new ShopCategoryProperties( ProductType.Clothing ) }, // Брюки
            { "429", new ShopCategoryProperties( ProductType.Clothing ) }, // Комбинезоны
            { "433", new ShopCategoryProperties( ProductType.Clothing ) }, // Комплекты
            { "86", new ShopCategoryProperties( ProductType.Clothing, Gender.Man, Age.Child ) }, // Lassie для мальчиков
            { "91", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Child ) }, // Lassie для девочек
            { "31", new ShopCategoryProperties( ProductType.Clothing, Gender.Unisex, Age.Baby ) } // Для новорожденных
        };
    }
}