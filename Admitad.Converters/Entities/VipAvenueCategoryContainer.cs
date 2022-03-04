// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class VipAvenueCategoryContainer : BaseCategoryPropertiesContainer
    {
        
        private static Dictionary<string, ShopCategoryProperties> _properties =
            new () {
                { "1019", new ShopCategoryProperties( ProductType.Bags ) }, // Сумки
                { "1148", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Child ) }, // Одежда для девочек
                { "1150", new ShopCategoryProperties( ProductType.Footwear, Gender.Woman, Age.Child ) }, // Обувь для девочек
                { "1151", new ShopCategoryProperties( ProductType.Footwear, Gender.Man, Age.Child ) }, // Обувь для мальчиков
                { "1152", new ShopCategoryProperties( ProductType.Clothing, Gender.Man, Age.Child ) }, // Одежда для мальчиков
                { "1238", new ShopCategoryProperties( ProductType.Jewelry ) }, // Ювелирные украшения
                { "861", new ShopCategoryProperties( ProductType.Footwear, age: Age.Adult ) }, // Обувь
                { "1003", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Топы
                { "1015", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Юбки
                { "1126", new ShopCategoryProperties( ProductType.Clothing, Gender.Man, Age.Adult ) }, // Смокинги
                { "1128", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Боди
                { "914", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Туники
                { "929", new ShopCategoryProperties( ProductType.Clothing, Gender.Man, Age.Adult ) }, // Чиносы
                { "921", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Клеш
                { "866", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Кожанные
                { "923", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Леггинсы
                { "918", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Широкие
                { "858", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Спортивные леггинсы
                { "812", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Спортивные шорты
                { "1127", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Спортивные топы
                { "894", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Пончо и накидки
                { "1125", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Бойфренды
                { "911", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Скинни
                { "946", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Клеш
                { "945", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Высокая посадка
                { "948", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Широкие
                { "949", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Комбинезоны
                { "960", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Платья
                { "974", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Парео
                { "976", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Пляжная одежда -> Платья
                { "980", new ShopCategoryProperties( ProductType.Clothing, Gender.Man, Age.Adult ) }, // Поло
                
                { "910", new ShopCategoryProperties( ProductType.Clothing, age: Age.Adult ) } // Одежда
            }; 
        
        public VipAvenueCategoryContainer(  )
            : base( _properties ) { }
    }
}