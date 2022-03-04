// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class DochkiSinochkiCategoryContainer : BaseCategoryPropertiesContainer
    {

        private static Dictionary<string, ShopCategoryProperties> _properties =
            new () {
                { "1274", new ShopCategoryProperties( ProductType.Toys ) }, //Игрушки
                { "1327", new ShopCategoryProperties( ProductType.Toys ) },  //Спорт и игры на улице
                { "19912", new ShopCategoryProperties( ProductType.Footwear, age: Age.Child ) }, //Обувь
                { "23313", new ShopCategoryProperties( ProductType.Clothing, age: Age.Child ) }, //Одежда 
                { "1354", new ShopCategoryProperties( ProductType.Toys ) }, //Хобби и творчество
                { "1382", new ShopCategoryProperties( ProductType.HouseholdGoods ) }, //Бытовая химия и хозтовары
                { "1666", new ShopCategoryProperties( ProductType.Food ) }, //Питание для мам
                { "1713997", new ShopCategoryProperties( ProductType.Bags ) }, //Сумки, рюкзаки
                { "1912", new ShopCategoryProperties( ProductType.Food ) }, //Питание и кормление
                { "1714111", new ShopCategoryProperties( ProductType.Furniture ) }, //Декор и освещение
                { "1360", new ShopCategoryProperties( ProductType.HygieneAndCare, age: Age.Baby ) }, //Подгузники и гигиена
                { "2307121", new ShopCategoryProperties( ProductType.HygieneAndCare, age: Age.Adult, gender: Gender.Woman ) }, //Красота и уход
                { "2307115", new ShopCategoryProperties( ProductType.HygieneAndCare, age: Age.Adult, gender: Gender.Woman ) }, //Средства личной гигиены
                { "1555", new ShopCategoryProperties( ProductType.Clothing, Gender.Woman, Age.Adult ) }, // Одежда для мам
                { "1714015", new ShopCategoryProperties( ProductType.Footwear, Gender.Woman, Age.Adult ) }, // Обувь для мам
                { "2351743", new ShopCategoryProperties( ProductType.Textile ) }, // Текстиль
                { "2838805", new ShopCategoryProperties( ProductType.Appliances ) }, //Мелкая бытовая техника
                { "297455", new ShopCategoryProperties( ProductType.Animals ) }, //Товары для собак
                { "297467", new ShopCategoryProperties( ProductType.Animals ) }, //Товары для кошек
                { "334778", new ShopCategoryProperties( ProductType.Animals ) }, //Товары для грызунов
                { "334781", new ShopCategoryProperties( ProductType.Animals ) }, //Товары для птиц
                { "400496", new ShopCategoryProperties( ProductType.Animals ) }, //Товары для рыб
                { "577607", new ShopCategoryProperties( ProductType.Furniture ) }, //Ванна и туалет
            }; 
        
        public DochkiSinochkiCategoryContainer()
            : base( _properties ) { }
    }
}