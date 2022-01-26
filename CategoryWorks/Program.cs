using Admitad.Converters.Workers;
using Common.Entities;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Helpers;

namespace CategoryWorks
{
    class Program
    {

        //яндекс маркет
        private const int ShopId = 121;

        private static Dictionary<string, ShopCategory> _categoriesById;
        private static Dictionary<string, List<ShopCategory>> _categoriesByParentId;

        static void Main(string[] args) {

            FillCache();

            // электроника
            // CreateCategories( 90000000, "198119");
            // бытовая техника
            // CreateCategories(100000000, "198118");
            // UpdateCategoriesNames();
         }

        private static void UpdateCategoriesNames() {
            var repository = RepositoryFabric.GetCategoryMapRepository();
            var maps = repository.GetByShopId(ShopId);
            foreach( var map in maps) {
                FillOriginName(map);
            }
            repository.Update(maps);
        }

        private static void FillOriginName( CategoryMapDb map) {
            var category = _categoriesById[map.ShopCategoryId];
            //map.OriginalName = category.Name;
            map.OriginalParentId = category.ParentId;
        }

        private static void CreateCategories(int startId, string categoryShopId) {

            var category = GetCategory(categoryShopId);
            var packs = CreatePacks(category, ref startId);

            WriteToDbCategories(packs);
            CreateAndWriteMaps(packs);
        }

        private static void CreateAndWriteMaps(List<CategoryPack> packs) {
            var maps = packs.Select(CreateMap).ToList();
            var repository = RepositoryFabric.GetCategoryMapRepository();
            repository.Write(maps);
        }

        private static CategoryMapDb CreateMap(CategoryPack pack) => new CategoryMapDb {
                ShopId = ShopId,
                LocalCategoryId = pack.LocalCatetory.Id,
                ShopCategoryId = pack.ShopCategory.Id,
                OriginalName = pack.ShopCategory.Name
            };


        private static void WriteToDbCategories(List<CategoryPack> packs) {

            var categories = packs.Select(p => p.LocalCatetory).ToList();
            var repository = RepositoryFabric.GetCategoryRepository();

            repository.AddCategories(categories);
        }

        private static List<CategoryPack> CreatePacks(ParentShopCategory shopCategory, ref int id, int parentId = 0, int level = 0) {

            var currentId = id;
            var list = new List<CategoryPack>();

            level++;
            
            list.Add(CreateCategory(shopCategory, ref id, parentId, level));

            id++;
            
            foreach ( var child in shopCategory.Children) {
                list.AddRange(CreatePacks(child, ref id, currentId, level));
            }
            
            return list;
        }

        private static CategoryPack CreateCategory(ParentShopCategory shopCategory, ref int id, int parentId, int level) =>
            new CategoryPack {
                    LocalCatetory = new CategoryDb
                    {
                        Id = id,
                        Enabled = true,
                        ParentId = parentId,
                        Name = shopCategory.Name,
                        Name2 = shopCategory.Name,
                        Name3 = shopCategory.Name,
                        H1 = shopCategory.Name,
                        H1Vin = shopCategory.Name,
                        LatinName = TransliterationHelper.Translit(shopCategory.Name),
                        LatinName2 = TransliterationHelper.Translit(shopCategory.Name).Replace("_", "-"),
                        Level = level,
                        Image = string.Empty,
                        Search = string.Empty,
                        SearchFields = string.Empty,
                        SearchGender = string.Empty,
                        SearchMinus = string.Empty,
                        Unit = string.Empty,
                        Sex = string.Empty,
                        SearchSpecify = string.Empty,
                        SearchMinusFields = string.Empty
                    },
                    ShopCategory = shopCategory
            };

        private static void FillCache() {
            var downloadInfo = new ReducedDownloadsInfo {
                NameLatin = "beru",
                ShopWeight = 10,
                //!!!!!!!!!!!!!!!!!!!!!!!!!!
                //FilePath = "g:\\admitadFeeds\\beru.xml"
            };

            var info = new ParsingInfo( "g:\\admitadFeeds\\beru.xml", 10, "beru" );
            
            var parser = new GeneralParser(info, new Common.Api.BackgroundBaseContext("", ""));
            var shopInfo = parser.Parse(true);

            _categoriesById = shopInfo.Categories;
            _categoriesByParentId = _categoriesById.Values.Where(c=>c.ParentId != null).GroupBy(c => c.ParentId).ToDictionary(k => k.Key, v => v.ToList());
        }

        private static ParentShopCategory GetCategory(string id) {
            var category = ParentShopCategory.Create(_categoriesById[id]);
            category.Children = GetChildren(id);
            return category;
        }

        private static List<ParentShopCategory> GetChildren(string id) {
            if(_categoriesByParentId.ContainsKey(id) == false) {
                return new List<ParentShopCategory>();
            }

            var children = _categoriesByParentId[id].Select(ParentShopCategory.Create).ToList();
            foreach( var child in children) {
                child.Children = GetChildren(child.Id);
            }

            return children;
        }
    }
}
