
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Helpers;
using Common.Workers;

using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Helpers;
using TheStore.Api.Front.Data.Repositories;

namespace WorkWithTags
{
    class Program
    {

        private const int AddDate = 20210709;
        private static Regex _enWords = new ( "[a-zA-Z]", RegexOptions.Compiled );

        static void Main( string[] args )
        {
            // TypeOne();
            // TypeTwo();
            // FixTags();
            // CloneTags( 20815000, 41715000, ( "кроссовки", "сникерсы" ) );
            CloneTags( 31301030, 31301040, ( "пуховики", "шубы" ) );
            Console.WriteLine( "Ok!" );
        }

        private static void CloneTags( int from, int to, (string,string)? replace )
        {
            var repository = new TagsRepository( SettingsBuilder.GetDbSettings() );
            var worker = new TagsCloner( repository, "Женские", "для девочек" );
            var result = worker.CopyTags( from, to, replace );
            Console.WriteLine( $"Cloned { result }" );
        }
        
        private static void TypeTwo()
        {
            const string ToChange = "носки";
            const string ForWhat = "майки";
            using var repository = RepositoryFabric.GetTagsRepository();
            var tags = repository.GetTagsByCategory( 41612010 );
            var newTags = tags.Select( t => CreateNewTag( t, ToChange, ForWhat, 41612012 ) ).ToList();
            repository.AddNewTags( newTags );
        }

        private static TagDb CreateNewTag( TagDb tag, string toChange, string forWhat, int newCatetory ) =>
            new TagDb {
                AddDate = AddDate,
                Name = tag.Name,
                Exclude = tag.Exclude,
                Name2 = tag.Name2,
                Menu = tag.Menu,
                LatinName = tag.LatinName,
                LatinName2 = tag.LatinName2,
                Sex = tag.Sex,
                CategoryId = newCatetory,
                SearchFields = tag.SearchFields,
                Enabled = true,
                Important = false,
                Specify = tag.Specify,
                H1 = ProcessText( tag.H1, toChange, forWhat ),
                Title = ProcessText( tag.Title, toChange, forWhat )
            };

        private static string ProcessText( string text, string toChange, string forWhat )
        {
            var newText = text.ToLower().Replace( toChange, forWhat );
            var upperFirst = Helper.ToUpperFirstLetter( newText );
            return upperFirst;
        }
        
        private static void TypeOne()
        {
            foreach( var setting in _settings ) {
                ProcessTags( setting );
            }
        }

        private static void ProcessTags( string name )
        {
            using var repository = RepositoryFabric.GetTagsWorkRepository();
            var category = GetCategory( repository, name );
            var clearTags = GetClearedTags( name, category.Id, repository );
            repository.AddNewTags( clearTags );
            Console.WriteLine( $"{name} - {category.Id}" );
        }

        private static List<TagDb> GetClearedTags( string name, int categoryId, TagsWorkRepository repository ) {
            const string conditionBoy = "для мальчиков";
            const string conditionMan = "мужские";
            var tags = GetTags( ( name, categoryId ), conditionBoy, repository );
            tags.AddRange( GetTags( ( name, categoryId ), conditionMan, repository ) );
            return tags.Where( t => t.Title.ToLower().Contains( "детские" ) == false ).Distinct().ToList();
        }
        
        private static CategoryDb GetCategory( TagsWorkRepository repository, string name )
        {
            var categories = repository.GetCategories( name[ 1.. ] )
                .Where( c => c.Id > 39999999 && c.Id < 49999999 );
            return categories.FirstOrDefault();
        }
        
        private static List<TagDb> GetTags( (string, int) settings, string condition1, TagsWorkRepository repository ) {
            
            var ( condition2, categoryId ) = settings;
            return repository.GetOtherTags( condition1, condition2 ).Select( t => Convert( t, categoryId )  ).ToList();
        }
        
        private static TagDb Convert( OtherTagDb otherTag, int categoryId )
        {
            var title = otherTag.Title.ToLower();
            
            title = title.Replace( "жилетки", "жилеты" );
            
            if( title.Contains( "мужские" ) ) {
                title = title.Replace( "мужские ", string.Empty ) + " для мальчиков";
            }
            
            title = char.ToUpper( title[ 0 ] ) + title.Substring( 1 );

            var tag = new TagDb {
                AddDate = AddDate,
                Name = string.Join(
                    ",",
                    otherTag.Rules.Split( ',' ).Where( rule => _enWords.IsMatch( rule ) == false ) ),
                Menu = otherTag.UnitValue,
                H1 = title,
                Title = title,
                LatinName = TransliterationHelper.Translit(otherTag.UnitValue),
                CategoryId = categoryId,
                Enabled = true,
                Important = false,
                SearchFields = "name,model,typeprefix,categoryName,param",
                Specify = string.Empty
            };
            tag.LatinName2 = tag.LatinName;

            return tag;
        }

        private static void FixTags()
        {
            // 41610012
            
            const string ToChange = "пижамы";
            const string ForWhat = "халаты";
            
            using var repository = RepositoryFabric.GetTagsRepository();
            var tags = repository.GetTagsByCategory( 41611010 );
            tags.ForEach(
                t => {
                    t.H1 = ProcessText( t.H1, ToChange, ForWhat );
                    t.Title = ProcessText( t.Title, ToChange, ForWhat );
                });
            repository.SaveChanges();
        }
        
        private static List<string> _settings = new() {
            // ( "пальто", 41601010 ),    // пальто
            // ( "куртки", 41601021 ),    // Куртки
            // ( "ветровки", 41601022 ),  // Ветровки
            // ( "парки", 41601024 ),     // Дубленки
            // ( "жилетки", 41601025 ),   // Парки
            // ( "бомберы", 41601026 ),   // Жилеты
            // "плащи",      // Плащи
            // "шубы", // пусто
            // "дождевик",
            // "пуховик",
            // "тренчкот", // пусто 
            // "бушлат",
            // "пиджак",
            // "рубашк",
            // "водолаз",
            // "кардиган",
            // "свитеры"
            // "джемпер"
            // "пуловер"
            // "футболк"  // подставляется ощая категория. нужно руками обновлять
            // "поло"     // подставляется ощая категория. нужно руками обновлять
            // "лонгслив"
            // "смокинг"
            // "костюм" // подставляется ощая категория. нужно руками обновлять
            // "комбинезон"
            // "джинсы"
            // "шорты"
            // "толстовк"
            // "свитшот"
            // "худи" // пусто
            // "олимпийки" // пусто
            // "флиски"  // пусто
            "брюки"
        };
        
        
    }
}