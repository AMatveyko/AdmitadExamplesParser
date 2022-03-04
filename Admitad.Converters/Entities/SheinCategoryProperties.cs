// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class SheinCategoryProperties : BaseCategoryPropertiesContainer
    {
        private static readonly Dictionary<string, ShopCategoryProperties> Properties = new() {
            {
                "a7dd246c3c", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Деним для малышей &gt;
            {
                "de4586fc41", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Рубашки поло для мальчиков &gt;
            {
                "115bfab552", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Комплекты для мальчиков &gt;
            {
                "89961ea22b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Футболки и майки для девочек &gt; Футболки для девочек
            {
                "78661231cb", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Спортивные штаны для маленьких девочек &gt;
            {
                "2d2cc46a10", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсы для девочек
            {
                "b2248f805f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Шорты для девочек &gt;
            {
                "9a16683c5b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Футболки и майки для мальчиков &gt; Футболки для мальчиков
            {
                "8868499fd1", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Блузки для девочек &gt;
            {
                "7e10eea3a8", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Домашняя одежда для девочек &gt;
            {
                "e3500d8cf4", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Деним для мальчиков &gt; Джинсы для мальчиков
            {
                "fc4207d3d7", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Футболки для мальчиков &gt; Футболки для мальчиков
            {
                "83a6ba6d14", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Нижнее белье для девочек &gt; Бюстгальтеры и бралетты для девочек
            {
                "5a6741e612", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Комплекты для девочек &gt;
            {
                "0966ca189a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсы для девочек
            {
                "a05768d17a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Комплект из двух предметов для девочек &gt;
            {
                "cb02baeaa9", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Детская обувь &gt; обувь (малыши 0-2 )  для малышей &gt; Балетки для малышей
            {
                "90a33b458f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Платья для девочек &gt;
            {
                "97f54c30d3", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Платья для девочек &gt;
            {
                "b86554c335", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Юбки для девочек &gt;
            {
                "c1534b1ccb", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Детская обувь &gt; обувь (малыши 0-2 )  для малышей &gt; Сандалии для малышей
            {
                "b7e11cbf69", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Юбки для девочек &gt;
            {
                "7da014d638", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Нижнее белье для мальчиков &gt;
            {
                "6421e954f4", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Брюки и легинсы для девочек &gt; Брюки для девочек
            {
                "8fd2139c2f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Комплект из двух предметов для мальчиков &gt;
            {
                "7fd6aecc6d", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Шорты для мальчиков &gt;
            {
                "1a814c3ca2", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Шорты для девочек &gt;
            {
                "fd945e1726", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Футболки и майки для девочек &gt; Футболки для девочек
            {
                "33591530e2", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Джинсовая одежда для мальчиков &gt; Джинсы для мальчиков
            {
                "0de08e74e9", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Платья для малышей &gt;
            {
                "3112746a35", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Домашняя одежда для девочек &gt;
            {
                "4ffd8efddd", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Наборы для малышей &gt;
            {
                "e17b91a8ec", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 84, 191 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (школьники 7-16) &gt; Детские балетки
            {
                "e1ee81d6b0", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 84, 191 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (школьники 7-16) &gt; Детские кроссовки
            {
                "297f78f30c", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Домашняя одежда для мальчиков &gt;
            {
                "f0dfe93cfc", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Блузы для девочек &gt;
            {
                "cac65b2cd8", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Комбинезоны для малышей &gt; комбинезоны для малышей
            {
                "d3e14b31a7", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Футболки и майки для девочек &gt; Майки для девочек
            {
                "bf365aac31", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Рубашки для мальчиков &gt;
            {
                "61534e046a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; трикотажные изделия для малышей &gt; трикотажные комбинезоны для малышей
            {
                "4a9e99f7de", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Комбинезоны для малышей &gt; Комбинезоны для малышей
            {
                "96292101c8", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Футболки и майки для мальчиков &gt; Майки для мальчиков
            {
                "ee080f6b30", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 84, 191 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (школьники 7-16) &gt; Детские сандалии
            {
                "7b0ec38d63", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Комбинезоны для девочек &gt;
            {
                "6fa250d965", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Шорты для мальчиков &gt;
            {
                "1cf4a8b946", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Футболки для мальчиков &gt; Майка для мальчиков
            {
                "97f2390406", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Рубашки для мальчиков &gt;
            {
                "71c8c1472d", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Брюки и легинсы для девочек &gt; Брюки для девочек
            {
                "d7a594f794", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Комбинезоны для малышей &gt; Боди для малышей
            {
                "fa0e0b41e3", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (дошкольники 1-6) &gt; Детские балетки
            {
                "0a892cac9a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Домашняя одежда для мальчиков &gt;
            {
                "7dadd84e3b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Купальники для девочек &gt;
            {
                "35e91d0ca9", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Купальники для девочек &gt;
            {
                "bbcb386057", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (дошкольники 1-6) &gt; Детские кроссовки
            {
                "d4f46bb062", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Купальник для мальчиков &gt;
            {
                "d58f9e194e", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Комбинезоны для девочек &gt;
            {
                "9e21c2cd5b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Жакеты и пальто для девочек &gt; Куртки для девочек
            {
                "f9968f3e0d", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Брюки и легинсы для девочек &gt; Леггинсы для девочек
            {
                "dd265e8d8a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Свитеры и кардиганы для девочек &gt; Трикотажные комплекты для девочек
            {
                "998b5b95b9", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Детская обувь &gt; обувь (малыши 0-2 )  для малышей &gt; Кроссовки для малышей
            {
                "89af18944a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Спортивная одежда для девочек &gt; Спортивные топы для девочек
            {
                "d6073bcc11", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Свитеры и кардиганы для девочек &gt; Трикотажные платья для девочек
            {
                "14a1f9db4a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Топы для малышей &gt;
            {
                "ce9ddfaafa", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Толстовки для девочек &gt;
            {
                "2090b870c3", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Брюки и легинсы для девочек &gt; Леггинсы для девочек
            {
                "3368827426", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Свитеры и кардиганы для девочек &gt; Трикотажные платья для девочек
            {
                "ad9e6621bf", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Свитеры и кардиганы для девочек &gt; Свитера для девочек
            {
                "1fff7889af", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Нарядное платье для маленьких девочек &gt;
            {
                "745533db62", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Рубашки поло для мальчиков &gt;
            {
                "39d64ed316", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Детские плавки &gt;
            {
                "31e7e893f0", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Футболки и майки для девочек &gt; майки для девочек
            {
                "1543ff8178", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Фотоальбом для младенцев &gt; Костюмы для фотографии новорожденных
            {
                "40315e0de3", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Нижнее белье для девочек &gt; Трусики для девочек
            {
                "40e9893011", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Брюки для мальчиков &gt;
            {
                "c243b95e56", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовые юбки для девочек
            {
                "35b256f9fb", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; купальник для малышей &gt;
            {
                "e4bdcdfad2", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Свитшоты для мальчиков &gt;
            {
                "d33e44c744", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Свитшоты для девочек &gt;
            {
                "c5e8b22c86", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Комбинезоны для мальчиков &gt;
            {
                "7d4dd42a63", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые юбки для девочек
            {
                "2f0311989f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Нижнее белье для девочек &gt;
            {
                "38d0a12443", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Спортивные брюки для мальчиков &gt;
            {
                "56d623515b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Спортивные брюки для девочек &gt;
            {
                "302219dc1e", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (дошкольники 1-6) &gt; Детские тапочки
            {
                "fd43e34d06", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Трикотажные изделия для мальчиков &gt; Свитеры для мальчиков
            {
                "65f374e7a6", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые комбинезоны для девочек
            {
                "69eba692c1", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Комбинезоны для мальчиков &gt;
            {
                "0227aebb8b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовые комбинезоны для девочек
            {
                "72ff3383c5", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Нарядное платье для девочек &gt;
            {
                "dab677b194", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Спортивные штаны для маленьких мальчиков &gt;
            {
                "e2f2dbe899", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Деним для мальчиков &gt; Джинсовые комбинезоны для мальчиков
            {
                "bb3c789b08", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Кимоно для девочек &gt;
            {
                "9939a2e77f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые костюмы для девочек
            {
                "23740d2222", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Детская обувь &gt; обувь (малыши 0-2 )  для малышей &gt; Ботинки для малышей
            {
                "1b439a8b39", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Жакеты и пальто для мальчиков &gt; Пальто для мальчиков
            {
                "a3ec06fb06", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 84, 191 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (школьники 7-16) &gt; Детские тапочки
            {
                "a8f4bcf1cb", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые платья для девочек
            {
                "42fd230be0", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Жакеты и пальто для мальчиков &gt; Куртки для мальчиков
            {
                "bf103baac1", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Свитеры и кардиганы для девочек &gt; Кардиганы для девочек
            {
                "296512b2f7", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (дошкольники 1-6) &gt; Детские сандалии
            {
                "a7255d009a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Брюки для мальчиков &gt;
            {
                "8afc6b128f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Деним для мальчиков &gt; Джинсовые шорты для мальчиков
            {
                "3ddc6a9a0f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; нарядное платье для малышей &gt;
            {
                "09c4367b93", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовые шорты для девочек
            {
                "154ee72b68", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Куртки и пальто для девочек &gt; Пальто для девочек
            {
                "60c720f718", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Жакеты и пальто для девочек &gt; Пальто для девочек
            {
                "f8697e789d", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Свитеры и кардиганы для девочек &gt; Свитеры для девочек
            {
                "212fb1b32b", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 84, 191 ) )
            }, // Дети &gt; Детская обувь &gt; Детская обувь (школьники 7-16) &gt; Детские ботинки
            {
                "9e4d6b8c71", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Низ для малышей &gt;
            {
                "e3e13f42f1", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Куртки и пальто для мальчиков &gt; Куртки для мальчиков
            {
                "d4f922854a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Спальный мешок для малышей &gt;
            {
                "4877b779e0", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Толстовки для мальчиков &gt;
            {
                "c1141d8340", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Свитеры и кардиганы для девочек &gt; Трикотажные комплекты для девочек
            {
                "087b517d56", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые шорты для девочек
            {
                "579d4108b1", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Джинсовая одежда для мальчиков &gt; Джинсовые шорты для мальчиков
            {
                "13b8ad7045", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Куртки и пальто для мальчиков &gt; Пальто для мальчиков
            {
                "b4a9ea635a", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Нижнее белье для мальчиков &gt;
            {
                "f49a275f60", new ShopCategoryProperties( ProductType.Footwear, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Детская обувь &gt; обувь (малыши 0-2 )  для малышей &gt; Тапочки для малышей
            {
                "f492fe9459", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Куртки и пальто для малышей &gt; куртки для малышей
            {
                "148e3dbbac", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовые платья для девочек
            {
                "6a13f82370", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовый костюм для девочек
            {
                "995c2acb88", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Трикотажные изделия для мальчиков &gt; Кардиганы для мальчиков
            {
                "b56c3ec1bd", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые куртки и пальто для девочек
            {
                "8ef32aae77", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Свитеры и кардиганы для девочек &gt; Кардиганы для девочек
            {
                "22f8b1568f", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Деним для мальчиков &gt; Джинсовые топы для мальчиков
            {
                "83f80e5f98", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Нижнее белье для девочек &gt; Бюстгальтер и трусики для девочек
            {
                "9ab6f6be2e", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Спортивная одежда для девочек &gt; Спортивные комплекты для девочек
            {
                "47d32a016d", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; свитшоты для малышей &gt;
            {
                "ecf3284eba", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Комбинезоны для малышей &gt; Детская пижама
            {
                "98e7b303c5", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Фотоальбом для младенцев &gt; Костюм с дизайном  для малышей
            {
                "6dfb44bd7b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Трикотажная одежда для мальчиков &gt; Свитеры для мальчиков
            {
                "06cbf183b0", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Джинсовая одежда для мальчиков &gt; Джинсовые комбинезоны для мальчиков
            {
                "2d0dde0852", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; трикотажные изделия для малышей &gt; трикотажные платья для малышей
            {
                "4c322c812d", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Спортивная одежда для девочек &gt; Спортивный низ для девочек
            {
                "30ef16ade4", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Куртки и пальто для девочек &gt; Куртки для девочек
            {
                "7648f9a01b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Деним для девочек &gt; Джинсовые топы для девочек
            {
                "b31da61996", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 0, 24 ) )
            }, // Дети &gt; Малыши &gt; Куртки и пальто для малышей &gt; пальто  для малышей
            {
                "0ea6951ddc", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Спортивная одежда для девочек &gt; Комбинезон для девочек
            {
                "2b653a523b", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; девочка-подросток &gt; Спортивная одежда для девочек &gt; Спортивный низ для девочек
            {
                "769d55571e", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для мальчиков &gt; Джинсовая одежда для мальчиков &gt; Джинсовые топы для мальчиков
            {
                "bc2092dac0", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 12, 72 ) )
            }, // Дети &gt; мальчик-подросток &gt; Трикотажная одежда для мальчиков &gt; Трикотажные брюки для мальчиков
            {
                "93bd33ac24", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовые куртки и пальто для девочек
            {
                "8410b17163", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            }, // Дети &gt; Одежда для девочек &gt; Нижнее белье для девочек &gt; Нижняя рубашка для девочек
            {
                "4ba5a82bf4", new ShopCategoryProperties( ProductType.Clothing, range: new AgeRange( 84, 168 ) )
            } // Дети &gt; Одежда для девочек &gt; Деним для девочек &gt; Джинсовые топы для девочек
        };

        public SheinCategoryProperties()
            : base( Properties ) { }
    }
}