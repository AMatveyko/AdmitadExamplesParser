// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class FarfetchCategoryProperties : BaseCategoryPropertiesContainer
    {
        private static readonly Dictionary<string, ShopCategoryProperties> _properties = new() {
            {
                "136659",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Топы для мальчиков (2-12 лет)
            {
                "136660",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Шорты для мальчиков (2-12 лет)
            {
                "136661",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Брюки для мальчиков (2-12 лет)
            {
                "136662",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Деним для мальчиков (2-12 лет)
            {
                "136663",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Куртки для мальчиков (2-12 лет)
            {
                "136664",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Пальто для мальчиков (2-12 лет)
            {
                "136665",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Костюмы для мальчиков (2-12 лет)
            {
                "136667",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Пляжная одежда для мальчиков (2-12 лет)
            {
                "136668",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Нижнее белье для мальчиков (2-12 лет)
            {
                "136677",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Головные уборы для мальчиков (2-12 лет)
            {
                "136678",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Шарфы для мальчиков (2-12 лет)
            {
                "136679",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Перчатки и варежки для мальчиков (2-12 лет)
            {
                "136681",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Ремни и подтяжки для мальчиков (2-12 лет)
            {
                "136682",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Галстуки для мальчиков (2-12 лет)
            {
                "136718",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Полотенца и халаты для мальчиков (2-12 лет)
            {
                "136719",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Спортивная одежда для мальчиков (2-12 лет)
            {
                "136722",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Платья для девочек (2-12 лет)
            {
                "136723",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Юбки для девочек (2-12 лет)
            {
                "136724",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Топы для девочек (2-12 лет)
            {
                "136725",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Шорты для девочек (2-12 лет)
            {
                "136726",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Брюки для девочек (2-12 лет)
            {
                "136727",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Деним для девочек (2-12 лет)
            {
                "136728",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Комбинезоны для девочек (2-12 лет)
            {
                "136729",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Спортивная одежда для девочек (2-12 лет)
            {
                "136730",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Куртки для девочек (2-12 лет)
            {
                "136731",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Пальто для девочек (2-12 лет)
            {
                "136732",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Пляжная одежда для девочек (2-12 лет)
            {
                "136733",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Нижнее белье для девочек (2-12 лет)
            {
                "136739",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Головные уборы для девочек (2-12 лет)
            {
                "136740",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Шарфы для девочек (2-12 лет)
            {
                "136741",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Перчатки и митенки для девочек (2-12 лет)
            {
                "136742",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Аксессуары для волос для девочек (2-12 лет)
            {
                "136743",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Сумки для девочек (2-12 лет)
            {
                "136745",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Ремни и подтяжки для девочек (2-12 лет)
            {
                "136791",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Полотенца и халаты для девочек (2-12 лет)
            {
                "136793",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Одежда для мальчиков (0-36 мес.)
            {
                "136794",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Топы для мальчиков (0-36 мес.)
            {
                "136795",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Шорты для мальчиков (0-36 мес.)
            {
                "136796",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Брюки для мальчиков (0-36 мес.)
            {
                "136797",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Спортивная одежда для мальчиков (0-36 мес.)
            {
                "136798",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Куртки для мальчиков (0-36 мес.)
            {
                "136799",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Пальто для мальчиков (0-36 мес.)
            {
                "136800",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Пинетки для мальчиков (0-36 мес.)
            {
                "136801",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Обувь для мальчиков (0-36 мес.)
            {
                "136802",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Сапоги для мальчиков (0-36 мес.)
            {
                "136803",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Кроссовки для мальчиков (0-36 мес.)
            {
                "136804",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Слиперы для мальчиков (0-36 мес.)
            {
                "136805",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Сандалии для мальчиков (0-36 мес.)
            {
                "136811",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Бутылочки и пустышки для мальчиков (0-36 мес.)
            {
                "136813",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Нагрудники для мальчиков (0-36 мес.)
            {
                "136814",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Пеленальные сумки для мальчиков (0-36 мес.)
            {
                "136815",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Одеяла и конверты для мальчиков (0-36 мес.)
            {
                "136816",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Головные уборы для мальчиков (0-36 мес.)
            {
                "136821",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Перчатки и митенки для мальчиков (0-36 мес.)
            {
                "136822",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Ремни и подтяжки для мальчиков (0-36 мес.)
            {
                "136859",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Пляжная одежда для мальчиков (0-36 мес.)
            {
                "136864",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Одежда для девочек (0-36 мес.)
            {
                "136865",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Топы для девочек (0-36 мес.)
            {
                "136866",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Платья для девочек (0-36 мес.)
            {
                "136867",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Юбки для девочек (0-36 мес.)
            {
                "136868",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Шорты для девочек (0-36 мес.)
            {
                "136869",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Брюки для девочек (0-36 мес.)
            {
                "136870",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Спортивная одежда для девочек (0-36 мес.)
            {
                "136871",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Куртки для девочек (0-36 мес.)
            {
                "136872",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Пальто для девочек (0-36 мес.)
            {
                "136873",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Пляжная одежда для девочек (0-36 мес.)
            {
                "136894",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Ободки для волос для девочек (0-36 мес.)
            {
                "136897",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Головные уборы для девочек (0-36 мес.)
            {
                "136903",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Ремни и подтяжки для девочек (0-36 мес.)
            {
                "136994",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Топы для мальчиков (13-16 лет)
            {
                "136995",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Брюки для мальчиков (13-16 лет)
            {
                "136996",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Шорты для мальчиков (13-16 лет)
            {
                "136997",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Деним для мальчиков (13-16 лет)
            {
                "136998",
                new ShopCategoryProperties( ProductType.Clothing, age: Age.Child, range: new AgeRange( 156, 192 ) )
            }, // Спортивная одежда (13-16 лет)
            {
                "136999",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Пальто для мальчиков (13-16 лет)
            {
                "137000",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Куртки для мальчиков (13-16 лет)
            {
                "137001",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Костюмы для мальчиков (13-16 лет)
            {
                "137002",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Плавки для мальчиков (13-16 лет)
            {
                "137003",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Нижнее белье для мальчиков (13-16 лет)
            {
                "137004",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Головные уборы для мальчиков (13-16 лет)
            {
                "137005",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Шарфы для мальчиков (13-16 лет)
            {
                "137007",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Сумки для мальчиков (13-16 лет)
            {
                "137008",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Ремни для мальчиков (13-16 лет)
            {
                "137009",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Галстуки для мальчиков (13-16 лет)
            {
                "137044",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Платья для девочек (13-16 лет)
            {
                "137045",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Юбки для девочек (13-16 лет)
            {
                "137046",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Топы для девочек (13-16 лет)
            {
                "137047",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Брюки для девочек (13-16 лет)
            {
                "137048",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Шорты для девочек (13-16 лет)
            {
                "137049",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Деним для девочек (13-16 лет)
            {
                "137050",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Комбинезоны для девочек (13-16 лет)
            {
                "137051",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Спортивная одежда для девочек (13-16 лет)
            {
                "137052",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Куртки для девочек (13-16 лет)
            {
                "137053",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Пальто для девочек (13-16 лет)
            {
                "137054",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Купальники для девочек (13-16 лет)
            {
                "137055",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Нижнее белье для девочек (13-16 лет)
            {
                "137056",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Головные уборы для девочек (13-16 лет)
            {
                "137057",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Шарфы для девочек (13-16 лет)
            {
                "137063",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Ремни для девочек (13-16 лет)
            {
                "137185",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Шарфы для мальчиков (0-36 мес.)
            {
                "137186",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Галстуки и бабочки для мальчиков (0-36 мес.)
            {
                "137187",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Шарфы для девочек (0-36 мес.)
            {
                "139312",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 0, 36 ) )
            }, // Носки для мальчиков (0-36 мес.)
            {
                "139313",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Колготки и носки для девочек (0-36 мес.)
            {
                "139321",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Носки и колготки для девочек (2-12 лет)
            {
                "139322",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Носки для мальчиков (2-12 лет)
            {
                "139375",
                new ShopCategoryProperties(
                    ProductType.Clothing,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Халаты для мальчиков (13-16 лет)
            {
                "139360",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Аксессуары для обуви для девочек (13-16 лет)
            {
                "137065",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Сапоги для девочек (13-16 лет)
            {
                "137066",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Кроссовки для девочек (13-16 лет)
            {
                "137067",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Лоферы для девочек (13-16 лет)
            {
                "137068",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Балетки для девочек (13-16 лет)
            {
                "137158",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Сандалии для мальчиков (13-16 лет)
            {
                "137159",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 156, 192 ) )
            }, // Сандалии для девочек (13-16 лет)
            {
                "137012",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Кроссовки для мальчиков (13-16 лет)
            {
                "137013",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Лоферы для мальчиков (13-16 лет)
            {
                "137014",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Броги для мальчиков (13-16 лет)
            {
                "137015",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 156, 192 ) )
            }, // Сапоги для мальчиков (13-16 лет)
            {
                "136670",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Кроссовки для мальчиков (2-12 лет)
            {
                "136671",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Босоножки для мальчиков (2-12 лет)
            {
                "136672",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Сапоги для мальчиков (2-12 лет)
            {
                "136673",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Топсайдеры для мальчиков (2-12 лет)
            {
                "136674",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Лоферы и мокасины для мальчиков (2-12 лет)
            {
                "136675",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Броги для мальчиков (2-12 лет)
            {
                "136676",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Man,
                    range: new AgeRange( 24, 144 ) )
            }, // Слиперы для мальчиков (2-12 лет)
            {
                "136734",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Сапоги для девочек (2-12 лет)
            {
                "136735",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Кроссовки для девочек (2-12 лет)
            {
                "136736",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Балетки для девочек (2-12 лет)
            {
                "136737",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Босоножки для девочек (2-12 лет)
            {
                "136738",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 24, 144 ) )
            }, // Слиперы для девочек (2-12 лет)
            {
                "136874",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Пинетки для девочек (0-36 мес.)
            {
                "136875",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Пинетки для девочек (0-36 мес.)
            {
                "136876",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Сапоги для девочек (0-36 мес.)
            {
                "136877",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Кроссовки для девочек (0-36 мес.)
            {
                "136878",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Балетки для девочек (0-36 мес.)
            {
                "136879",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            }, // Слиперы для девочек (0-36 мес.)
            {
                "136880",
                new ShopCategoryProperties(
                    ProductType.Footwear,
                    age: Age.Child,
                    gender: Gender.Woman,
                    range: new AgeRange( 0, 36 ) )
            } // Босоножки для девочек (0-36 мес.)
        };

        public FarfetchCategoryProperties()
            : base( _properties ) { }
    }
}